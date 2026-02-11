using Appfinanças.Models;

namespace Appfinanças;

public partial class LoginPage : ContentPage
{
    DatabaseService db;

    public LoginPage()
    {
        InitializeComponent();

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
        db = new DatabaseService(dbPath);
    }

    private async void Login_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(UsuarioEntry.Text) ||
            string.IsNullOrWhiteSpace(SenhaEntry.Text))
        {
            await DisplayAlert("Erro!", "Digite usuário e senha.", "Ok");
            return;
        }

        var usuario = await db.GetUsuarioAsync(
            UsuarioEntry.Text,
            SenhaEntry.Text);

        
        if (usuario != null)
        {
          
            if (usuario.Salario > 0)
            {
                await Navigation.PushAsync(new MainPage(usuario));
            }
            else
            {
                await Navigation.PushAsync(new SetupFinanceiro(usuario));
            }

            return;
        }

        
        bool cadastrar = await DisplayAlert(
            "Usuário não encontrado",
            "Deseja cadastrar?",
            "Sim",
            "Não");

        if (!cadastrar)
        {
            await DisplayAlert("Aviso",
                "Você precisa ter cadastro para entrar.",
                "Ok");
            return;
        }

        var novoUsuario = new Usuario()
        {
            Nome = UsuarioEntry.Text,
            Senha = SenhaEntry.Text,
            Salario = 0,
            Gastos = 0,
            Saldo = 0
        };

        await db.AddUsuarioAsync(novoUsuario);

       
        usuario = await db.GetUsuarioAsync(
            UsuarioEntry.Text,
            SenhaEntry.Text);

        await DisplayAlert("Sucesso", "Cadastro realizado!", "Ok");

        await Navigation.PushAsync(new SetupFinanceiro(usuario));
    }

    private async void Excluir_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(UsuarioEntry.Text) ||
            string.IsNullOrWhiteSpace(SenhaEntry.Text))
        {
            await DisplayAlert("Erro", "Digite usuário e senha.", "Ok");
            return;
        }

        var usuario = await db.GetUsuarioAsync(
            UsuarioEntry.Text,
            SenhaEntry.Text);

        if (usuario == null)
        {
            await DisplayAlert("Erro", "Usuário não encontrado.", "Ok");
            return;
        }

        bool confirmar = await DisplayAlert(
            "Confirmação",
            "Tem certeza que deseja excluir sua conta?",
            "Sim",
            "Não");

        if (confirmar)
        {
            await db.LimparTransacoesAsync(usuario.Id);
            await db.ExcluirUsuarioAsync(usuario.Id);

            await DisplayAlert("Sucesso", "Conta excluída!", "OK");

            UsuarioEntry.Text = "";
            SenhaEntry.Text = "";
        }
    }

}
