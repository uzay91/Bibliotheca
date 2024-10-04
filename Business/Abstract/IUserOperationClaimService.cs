using Core.Concrete.Entities;
using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserOperationClaimService
    {
        Task<IDataResult<IEnumerable<UserOperationClaim>>> ListByOperationClaimId(Guid operationClaimId);
        Task<IDataResult<IEnumerable<UserOperationClaim>>> ListByUserId(Guid userId);
        Task<IDataResult<UserOperationClaim>> GetByUserOperationClaimId(Guid userOperationClaimId);
        Task<IDataResult<UserOperationClaim>> Add(UserOperationClaim userOperationClaim);// +
        Task<IDataResult<UserOperationClaim>> Update(UserOperationClaim userOperationClaim);
        Task<IResult> Delete(Guid id);
    }
}
