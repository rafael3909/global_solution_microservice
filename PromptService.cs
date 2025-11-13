using MyApp.Domain;
using MyApp.Infra;

namespace MyApp.Services
{
    public interface IPromptService
    {
        Task<IEnumerable<Prompt>> GetAllAsync();
        Task<Prompt?> GetByIdAsync(Guid id);
        Task<Prompt> CreateAsync(Prompt prompt);
        Task UpdateAsync(Guid id, Prompt prompt);
        Task DeleteAsync(Guid id);
    }

    public class PromptService : IPromptService
    {
        private readonly IPromptRepository _repo;
        public PromptService(IPromptRepository repo) => _repo = repo;

        public Task<IEnumerable<Prompt>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Prompt?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

        public async Task<Prompt> CreateAsync(Prompt prompt)
        {
            prompt.Id = Guid.NewGuid();
            prompt.CreatedAt = DateTime.UtcNow;
            await _repo.CreateAsync(prompt);
            return prompt;
        }

        public async Task UpdateAsync(Guid id, Prompt prompt)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException("Prompt not found");
            existing.Title = prompt.Title;
            existing.Content = prompt.Content;
            existing.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(existing);
        }

        public Task DeleteAsync(Guid id) => _repo.DeleteAsync(id);
    }
}
