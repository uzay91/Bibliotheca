using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bibliotheca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet()]
        public async Task<IActionResult> List()
        {
            var result = await _bookService.List();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("book")]
        public async Task<IActionResult> ListByBookName(string bookName) 
        {
            var result = await _bookService.ListByBookName(bookName);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("genre")]
        public async Task<IActionResult> ListByGenreId([FromQuery] Guid genreId)
        {
            var result = await _bookService.ListByGenreId(genreId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }


        [HttpGet("author")]
        public async Task<IActionResult> ListByAuthor([Required] string author)
        {
            var result = await _bookService.ListByAuthor(author);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }


        [HttpGet("book/{bookName}/author/{author}")]
        public async Task<IActionResult> GetByBookNameAndAuthor(string bookName, string author)
        {
            var result = await _bookService.GetByBookNameAndAuthor(bookName, author);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        // Add metodu Http Post türündedir
        [HttpPost()]
        public async Task<IActionResult> Add(Book book)
        {
            var result = await _bookService.Add(book);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.Status, result);
        }

        
        [HttpPut()]
        public async Task<IActionResult> Update(Book book)
        {
            var result = await _bookService.Update(book);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPatch()]
        public async Task<IActionResult> DeActive(Guid id)
        {
            var result = await _bookService.DeActive(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _bookService.Delete(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

   
    }
}
