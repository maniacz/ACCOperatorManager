using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccOperatorManager
{
    public class UserValidator
    {
        private readonly IConfiguration config;

        public UserValidator(IConfiguration config)
        {
            this.config = config;
        }

        public bool IsUserAllowedToAccess()
        {
            var windowsUserNameWithDomain = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            var windowsUserName = windowsUserNameWithDomain.Substring(windowsUserNameWithDomain.LastIndexOf(@"\") + 1);
            Console.WriteLine(windowsUserName);
            return config.GetSection("AllowedUsers").Get<List<string>>().Contains(windowsUserName);
        }
    }
}
