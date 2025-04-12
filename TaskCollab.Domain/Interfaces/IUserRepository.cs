using TaskCollab.Domain.Entities;

namespace TaskCollab.Domain.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<User> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
}