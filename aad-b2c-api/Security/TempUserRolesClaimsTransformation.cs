using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using aad_b2c_api.Data;
using aad_b2c_api.Models;

namespace aad_b2c_api.Security
{  
    public class TempUserRolesClaimsTransformation : IClaimsTransformation
    {
        // POC implementation of IClaimsTransformation to fetch user roles from the 
        // database and add them to the user's identity as additional claims. B2C 
        // does not support role-based authorization and Microsoft considers it a 
        // poor practice to mix policy-based authorization with the identity provider.
        // See: https://gunnarpeipman.com/aspnet-core-adding-claims-to-existing-identity/

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

            // in production call database with objectIdentifierClaim.Value but for 
            // this POC using a fake database. Only Natalie Ramsey <ramseyn@yahoo.com>
            // with oid 9e73ccd3-b194-4c83-ae13-769fa5dbef19 has valid roles.
            List<ClaimDTO> roles = new();
            if (objectIdentifierClaim.Value.Equals("9e73ccd3-b194-4c83-ae13-769fa5dbef19"))
            {
                roles = FakeDatabase.Claims.ToList();
            }

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
