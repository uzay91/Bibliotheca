using Business.Abstract;
using Business.Constants;
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
    public class BookManager : IBookService
    {
        private readonly IBookDal _bookDal;
        public BookManager(IBookDal bookDal)
        {
            _bookDal = bookDal;
        }

        public async Task<IDataResult<Book>> Add(Book book)
        {
            var isExist = await _bookDal.GetAsync(x => x.Name == book.Name && x.Author == book.Author);
            //var list = await _bookDal.GetListAsync();
            //list.Where(x => x.Author == "uzay");
            //var listAsQuery = _bookDal.Query().Where(x => x.Name == "Harry Potter");
            //var bookCount = await _bookDal.GetCountAsync(x => x.Name == "Harry Potter");

            if (isExist != null)
            {
                return new ErrorDataResult<Book>(409, Message.AlreadyExist);
            }
            var bookToAdd = new Book()
            {
                GenreId = book.GenreId,
                Name = book.Name.ToLower(),
                Author = book.Author.ToLower(),
                Description = book.Description,
                Edition = book.Edition.ToLower(),
                TotalPage = book.TotalPage,
            };
            var result = _bookDal.Add(bookToAdd);
            await _bookDal.SaveChangesAsync();

            return new SuccessDataResult<Book>(result);
        }

        public async Task<IDataResult<Book>> Update(Book book)
        {
            var bookToUpdate = await _bookDal.GetAsync(x => x.Id == book.Id);
            if (bookToUpdate == null)
            {
                return new ErrorDataResult<Book>(404, Message.NotFound);
            }
            bookToUpdate.GenreId = book.GenreId;
            bookToUpdate.Name = book.Name.ToLower();
            bookToUpdate.Author = book.Author.ToLower();
            bookToUpdate.Description = book.Description;
            bookToUpdate.Edition = book.Edition.ToLower();
            bookToUpdate.TotalPage = book.TotalPage;

            var result = _bookDal.Update(bookToUpdate);
            await _bookDal.SaveChangesAsync();
            return new SuccessDataResult<Book>(result, 200);
        }

        public async Task<IResult> DeActive(Guid id)
        {
            var bookToDeactive = await _bookDal.GetAsync(x => x.Id == id);
            if (bookToDeactive == null)
            {
                return new ErrorResult(404, Message.NotFound);
            }
            bookToDeactive.IsActive = false;
            _bookDal.Update(bookToDeactive);
            await _bookDal.SaveChangesAsync();
            return new SuccessResult();
        }

        public async Task<IResult> Delete(Guid id)
        {
            var bookToDelete = await _bookDal.GetAsync(x => x.Id == id);
            if (bookToDelete == null)
            {
                return new ErrorResult(404, Message.NotFound);
            }
            _bookDal.Delete(bookToDelete);
            await _bookDal.SaveChangesAsync();
            return new SuccessResult();
        }

        public async Task<IDataResult<Book>> GetByBookNameAndAuthor(string bookName, string author)
        {
            var result = await _bookDal.GetAsync(x => x.Name == bookName.ToLower() && x.Author == author.ToLower());
            if (result == null)
            {
                return new ErrorDataResult<Book>(404, Message.NotFound);
            }
            return new SuccessDataResult<Book>(result);
        }

        public async Task<IDataResult<IEnumerable<Book>>> List()
        {
            var result = await _bookDal.GetListAsync();
            return new SuccessDataResult<IEnumerable<Book>>(result);
        }

        public async Task<IDataResult<IEnumerable<Book>>> ListByAuthor(string author)
        {
            var result = await _bookDal.GetListAsync(x => x.Author.ToLower() == author.ToLower());
            return new SuccessDataResult<IEnumerable<Book>>(result);
        }

        public async Task<IDataResult<IEnumerable<Book>>> ListByBookName(string bookName)
        {
            var result = await _bookDal.GetListAsync(x => x.Name.ToLower() == bookName.ToLower());
            if (result == null)
            {
                return new ErrorDataResult<IEnumerable<Book>>(404, Message.NotFound);
            }

            return new SuccessDataResult<IEnumerable<Book>>(result);
        }

        public async Task<IDataResult<IEnumerable<Book>>> ListByGenreId(Guid genreId)
        {
            var result = await _bookDal.GetListAsync(x => x.GenreId == genreId);
            if (result == null)
            {
                return new ErrorDataResult<IEnumerable<Book>>(404, Message.NotFound);
            }

            return new SuccessDataResult<IEnumerable<Book>>(result);
        }

        public async Task<IDataResult<IEnumerable<Book>>> ListByTotalPage(int totalPage, string comparison)
        {
            var result = new List<Book>();
            if (comparison == "greaterthan")
            {
                result = (await _bookDal.GetListAsync(x => x.TotalPage >= totalPage)).ToList();
            }
            else if (comparison == "lessthan")
            {
                result = (await _bookDal.GetListAsync(x => x.TotalPage < totalPage)).ToList();
            }
            if (result == null)
            {
                return new ErrorDataResult<IEnumerable<Book>>(404, Message.NotFound);
            }

            return new SuccessDataResult<IEnumerable<Book>>(result);
        }

    }
}
