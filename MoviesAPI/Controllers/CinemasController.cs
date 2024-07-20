using AutoMapper;
using MoviesAPI.Data.Dtos;
using MoviesAPI.Data;
using MoviesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CinemasController : ControllerBase
    {
        private MovieContext _context;
        private IMapper _mapper;

        public CinemasController(MovieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCinemas([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var cinemas = _context.Cinemas
                .Skip(skip)
                .Take(take)
                .Include(c => c.Address)
                .ToList();

            var cinemaDtos = _mapper.Map<List<ReadCinemaDto>>(cinemas);

            return Ok(cinemaDtos);
        }


        [HttpGet("{id}")]
        public IActionResult GetCinemaByID(int id)
        {
            Cinema? cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
            if (cinema != null)
            {
                ReadCinemaDto cinemaDto = _mapper.Map<ReadCinemaDto>(cinema);
                return Ok(cinemaDto);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult CreateCinema([FromBody] CreateCinemaDto cinemaDto)
        {
            Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCinemaByID), new { Id = cinema.Id }, cinemaDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCinema(int id, [FromBody] UpdateCinemaDto cinemaDto)
        {
            Cinema? cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }
            _mapper.Map(cinemaDto, cinema);
            _context.SaveChanges();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteCinema(int id)
        {
            Cinema? cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }
            _context.Remove(cinema);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
