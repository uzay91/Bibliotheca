using Business.Abstract;
using Core.Concrete.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimDal _userOperationClaimDal;
        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal)
        {
            _userOperationClaimDal = userOperationClaimDal;
        }

        public async Task<IDataResult<UserOperationClaim>> Add(UserOperationClaim userOperationClaim)
        {
            var isExist = await _userOperationClaimDal.GetAsync(x => x.UserId == userOperationClaim.UserId
            && x.OperationClaimId == userOperationClaim.OperationClaimId);

            if (isExist != null)
            {
                return new ErrorDataResult<UserOperationClaim>(409, "Record already exists!");
            }
            var useroperationclaimToAdd = new UserOperationClaim()
            {
                UserId = userOperationClaim.UserId,
                OperationClaimId = userOperationClaim.OperationClaimId,
            };

            var result = _userOperationClaimDal.Add(useroperationclaimToAdd);
            await _userOperationClaimDal.SaveChangesAsync();
            return new SuccessDataResult<UserOperationClaim>(result);
        }

        public async Task<IResult> Delete(Guid id)
        {
            var useroperationclaimToDelete = await _userOperationClaimDal.GetAsync(x => x.Id == id);
            if (useroperationclaimToDelete == null)
            {
                return new ErrorResult(404, "Record not found!");
            }
            _userOperationClaimDal.Delete(useroperationclaimToDelete);
            await _userOperationClaimDal.SaveChangesAsync();
            return new SuccessResult();
        }

        public async Task<IDataResult<UserOperationClaim>> GetByUserOperationClaimId(Guid userOperationClaimId)
        {
            var result = await _userOperationClaimDal.GetAsync(x => x.Id == userOperationClaimId);
            if (result == null)
            {
                return new ErrorDataResult<UserOperationClaim>(404, "Record not Found");
            }

            return new SuccessDataResult<UserOperationClaim>(result);
        }

        public async Task<IDataResult<IEnumerable<UserOperationClaim>>> ListByOperationClaimId(Guid operationClaimId)
        {
            var result = await _userOperationClaimDal.GetListAsync(x => x.OperationClaimId == operationClaimId);
            if (result == null)
            {
                return new ErrorDataResult<IEnumerable<UserOperationClaim>>(404, "Record not Found");
            }

            return new SuccessDataResult<IEnumerable<UserOperationClaim>>(result);
        }

        public async Task<IDataResult<IEnumerable<UserOperationClaim>>> ListByUserId(Guid userId)
        {
            var result = await _userOperationClaimDal.GetListAsync(x => x.UserId == userId);
            if (result == null)
            {
                return new ErrorDataResult<IEnumerable<UserOperationClaim>>(404, "Record not Found");
            }

            return new SuccessDataResult<IEnumerable<UserOperationClaim>>(result);
        }

        public async Task<IDataResult<UserOperationClaim>> Update(UserOperationClaim userOperationClaim)
        {
            var userOperationClaimToUpdate = await _userOperationClaimDal.GetAsync(x => x.Id == userOperationClaim.Id);
            if (userOperationClaimToUpdate == null)
            {
                return new ErrorDataResult<UserOperationClaim>(404, "Record not found!");
            }
            userOperationClaimToUpdate.UserId = userOperationClaim.UserId;
            userOperationClaimToUpdate.OperationClaimId = userOperationClaim.OperationClaimId;
            var result = _userOperationClaimDal.Update(userOperationClaimToUpdate);
            await _userOperationClaimDal.SaveChangesAsync();
            return new SuccessDataResult<UserOperationClaim>(result);
        }
    }
}
