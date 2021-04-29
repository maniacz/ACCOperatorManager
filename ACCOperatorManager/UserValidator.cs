using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccOperatorManager
{
    //todo: Przerobić na statyczną klasę aby dostęp do metody IsUserAllowedToAccess był jak do metody statycznej w całym programie
    public class UserValidator
    {
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public UserValidator(IConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public bool IsUserAllowedToAccess()
        {
            var windowsUserNameWithDomain = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            var windowsUserName = windowsUserNameWithDomain.Substring(windowsUserNameWithDomain.LastIndexOf(@"\") + 1);
            Console.WriteLine(windowsUserName);
            logger.LogInformation($"Fetched windows username was: {windowsUserName}");
            return config.GetSection("AllowedUsers").Get<List<string>>().Contains(windowsUserName);
        }
    }
}
