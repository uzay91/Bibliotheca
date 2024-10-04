using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotheca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;
        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;

        }

        [HttpGet()]
        public async Task<IActionResult> List()
        {
            var result = await _genreService.List();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("Genre")]
        public async Task<IActionResult> GetByGenreName( string genreName)
        {
            var Result = await _genreService.GetByGenreName(genreName);
            if (Result.Success) 
            {
                return Ok(Result);
            }
            return BadRequest();
        
        }

        [HttpPost()]
        public async Task<IActionResult> Add(Genre genre) 
        {
            var result = await _genreService.Add(genre);
            if (result.Success) 
            {
                return Ok(result);
            }
            return StatusCode(result.Status, result);
        
        }

        [HttpPut()]
        public async Task<IActionResult> Update(Genre genre) 
        {
            var result = await _genreService.Update(genre);
            if (result.Success) 
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpDelete()]
        private async Task<IActionResult> Delete(Guid id) 
        {
            var result = await _genreService.Delete(id);
            if (result.Success) 
            {
                return Ok(result);
            }
            return NotFound(result);
        }  
          
    }
    
    }

