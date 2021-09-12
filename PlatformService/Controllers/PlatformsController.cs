using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
  [Route("api/platforms")]
  [ApiController]
  public class PlatformsController : ControllerBase
  {
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;

    public PlatformsController(IPlatformRepo repository, IMapper mapper)
    {
     _repository = repository;
     _mapper = mapper;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
     var platforms = _repository.GetAllPlatforms();

     return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms)); 
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
     var platform = _repository.GetPlatformById(id);

     if (platform == null) {
       return NotFound();
     }

     return Ok(_mapper.Map<PlatformReadDto>(platform)); 
    }

    [HttpPost]
    public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
      var platform = _mapper.Map<Platform>(platformCreateDto);
      _repository.CreatePlatform(platform);
      _repository.saveChanges();

      var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
      
      return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformReadDto.Id}, platformReadDto);
    }
  }
}