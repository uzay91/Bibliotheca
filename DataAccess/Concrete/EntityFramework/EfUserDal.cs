using Core.Concrete.Dtos;
using Core.Concrete.Entities;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepository<User, PostgreSqlDbContext>, IUserDal
    {
        public EfUserDal(PostgreSqlDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<UserOperationClaimDto>> GetClaims(User user)
        {
            var result = await (from operationClaim in Context.OperationClaims
                                join userOperationClaim in Context.UserOperationClaims
                                     on operationClaim.Id equals userOperationClaim.OperationClaimId
                                where userOperationClaim.UserId == user.Id
                                select new UserOperationClaimDto { Id = operationClaim.Id, Role = operationClaim.Role }).ToListAsync();
            return result;
        }

    }
}
