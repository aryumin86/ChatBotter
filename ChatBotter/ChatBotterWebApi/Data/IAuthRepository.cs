using System;
using System.Threading.Tasks;
using CBLib.Entities;

namespace ChatBotterWebApi.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);

        Task<User> Login(string login, string password);

        Task<bool> UserExists(string username);
    }
}
