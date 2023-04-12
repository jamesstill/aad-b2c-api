using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using aad_b2c_api.Data;
using aad_b2c_api.Models;

namespace aad_b2c_api.Security
{  
    public class UserRolesClaimsTransformation : IClaimsTransformation
    {
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            await Task.CompletedTask;
            
            if (principal.Identity?.IsAuthenticated is false)
            {
                return principal;
            }

            var objectIdentifierClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (objectIdentifierClaim == null)
            {
                return principal;
            }

            // DEMO CODE ONLY using a fake database
            List<ClaimDTO> roles = FakeDatabase.Claims.ToList();

            // clone principal
            var clonedPrincipal = principal.Clone();
            var clonedIdentity = clonedPrincipal.Identity as ClaimsIdentity;

            foreach (var role in roles)
            {
                var claim = new Claim(ClaimTypes.Role, role.Name);
                clonedIdentity?.AddClaim(claim);
            }

            return clonedPrincipal;
        }
    }
}
