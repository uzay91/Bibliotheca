using Core.Concrete.Dtos;
using Core.Concrete.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        Task<IEnumerable<UserOperationClaimDto>> GetClaims(User user);
    }
}
