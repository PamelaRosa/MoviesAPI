using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Data;
using MoviesAPI.Data.Dtos;
using MoviesAPI.Models;

namespace MoviesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{

    private MovieContext _context;
    private IMapper _mapper;

    public MoviesController(MovieContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetMovies([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        IEnumerable<Movie> filteredMovies = _context.Movies.Skip(skip).Take(take);
        IEnumerable<ReadMovieDto> moviesDto = _mapper.Map<List<ReadMovieDto>>(filteredMovies);

        return Ok(moviesDto);
    }

    [HttpGet("{id}")]
    public IActionResult GetMovieByID(int id)
    {
        Movie? movie = _context.Movies.FirstOrDefault(f => f.Id == id);

        if(movie == null) return NotFound(new { Message = "Filme não encontrado" });
        var movieDto = _mapper.Map<ReadMovieDto>(movie);

        return Ok(movieDto);
    }

    [HttpPost]
    public IActionResult CreateMovie([FromBody] CreateMovieDto movieDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Movie movie = _mapper.Map<Movie>(movieDto);

        _context.Movies.Add(movie);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetMovieByID), 
            new { Id = movie.Id}, 
            movie);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateMovie(int id, [FromBody] UpdateMovieDto movieDto)
    {
        var movie = _context.Movies.FirstOrDefault(f => f.Id == id);
        if(movie == null) return NotFound(new { Message = "Filme não encontrado" });

        _mapper.Map(movieDto, movie);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult UpdatePartialMovie(int id, 
        JsonPatchDocument<UpdateMovieDto> patch)
    {
        var movie = _context.Movies.FirstOrDefault(f => f.Id == id);
        if (movie == null) return NotFound(new { Message = "Filme não encontrado" });


        var movieToUpdate = _mapper.Map<UpdateMovieDto>(movie);

        patch.ApplyTo(movieToUpdate, ModelState);

        if(!TryValidateModel(movieToUpdate))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(movieToUpdate, movie);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMovie(int id)
    {
        var movie = _context.Movies.FirstOrDefault(f => f.Id == id);
        if (movie == null) return NotFound(new { Message = "Filme não encontrado" });

        _context.Remove(movie);
        _context.SaveChanges();

        return NoContent();
    }
}
