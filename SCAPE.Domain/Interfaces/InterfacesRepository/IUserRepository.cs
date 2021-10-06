using SCAPE.Domain.Entities;
using System.Threading.Tasks;

namespace SCAPE.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> insertUser(User user);
        Task<User> findUserByEmail(string email);
        Task<User> getUser(User user);
        Task<bool> editUser(string email, string password, string role);
        Task<bool> deleteuser(string email);
    }
}
