using Appfinanças.Models;

namespace Appfinanças;

public partial class MainPage : ContentPage
{
    DatabaseService db;
    Usuario usuario;

    public MainPage(Usuario usuarioLogado)
    {
        InitializeComponent();

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
        db = new DatabaseService(dbPath);

        usuario = usuarioLogado;

        resultadoLabel.Text = $"Saldo atual: R$ {usuario.Saldo:F2}";

        _ = CarregarExtrato();
    }


    private async void AdicionarEntrada_Clicked(object sender, EventArgs e)
    {
        if (decimal.TryParse(EntradaEntry.Text, out decimal valor))
        {
            usuario.Saldo += valor;

            await db.AddTransacaoAsync(new Transacao
            {
                UsuarioId = usuario.Id,
                Tipo = "Entrada",
                Valor = valor,
                Data = DateTime.Now,
                Descricao = string.IsNullOrWhiteSpace(DescricaoEntry.Text)
                 ? "Entrada"
        :       DescricaoEntry.Text
            }); 

            await db.UpdateUsuarioAsync(usuario);

            resultadoLabel.Text = $"Saldo atual: R$ {usuario.Saldo:F2}";
            EntradaEntry.Text = "";
            DescricaoEntry.Text = "";


            await CarregarExtrato();
        }
        else
        {
            await DisplayAlert("Erro", "Digite um valor válido!", "Ok");
        }
    }

    private async void AdicionarGasto_Clicked(object sender, EventArgs e)
    {
        if (decimal.TryParse(GastoEntry.Text, out decimal valor))
        {
            usuario.Saldo -= valor;

            await db.AddTransacaoAsync(new Transacao
            {
                UsuarioId = usuario.Id,
                Tipo = "Saída",
                Valor = valor,
                Data = DateTime.Now,
                Descricao = string.IsNullOrWhiteSpace(DescricaoEntry.Text) 
                ? "Saída" : DescricaoEntry.Text
            });

            await db.UpdateUsuarioAsync(usuario);

            resultadoLabel.Text = $"Saldo atual: R$ {usuario.Saldo:F2}";
            GastoEntry.Text = "";
            DescricaoEntry.Text = "";
           

            await CarregarExtrato();
        }
        else
        {
            await DisplayAlert("Erro", "Digite um valor válido!", "Ok");
        }
    }

    private async Task CarregarExtrato()
    {
        var transacoes = await db.GetTransacoesAsync(usuario.Id);
        ExtratoCollection.ItemsSource = transacoes;

    }

    private async void  LimparExtrato_Clicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert(
        "Confirmação",
        "Deseja realmente apagar todo o extrato?",
        "Sim",
        "Não");

        if (confirmar)
        {
            await db.LimparTransacoesAsync(usuario.Id);

            await CarregarExtrato();
        }
    }
}
