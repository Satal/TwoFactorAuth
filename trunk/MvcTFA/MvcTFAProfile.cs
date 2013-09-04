using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using WebMatrix.WebData;

namespace MvcTFA
{
    public class MvcTFAProfile : ProfileBase
    {
        private const string SecretKeyPropName = "SecretKey";
        private const string UsesTwoFactorPropName = "UsesTwoFactorAuthentication";

        public static MvcTFAProfile GetCurrent()
        {
            return GetProfile(WebSecurity.CurrentUserName);
        }

        public static MvcTFAProfile GetProfile(string userId)
        {
            return (MvcTFAProfile)Create(userId);
        }

        /// <summary>
        /// The Two Factor secret key for the user
        /// </summary>
        public string SecretKey
        {
            get { return base[SecretKeyPropName] as string; }
            set
            {
                base[SecretKeyPropName] = value;
                Save();
            }
        }

        /// <summary>
        /// Whether or not the user uses two factor authentication on their profile
        /// </summary>
        public bool UsesTwoFactorAuthentication
        {
            get
            {
                object rtn = base[UsesTwoFactorPropName];

                if (!(rtn is bool))
                {
                    rtn = false;
                }

                return (bool)rtn;
            }
            set
            {
                base[UsesTwoFactorPropName] = value;
                Save();
            }
        }
    }
}