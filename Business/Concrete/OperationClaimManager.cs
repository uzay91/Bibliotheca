using Business.Abstract;
using Core.Concrete.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IOperationClaimDal _operationclaimDal;
        public OperationClaimManager(IOperationClaimDal operationclaimDal)
        {
            _operationclaimDal = operationclaimDal;
        }
        public async Task<IDataResult<OperationClaim>> Add(OperationClaim operationClaim)
        {
            var isExist = await _operationclaimDal.GetAsync(x => x.Role == operationClaim.Role);
            if (isExist != null)
            {
                return new ErrorDataResult<OperationClaim>(409, "Record already exists!");
            }

            var operationClaimToAdd = new OperationClaim()
            {
                Role = operationClaim.Role,
            };

            var result = _operationclaimDal.Add(operationClaimToAdd);
            await _operationclaimDal.SaveChangesAsync();
            return new SuccessDataResult<OperationClaim>(result);
        }

        public async Task<IResult> Delete(Guid id)
        {
            var operationClaim = await _operationclaimDal.GetAsync(x => x.Id == id);
            if (operationClaim == null)
            {
                return new ErrorResult(404, "Operation claim not found!");
            }

            _operationclaimDal.Delete(operationClaim);
            await _operationclaimDal.SaveChangesAsync();

            return new SuccessResult("Operation claim deleted successfully.");
        }

        public async Task<IDataResult<OperationClaim>> GetByRole(string role)
        {
            var operationClaim = await _operationclaimDal.GetAsync(x => x.Role == role);
            if (operationClaim == null)
            {
                return new ErrorDataResult<OperationClaim>(404, "Operation claim not found for the specified role.");
            }

            return new SuccessDataResult<OperationClaim>(operationClaim);
        }

        public async Task<IDataResult<IEnumerable<OperationClaim>>> List()
        {
            var operationClaims = await _operationclaimDal.GetListAsync();
            return new SuccessDataResult<IEnumerable<OperationClaim>>(operationClaims);
        }

        public async Task<IDataResult<OperationClaim>> Update(OperationClaim operationClaim)
        {
            var isExistOperationClaim = await _operationclaimDal.GetAsync(x => x.Id == operationClaim.Id);
            if (isExistOperationClaim == null)
            {
                return new ErrorDataResult<OperationClaim>(404, "Operation claim not found!");
            }

            isExistOperationClaim.Role = operationClaim.Role;

            _operationclaimDal.Update(isExistOperationClaim);
            await _operationclaimDal.SaveChangesAsync();

            return new SuccessDataResult<OperationClaim>(isExistOperationClaim);
        }
    }
}
