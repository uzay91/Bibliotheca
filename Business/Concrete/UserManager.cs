using Business.Abstract;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Concrete.Dtos;
using Core.Concrete.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        //[CacheRemoveAspect()]
        public async Task<IDataResult<User>> Add(User user)
        {
            var userIsExist = await _userDal.GetAsync(x => x.TcNo == user.TcNo);
            if (userIsExist != null)
            {
                return new ErrorDataResult<User>(409, Message.AlreadyExist);
            }
            

            var userToAdd = new User()
            {
                TcNo = user.TcNo,
                FullName = user.FullName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                RefreshToken = user.RefreshToken,
                ResetToken = user.ResetToken,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
            };
            var result = _userDal.Add(userToAdd);
            await _userDal.SaveChangesAsync();
            return new SuccessDataResult<User>(result);
        }

        //[CacheRemoveAspect()]
        public async Task<IDataResult<User>> Deactive(Guid id)
        {
            var userToDeactive = await _userDal.GetAsync(x => x.Id == id);
            if (userToDeactive == null)
            {
                return new ErrorDataResult<User>(404, "Record not found!");
            }
            userToDeactive.IsActive = false;
            _userDal.Update(userToDeactive);
            await _userDal.SaveChangesAsync();
            return new SuccessDataResult<User>();
        }

        //[CacheRemoveAspect()]
        public async Task<IResult> Delete(Guid id)
        {
            var userToDelete = await _userDal.GetAsync(x => x.Id == id);
            if (userToDelete == null)
            {
                return new ErrorResult(404, "Record not found!");
            }
            _userDal.Delete(userToDelete);
            await _userDal.SaveChangesAsync();
            return new SuccessResult();
        }

        public async Task<IDataResult<User>> GetByEmail(string email)
        {
            var result = await _userDal.GetAsync(x => x.Email == email);
            if (result == null)
            {
                return new ErrorDataResult<User>(404, Message.NotFound);
            }

            return new SuccessDataResult<User>(result);
        }

        public async Task<IDataResult<User>> GetById(Guid id)
        {
            var result = await _userDal.GetAsync(x => x.Id == id);
            if (result == null)
            {
                return new ErrorDataResult<User>(404, Message.NotFound);
            }
            return new SuccessDataResult<User>(result);
        }

        public async Task<IDataResult<User>> GetByRefreshToken(string refreshToken, Guid userId)
        {
            var result = await _userDal.GetAsync(x => x.RefreshToken == refreshToken && x.Id == userId);
            if (result == null)
            {
                return new ErrorDataResult<User>(404, "Record not Found");
            }
            return new SuccessDataResult<User>(result);
        }

        public async Task<IDataResult<User>> GetByResetToken(string resetToken)
        {
            var result = await _userDal.GetAsync(x => x.ResetToken == resetToken);
            if (result == null)
            {
                return new ErrorDataResult<User>(404, "Record not Found");
            }
            return new SuccessDataResult<User>(result);
        }

        public async Task<IDataResult<User>> GetByTcNo(string tcNo)
        {
            var result = await _userDal.GetAsync(x => x.TcNo == tcNo);
            if (result == null)
            {
                return new ErrorDataResult<User>(404, "Record not Found");
            }
            return new SuccessDataResult<User>(result);
        }

        public async Task<IDataResult<User>> GetByUsername(string username)
        {
            var result = await _userDal.GetAsync(x => x.UserName == username);
            if (result == null)
            {
                return new ErrorDataResult<User>(404, "Record not Found");
            }
            return new SuccessDataResult<User>(result);
        }

        public async Task<IDataResult<IEnumerable<UserOperationClaimDto>>> GetClaims(User user)
        {
            var result = await _userDal.GetClaims(user);
            return new SuccessDataResult<IEnumerable<UserOperationClaimDto>>(result);
        }

        //[CacheAspect(duration:120)]
        public async Task<IDataResult<IEnumerable<User>>> List()
        {
            var result = await _userDal.GetListAsync();
            return new SuccessDataResult<IEnumerable<User>>(result);
        }


        public async Task<IDataResult<IEnumerable<User>>> ListByAgeGreaterThan(int age)
        {
            var result = new List<User>();
            var users = await _userDal.GetListAsync();
            foreach (var user in users)
            {
                var currentUsersAge = DateTime.Now.Year - user.BirthDate.Year;
                if (currentUsersAge > age)
                {
                    result.Add(user);
                }

            }
            return new SuccessDataResult<IEnumerable<User>>(result);
        }

        //[CacheRemoveAspect()]
        public async Task<IDataResult<User>> Update(User user)
        {
            var userToUpdate = await _userDal.GetAsync(x => x.Id == user.Id);
            if (userToUpdate == null)
            {
                return new ErrorDataResult<User>(404, "Record not found!");
            }
            userToUpdate.TcNo = user.TcNo;
            userToUpdate.FullName = user.FullName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.BirthDate = user.BirthDate;
            userToUpdate.Email = user.Email;
            userToUpdate.PhoneNumber = user.PhoneNumber;
            userToUpdate.UserName = user.UserName;
            userToUpdate.RefreshToken = user.RefreshToken;
            userToUpdate.ResetToken = user.ResetToken;
            userToUpdate.PasswordHash = user.PasswordHash;
            userToUpdate.PasswordSalt = user.PasswordSalt;


            var result = _userDal.Update(userToUpdate);
            await _userDal.SaveChangesAsync();
            return new SuccessDataResult<User>(result, 200);
        }
    }
}
