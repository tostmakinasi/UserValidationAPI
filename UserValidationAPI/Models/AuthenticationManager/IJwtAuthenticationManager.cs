using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserValidationAPI.Models.AuthenticationManager
{
    public interface IJwtAuthenticationManager
    {
        string Authentication(string username, string password);
    }
}
