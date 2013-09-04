using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcTFA.Models
{
    public class UserProfileModel
    {
        public string AppName { get; set; }

        public string SecretKey { get; set; }

        public bool UsesTwoFactor { get; set; }
    }
}