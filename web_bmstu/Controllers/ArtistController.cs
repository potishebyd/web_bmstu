using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using web_bmstu.DTO;
using web_bmstu.ModelsBL;
using web_bmstu.Models;
using web_bmstu.Enums;
using web_bmstu.Services;
using System.Linq;
using AutoMapper;
using web_bmstu.ModelsConverters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace web_bmstu.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("/api/v1/artists")]
    public class ArtistController : Controller
    {

        private IArtistService artistService;
        private IMapper mapper;
        private ArtistConverters artistConverters;
        private readonly ILogger<ArtistController> _logger;

        public ArtistController(IArtistService artistService, IMapper mapper,
                              ArtistConverters artistConverters, ILogger<ArtistController> logger)
        {
            this.artistService = artistService;
            this.mapper = mapper;
            this.artistConverters = artistConverters;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Artist>), StatusCodes.Status200OK)]
        public IActionResult GetAll(
            [FromQuery] ArtistFilterDto filter,
            [FromQuery] ArtistSortState? sortState
        )
        {
            Console.WriteLine(filter.Name);
            _logger.LogInformation("Artists (Request: GET)");
            return Ok(mapper.Map<IEnumerable<ArtistDto>>(artistService.GetAll(filter, sortState)));
        }

        // [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ArtistDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public IActionResult Add(ArtistBaseDto artistDto)
        {
            try
            {
                var addedArtist = artistService.Add(mapper.Map<ArtistBL>(artistDto));
                return Ok(mapper.Map<ArtistDto>(addedArtist));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ArtistDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public IActionResult Put(int id, ArtistBaseDto artist)
        {
            try
            {
                var updatedArtist = artistService.Update(mapper.Map<ArtistBL>(artist,
                        o => o.AfterMap((src, dest) => dest.Id = id)));

                return updatedArtist != null ? Ok(mapper.Map<ArtistDto>(updatedArtist)) : NotFound();
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // [HttpPatch("{id}")]
        // [ProducesResponseType(typeof(ArtistDto), StatusCodes.Status200OK)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        // public IActionResult Patch(int id, ArtistBaseDto artist)
        // {
        //     try
        //     {
        //         var updatedArtist = artistService.Update(artistConverters.convertPatch(id, artist));
        //         return updatedArtist != null ? Ok(mapper.Map<ArtistDto>(updatedArtist)) : NotFound();
        //     }
        //     catch (Exception ex)
        //     {
        //         return Conflict(ex.Message);
        //     }
        // }

        // [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ArtistDto), StatusCodes.Status200OK)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var deletedArtist = artistService.Delete(id);
            return deletedArtist != null ? Ok(mapper.Map<ArtistDto>(deletedArtist)) : NotFound();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ArtistDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var artist = artistService.GetByID(id);
            return artist != null ? Ok(mapper.Map<ArtistDto>(artist)) : NotFound();
        }
    }
}