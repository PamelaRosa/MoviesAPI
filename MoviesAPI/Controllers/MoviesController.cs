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

    /// <summary>
    /// Recupera uma lista paginada de todos os filmes cadastrados no banco de dados.
    /// </summary>
    /// <param name="skip">Número de itens a serem pulados.</param>
    /// <param name="take">Número de itens a serem retornados.</param>
    /// <returns>Uma lista paginada de filmes.</returns>
    /// <response code="200">Caso a leitura seja feita com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetMovies([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        IEnumerable<Movie> filteredMovies = _context.Movies.Skip(skip).Take(take);
        IEnumerable<ReadMovieDto> moviesDto = _mapper.Map<List<ReadMovieDto>>(filteredMovies);

        return Ok(moviesDto);
    }

    /// <summary>
    /// Recupera um filme específico pelo seu ID.
    /// </summary>
    /// <param name="id">O ID do filme a ser recuperado.</param>
    /// <returns>O filme correspondente ao ID fornecido.</returns>
    /// <response code="200">Caso o filme seja encontrado e retornado com sucesso.</response>
    /// <response code="404">Caso o filme com o ID fornecido não seja encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetMovieByID(int id)
    {
        Movie? movie = _context.Movies.FirstOrDefault(f => f.Id == id);

        if(movie == null) return NotFound(new { Message = "Filme não encontrado" });
        var movieDto = _mapper.Map<ReadMovieDto>(movie);

        return Ok(movieDto);
    }

    /// <summary>
    /// Adiciona um novo filme ao banco de dados.
    /// </summary>
    /// <param name="movieDto">Objeto contendo os dados necessários para criar um novo filme.</param>
    /// <returns>Uma resposta indicando o resultado da operação de criação.</returns>
    /// <response code="201">Caso o filme seja criado com sucesso. Retorna o filme criado com um link para sua visualização.</response>
    /// <response code="400">Caso os dados fornecidos sejam inválidos.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Atualiza os dados de um filme existente no banco de dados.
    /// </summary>
    /// <param name="id">O ID do filme a ser atualizado.</param>
    /// <param name="movieDto">Objeto contendo os dados atualizados do filme.</param>
    /// <returns>Uma resposta indicando o resultado da operação de atualização.</returns>
    /// <response code="204">Caso a atualização seja realizada com sucesso.</response>
    /// <response code="404">Caso o filme com o ID fornecido não seja encontrado.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateMovie(int id, [FromBody] UpdateMovieDto movieDto)
    {
        var movie = _context.Movies.FirstOrDefault(f => f.Id == id);
        if(movie == null) return NotFound(new { Message = "Filme não encontrado" });

        _mapper.Map(movieDto, movie);
        _context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Atualiza parcialmente os dados de um filme existente no banco de dados.
    /// </summary>
    /// <param name="id">O ID do filme a ser atualizado.</param>
    /// <param name="patch">Objeto contendo as operações de patch para atualizar parcialmente o filme.</param>
    /// <returns>Uma resposta indicando o resultado da operação de atualização parcial.</returns>
    /// <response code="204">Caso a atualização parcial seja realizada com sucesso.</response>
    /// <response code="400">Caso ocorra um erro de validação dos dados após a aplicação do patch.</response>
    /// <response code="404">Caso o filme com o ID fornecido não seja encontrado.</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Remove um filme do banco de dados pelo seu ID.
    /// </summary>
    /// <param name="id">O ID do filme a ser removido.</param>
    /// <returns>Uma resposta indicando o resultado da operação de remoção.</returns>
    /// <response code="204">Caso a remoção seja realizada com sucesso.</response>
    /// <response code="404">Caso o filme com o ID fornecido não seja encontrado.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteMovie(int id)
    {
        var movie = _context.Movies.FirstOrDefault(f => f.Id == id);
        if (movie == null) return NotFound(new { Message = "Filme não encontrado" });

        _context.Remove(movie);
        _context.SaveChanges();

        return NoContent();
    }
}
