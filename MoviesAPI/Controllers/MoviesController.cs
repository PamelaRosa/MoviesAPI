using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Data;
using MoviesAPI.Models;

namespace MoviesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{

    private MovieContext _context;

    public MoviesController(MovieContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetMovies([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        IEnumerable<Movie> filteredMovies = _context.Movies.Skip(skip).Take(take);

        return Ok(filteredMovies);
    }

    [HttpGet("{id}")]
    public IActionResult GetMovieByID(int id)
    {
        Movie? movie = _context.Movies.FirstOrDefault(f => f.Id == id);

        if(movie == null)
        {
            return NotFound();
        }

        return Ok(movie);
    }

    [HttpPost]
    public IActionResult CreateMovie([FromBody] Movie movie)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Movies.Add(movie);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetMovieByID), 
            new { Id = movie.Id}, 
            movie);
    }
}
