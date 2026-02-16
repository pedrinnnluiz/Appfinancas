
﻿using Appfinanças.Models;

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

﻿
       namespace Appfinanças
    {
        public partial class MainPage : ContentPage
        {
            int saldo;
            int entrada;
            int totalGastos;
            List<string> extrato = new();

            List<int> diasRecebimento = new() { 15, 30 };

            public MainPage()
            {
                InitializeComponent();

                int salario = Preferences.Get("Salario", 0);
                int gastosFixos = Preferences.Get("Gastos", 0);

            saldo = salario - gastosFixos;

            string data = DateTime.Today.ToString("dd/MM");  
                
                resultadoLabel.Text = $"Saldo inicial: {saldo}";
            }

        private void CalcularSaldo_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(EntradaEntry.Text, out int valor))
            {
                saldo += valor;


                extrato.Clear();

                string data = DateTime.Today.ToString("dd/MM");

                
                extrato.Add($"{data} 💰 Valor de entrada: {saldo}");
                resultadoLabel.Text = $"Saldo restante: {saldo}";

                ExtratoView.ItemsSource = null;
                ExtratoView.ItemsSource = extrato;
            }
        }

            private void AdicionarGasto_Clicked(object sender, EventArgs e)
            {
            if (int.TryParse(GastoEntry.Text, out int valor))
            {
                saldo -= valor;

                string data = DateTime.Today.ToString("dd/MM");
                extrato.Add($"{data} 💸 Gasto: -{valor}");
                Preferences.Set("Extrato", string.Join("|", extrato));

                string extratoSalvo = Preferences.Get("Extrato", "");
                if (!string.IsNullOrEmpty(extratoSalvo)) 
                { extrato = extratoSalvo.Split('|').ToList(); ExtratoView.ItemsSource = extrato; }

                resultadoLabel.Text = $"Saldo restante: {saldo}";

                    ExtratoView.ItemsSource = null;
                    ExtratoView.ItemsSource = extrato;

                    GastoEntry.Text = "";
                }

            }
        }
    }

