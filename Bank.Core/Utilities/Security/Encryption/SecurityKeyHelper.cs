using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.Utilities.Security.Encryption
{
    public class SecurityKeyHelper
    {
        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(securityKey);


            if (keyBytes.Length < 64)  
            {
                var newKey = new byte[64];
                Array.Copy(keyBytes, newKey, keyBytes.Length);  
                keyBytes = newKey;
            }

            return new SymmetricSecurityKey(keyBytes);
        }
    }
}
