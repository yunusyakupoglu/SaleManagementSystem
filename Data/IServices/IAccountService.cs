using Data.Models;

namespace Data.IServices
{
    public interface IAccountService
    {
        Login Login(string Username, string Password);
        void Register(User user);
        User GetCookieUser(string username);
        CheckAuthEntities Check();
    }
}
