using Appfinanças.Models;

using SQLite;

public class DatabaseService
{
    readonly SQLiteAsyncConnection _database;

    public DatabaseService(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);

        _database.CreateTableAsync<Usuario>().Wait();
        _database.CreateTableAsync<Transacao>().Wait();
    }

    public Task<int> AddUsuarioAsync(Usuario usuario)
    {
        return _database.InsertAsync(usuario);
    }

    public Task<int> UpdateUsuarioAsync(Usuario usuario)
    {
        return _database.UpdateAsync(usuario);
    }

    public Task<Usuario> GetUsuarioAsync(string nome, string senha)
    {
        return _database.Table<Usuario>()
            .Where(u => u.Nome == nome && u.Senha == senha)
            .FirstOrDefaultAsync();
    }

    public Task<int> AddTransacaoAsync(Transacao transacao)
    {
        return _database.InsertAsync(transacao);
    }

    public Task<List<Transacao>> GetTransacoesAsync(int usuarioId)
    {
        return _database.Table<Transacao>()
            .Where(t => t.UsuarioId == usuarioId)
            .OrderByDescending(t => t.Data)
            .ToListAsync();
    }

    public Task<int> LimparTransacoesAsync(int usuarioId)
    {
        return _database.Table<Transacao>()
            .Where(t => t.UsuarioId == usuarioId)
            .DeleteAsync();
    }

    public Task<int> ExcluirUsuarioAsync(int id)
    {
        return _database.DeleteAsync<Usuario>(id);
    }



}
