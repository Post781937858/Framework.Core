using System.Collections.Generic;
using System.Security.Claims;

namespace Framework.Core.Common
{
    public interface IUser
    {
        string Name { get; }
        int ID { get; }

        string Role { get; }

        bool IsAuthenticated();

        IEnumerable<Claim> GetClaimsIdentity();

        List<string> GetClaimValueByType(string ClaimType);

        string GetToken();
        List<string> GetUserInfoFromToken(string ClaimType);
    }
}
