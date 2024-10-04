using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class GenreManager : IGenreService
    {
        private readonly IGenreDal _genreDal;
        public GenreManager(IGenreDal genreDal)
        {
            _genreDal = genreDal;
        }

        [SecuredOperation("admin,manager")]
        public async Task<IDataResult<Genre>> Add(Genre genre)
        {
            var isExist = await _genreDal.GetAsync(x => x.Name == genre.Name);
            if (isExist != null)
            {
                return new ErrorDataResult<Genre>(409, "Record already exists!");
            }
            var genreToAdd = new Genre()
            {
                Name = genre.Name,
            };
            var result = _genreDal.Add(genreToAdd);
            await _genreDal.SaveChangesAsync();
            return new SuccessDataResult<Genre>(result);
        }

        [SecuredOperation("admin,manager")]
        public async Task<IDataResult<Genre>> Update(Genre genre)
        {
            var genreToUpdate = await _genreDal.GetAsync(x => x.Id == genre.Id);
            if (genreToUpdate == null)
            {
                return new ErrorDataResult<Genre>(404, "Record not found!");
            }
            genreToUpdate.Name = genre.Name;


            var result = _genreDal.Update(genreToUpdate);
            await _genreDal.SaveChangesAsync();
            return new SuccessDataResult<Genre>(result, 200);
        }

        [SecuredOperation("admin,manager")]
        public async Task<IResult> DeActive(Guid id)
        {
            var genreToDeactive = await _genreDal.GetAsync(x => x.Id == id);
            if (genreToDeactive == null)
            {
                return new ErrorResult(404, "Record not found!");
            }
            genreToDeactive.IsActive = false;
            _genreDal.Update(genreToDeactive);
            await _genreDal.SaveChangesAsync();
            return new SuccessResult();
        }

        public async Task<IResult> Delete(Guid id)
        {
            var genreToDelete = await _genreDal.GetAsync(x => x.Id == id);
            if (genreToDelete == null)
            {
                return new ErrorResult(404, "Record not found!");
            }
            _genreDal.Delete(genreToDelete);
            await _genreDal.SaveChangesAsync();
            return new SuccessResult();
        }

        public async Task<IDataResult<Genre>> GetByGenreName(string name)
        {
            var result = await _genreDal.GetAsync(x => x.Name == name);
            if (result == null)
            {
                return new ErrorDataResult<Genre>(404, "Record not Found");
            }
            return new SuccessDataResult<Genre>(result);
        }

        public async Task<IDataResult<IEnumerable<Genre>>> List()
        {
            var result = await _genreDal.GetListAsync();
            return new SuccessDataResult<IEnumerable<Genre>>(result);
        }


    }
}
