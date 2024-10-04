using Core.Concrete.Entities;
using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IOperationClaimService
    {
        Task<IDataResult<IEnumerable<OperationClaim>>> List();// +
        Task<IDataResult<OperationClaim>> GetByRole(string role);// +
        Task<IDataResult<OperationClaim>> Add(OperationClaim operationClaim);// +
        Task<IDataResult<OperationClaim>> Update(OperationClaim operationClaim);
        Task<IResult> Delete(Guid id);// +
    }
}
