<<<<<<< HEAD
using Appfinanças.Models;

=======
using Appfinancas.Models;
using Appfinanças.Models;
>>>>>>> f002a4dd645eac68944a8611a0bc0184bbb40749
namespace Appfinanças;

public partial class LoginPage : ContentPage
{
    DatabaseService db;
<<<<<<< HEAD

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
=======
    public LoginPage()
    {
        InitializeComponent();
    }


    private async void Login_Clicked(object sender, EventArgs e)


    {

        if(string.IsNullOrWhiteSpace(UsuarioEntry.Text) || string.IsNullOrWhiteSpace(SenhaEntry.Text))

        {
            await DisplayAlert("Erro!", "Digite um Usuário e senha para prosseguir", "Ok");

            return;
        }   
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
        var db = new DatabaseService(dbPath);

        var usuario = await db.GetUsuarioAsync(UsuarioEntry.Text, SenhaEntry.Text);

        if (usuario != null)
        {
            await Navigation.PushAsync(new SetupFinanceiro());
        }

        else

        {
            bool cadastrar = await DisplayAlert("Usuario não encontrado", "Faça um cadastro", "Sim", "Não");

            if (cadastrar)
            {
                await db.AddUsuarioAsync(new Usuario
                { Nome = UsuarioEntry.Text, Senha = SenhaEntry.Text });

                await DisplayAlert("Cadastro concluído com sucesso", "Sucesso", "OK!");
                await Navigation.PushAsync(new SetupFinanceiro());
            }
            else {
                
                await DisplayAlert("Aviso", "Você precisa ter um cadastro para entrar.", "Ok"); }
        }
    }
            private async void Excluir_Clicked(object sender, EventArgs e) 
    { await db.DeleteUsuarioAsync(UsuarioEntry.Text);
        await DisplayAlert("Conta excluída", "Seu cadastro foi removido.", "Ok"); }
}
   



