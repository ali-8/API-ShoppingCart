using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.DTO_s
{
    public class CredentialDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public string Access_token { get; set; }
        public DateTime Expire_at { get; set; }
    }
}
