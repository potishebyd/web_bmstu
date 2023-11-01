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

namespace db_cp.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("/api/v1/songs")]
    public class SongController : Controller
    {
        private readonly ISongService songService;
        private readonly IPlaylistService playlistService;
        private readonly IMapper mapper;
        private readonly SongConverters songConverters;

        public SongController(ISongService songService, IPlaylistService playlistService,
                                IMapper mapper, SongConverters songConverters)
        {
            this.songService = songService;
            this.playlistService = playlistService;
            this.mapper = mapper;
            this.songConverters = songConverters;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SongDto>), StatusCodes.Status200OK)]
        public IActionResult GetAll(
            [FromQuery] SongFilterDto filter,
            [FromQuery] SongSortState? sortState
        )
        {
            return Ok(mapper.Map<IEnumerable<SongDto>>(songService.GetAll(filter, sortState)));
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(SongDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public IActionResult Add(SongBaseDto songDto)
        {
            try
            {
                var addedSong = songService.Add(mapper.Map<SongBL>(songDto));
                return Ok(mapper.Map<SongDto>(addedSong));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SongDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public IActionResult Put(int id, SongBaseDto song)
        {
            try
            {
                var updatedSong = songService.Update(mapper.Map<SongBL>(song,
                        o => o.AfterMap((src, dest) => dest.Id = id)));

                return updatedSong != null ? Ok(mapper.Map<SongDto>(updatedSong)) : NotFound();
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // [HttpPatch("{id}")]
        // [ProducesResponseType(typeof(SongDto), StatusCodes.Status200OK)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        // public IActionResult Patch(int id, SongBaseDto song)playlist
        // {
        //     try
        //     {
        //         var updatedSong = songService.Update(songConverters.convertPatch(id, song));
        //         return updatedSong != null ? Ok(mapper.Map<SongDto>(updatedSong)) : NotFound();
        //     }
        //     catch (Exception ex)
        //     {
        //         return Conflict(ex.Message);
        //     }
        // }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SongDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var deletedSong = songService.Delete(id);
            playlistService.DeleteSongPlaylistsBySongId(id);

            return deletedSong != null ? Ok(mapper.Map<SongDto>(deletedSong)) : NotFound();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SongDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var song = songService.GetByID(id);
            return song != null ? Ok(mapper.Map<SongDto>(song)) : NotFound();
        }
    }
}