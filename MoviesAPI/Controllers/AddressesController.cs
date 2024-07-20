using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Data.Dtos;
using MoviesAPI.Data;
using MoviesAPI.Models;

namespace MoviesAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class AddressesController : ControllerBase
{


    private MovieContext _context;
    private IMapper _mapper;

    public AddressesController(MovieContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public IEnumerable<ReadAddressDto> GetAddresses()
    {
        return _mapper.Map<List<ReadAddressDto>>(_context.Addresses.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetAddressByID(int id)
    {
        Address? address = _context.Addresses.FirstOrDefault(address => address.Id == id);
        if (address != null)
        {
            ReadAddressDto addressDto = _mapper.Map<ReadAddressDto>(address);
            return Ok(addressDto);
        }
        return NotFound();
    }

    [HttpPost]
    public IActionResult CreateAddress([FromBody] CreateAddressDto addressDto)
    {
        Address? address = _mapper.Map<Address>(addressDto);
        _context.Addresses.Add(address);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAddressByID), new { Id = address.Id }, addressDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateAddress(int id, [FromBody] UpdateAddressDto addressDto)
    {
        Address? address = _context.Addresses.FirstOrDefault(address => address.Id == id);
        if (address == null)
        {
            return NotFound();
        }
        _mapper.Map(addressDto, address);
        _context.SaveChanges();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteAddress(int id)
    {
        Address? address = _context.Addresses.FirstOrDefault(address => address.Id == id);
        if (address == null)
        {
            return NotFound();
        }
        _context.Remove(address);
        _context.SaveChanges();
        return NoContent();
    }

}

}
