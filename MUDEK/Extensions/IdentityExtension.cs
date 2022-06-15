using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Mudek.Extensions
{
    public static class IdentityExtension
    {
        public static string GetId(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst(x => x.Type == "username");
            return claim.Value;
        }

        public static string GetMail(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claim.Value;
        }
    }
}
