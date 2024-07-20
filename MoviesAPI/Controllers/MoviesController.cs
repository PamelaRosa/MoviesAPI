using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models;
using System.Collections.Generic;

namespace MoviesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private static List<Movie> movies = new List<Movie>();
    private static int id = 0;

    [HttpGet]
    public IActionResult GetMovies([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        IEnumerable<Movie> filteredMovies = movies.Skip(skip).Take(take);

        return Ok(filteredMovies);
    }

    [HttpGet("{id}")]
    public IActionResult GetMovieByID(int id)
    {
        Movie? movie = movies.FirstOrDefault(f => f.id == id);
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
        movie.id = id++;
        movies.Add(movie);
        
        return CreatedAtAction(nameof(GetMovieByID), 
            new { id = movie.id}, 
            movie);
    }
}
