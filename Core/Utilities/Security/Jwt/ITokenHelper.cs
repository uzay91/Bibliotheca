using Core.Concrete.Dtos;
using Core.Concrete.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Jwt
{
    public interface ITokenHelper
    {
        AccessToken CreateAccessToken(User user, IEnumerable<UserOperationClaimDto> operationClaims);
        string GenerateRefreshToken();

    }
}
