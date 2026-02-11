
using Appfinanças.Models;

namespace Appfinanças;

public partial class SetupFinanceiro : ContentPage
{
    DatabaseService db;
    Usuario usuario;

    public SetupFinanceiro(Usuario usuarioLogado)
    {
        InitializeComponent();

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db");
        db = new DatabaseService(dbPath);

        usuario = usuarioLogado;
    }

    private async void Avancar_Clicked(object sender, EventArgs e)
    {
        if (!decimal.TryParse(SalarioEntry.Text, out decimal salario) ||
            !decimal.TryParse(GastosEntry.Text, out decimal gastos))
        {
            await DisplayAlert("Erro!", "Digite apenas números válidos!", "Ok");
            return;
        }

        usuario.Salario = salario;
        usuario.Gastos = gastos;
        usuario.Saldo = salario - gastos;

        await db.UpdateUsuarioAsync(usuario);

        await Navigation.PushAsync(new MainPage(usuario));
    }
}
