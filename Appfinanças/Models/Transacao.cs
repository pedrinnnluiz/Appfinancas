using SQLite;

namespace Appfinanças.Models
{
    public class Transacao
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public string Tipo { get; set; }   
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
       public string Descricao { get; set; }

       public string Categoria { get; set; }
    }
}
