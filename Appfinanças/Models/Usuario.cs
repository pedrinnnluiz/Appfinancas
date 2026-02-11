using SQLite;

using Appfinanças.Models;
public class Usuario
{ 

    [PrimaryKey, AutoIncrement]
    public int Id
    {
        get; set;
    }

    public string Nome { get; set; }
    public string Senha { get; set; }

    public decimal Salario { get; set; }
    public decimal Gastos { get; set; }
    public decimal Saldo { get; set; }

    public decimal SaldoInicial { get; set; }
 
}

