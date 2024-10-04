using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBookService
    {
        //Data varsa o zaman IDataResult => geri dönüş değeri varsa  
        //Data yok ise o zaman IResult => sadece işlem yapılıyorsa ve geri dönüş değeri yoksa
        Task<IDataResult<IEnumerable<Book>>> List();
        Task<IDataResult<IEnumerable<Book>>> ListByGenreId(Guid genreId);
        Task<IDataResult<IEnumerable<Book>>> ListByAuthor(string author);
        Task<IDataResult<IEnumerable<Book>>> ListByBookName(string bookName); //bunu sen ekle 
        Task<IDataResult<Book>> GetByBookNameAndAuthor(string bookName, string author);
        Task<IDataResult<IEnumerable<Book>>> ListByTotalPage(int totalPage, string comparison); //bunu ekleme
        Task<IDataResult<Book>> Add(Book book); 
        Task<IDataResult<Book>> Update(Book book);
        Task<IResult> Delete(Guid id);
        Task<IResult> DeActive(Guid id);

    }
}
