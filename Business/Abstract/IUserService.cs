using Core.Concrete.Dtos;
using Core.Concrete.Entities;
using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<IEnumerable<User>>> List();
        Task<IDataResult<IEnumerable<User>>> ListByAgeGreaterThan(int age);
        Task<IDataResult<IEnumerable<UserOperationClaimDto>>> GetClaims(User user);
        Task<IDataResult<User>> GetById(Guid id); // +
        Task<IDataResult<User>> GetByTcNo(string tcNo);// +
        Task<IDataResult<User>> GetByEmail(string email);// +
        Task<IDataResult<User>> GetByUsername(string username);// +
        Task<IDataResult<User>> GetByRefreshToken(string refreshToken, Guid userId);
        Task<IDataResult<User>> GetByResetToken(string resetToken);
        Task<IDataResult<User>> Add(User user);
        Task<IDataResult<User>> Update(User user);// +
        Task<IDataResult<User>> Deactive(Guid id);
        Task<IResult> Delete(Guid id);

    }
}
