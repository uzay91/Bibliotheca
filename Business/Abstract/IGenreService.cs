using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IGenreService
    {
        Task<IDataResult<IEnumerable<Genre>>> List(); // +
        Task<IDataResult<Genre>> GetByGenreName(string name);// +
        Task<IDataResult<Genre>> Add(Genre genre);// +
        Task<IDataResult<Genre>> Update(Genre genre);// +
        Task<IResult> Delete(Guid id);// +
        Task<IResult> DeActive(Guid id);
    }
}
