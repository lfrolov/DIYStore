using DIYStoreWeb.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DIYStoreWeb.Services
{
    public class UserService : IUserService
    {
        protected AuthorizationOptions Authorization { get; }        

        public UserService(IOptions<AuthorizationOptions> authorization) 
        {
            Authorization = authorization.Value;            
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => {
                var user1 = Authorization.Users.SingleOrDefault(user => user.Login == username && user.Password == password);                
                return user1?.WithoutPassword();
                }
            );
            return user;
        }
    }
}
