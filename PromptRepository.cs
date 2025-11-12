using System.Data;
using Dapper;
using MyApp.Domain;

namespace MyApp.Infra
{
    public interface IPromptRepository
    {
        Task<IEnumerable<Prompt>> GetAllAsync();
        Task<Prompt?> GetByIdAsync(Guid id);
        Task CreateAsync(Prompt prompt);
        Task UpdateAsync(Prompt prompt);
        Task DeleteAsync(Guid id);
    }

    public class PromptRepository : IPromptRepository
    {
        private readonly IDbConnectionFactory _dbFactory;
        public PromptRepository(IDbConnectionFactory dbFactory) => _dbFactory = dbFactory;

        public async Task<IEnumerable<Prompt>> GetAllAsync()
        {
            using var conn = _dbFactory.CreateConnection();
            var sql = "SELECT Id, Title, Content, CreatedAt, UpdatedAt FROM Prompts";
            return await conn.QueryAsync<Prompt>(sql);
        }

        public async Task<Prompt?> GetByIdAsync(Guid id)
        {
            using var conn = _dbFactory.CreateConnection();
            var sql = "SELECT Id, Title, Content, CreatedAt, UpdatedAt FROM Prompts WHERE Id = @Id";
            return await conn.QueryFirstOrDefaultAsync<Prompt>(sql, new { Id = id });
        }

        public async Task CreateAsync(Prompt prompt)
        {
            using var conn = _dbFactory.CreateConnection();
            var sql = "INSERT INTO Prompts (Id, Title, Content, CreatedAt, UpdatedAt) VALUES (@Id, @Title, @Content, @CreatedAt, @UpdatedAt)";
            await conn.ExecuteAsync(sql, prompt);
        }

        public async Task UpdateAsync(Prompt prompt)
        {
            using var conn = _dbFactory.CreateConnection();
            var sql = @"UPDATE Prompts SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt WHERE Id = @Id";
            await conn.ExecuteAsync(sql, prompt);
        }

        public async Task DeleteAsync(Guid id)
        {
            using var conn = _dbFactory.CreateConnection();
            var sql = "DELETE FROM Prompts WHERE Id = @Id";
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
