using DIYStoreWeb.Helpers;
using System.Threading.Tasks;

namespace DIYStoreWeb.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
    }
}
