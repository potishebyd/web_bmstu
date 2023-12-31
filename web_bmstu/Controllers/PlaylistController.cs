﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Route("/api/v1/playlists")]
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService playlistService;
        private readonly ISongService songService;
        private readonly IMapper mapper;
        private readonly PlaylistConverters playlistConverters;

        public PlaylistController(IPlaylistService playlistService, ISongService songService,
                               IMapper mapper,
                               PlaylistConverters playlistConverters)
        {
            this.playlistService = playlistService;
            this.songService = songService;
            this.mapper = mapper;
            this.playlistConverters = playlistConverters;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PlaylistDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll(
            [FromQuery] PlaylistSortState? sortState
        )
        {
            return Ok(mapper.Map<IEnumerable<PlaylistDto>>(await playlistService.GetAllAsync(sortState)));
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(PlaylistDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Add(PlaylistBaseDto playlistDto)
        {
            try
            {
                var addedPlaylist = await playlistService.AddAsync(mapper.Map<PlaylistBL>(playlistDto));
                return Ok(mapper.Map<PlaylistDto>(addedPlaylist));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // [HttpPut("{id}")]
        // [ProducesResponseType(typeof(PlaylistDto), StatusCodes.Status200OK)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        // public IActionResult Put(int id, PlaylistBaseDto playlist)
        // {
        //     try
        //     {
        //         var updatedPlaylist = playlistService.Update(mapper.Map<PlaylistBL>(playlist,
        //                 o => o.AfterMap((src, dest) => dest.Id = id)));

        //         return updatedPlaylist != null ? Ok(mapper.Map<PlaylistDto>(updatedPlaylist)) : NotFound();
        //     }
        //     catch (Exception ex)
        //     {
        //         return Conflict(ex.Message);
        //     }
        // }

        [Authorize]
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(PlaylistDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Patch(int id, PlaylistBaseDto playlist)
        {
            try
            {
                var updatedPlaylist = await playlistService.UpdateAsync(playlistConverters.convertPatch(id, playlist));
                return updatedPlaylist != null ? Ok(mapper.Map<PlaylistDto>(updatedPlaylist)) : NotFound();
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(PlaylistDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedPlaylist = await playlistService.DeleteAsync(id);
            playlistService.DeleteSongPlaylistsByPlaylistIdAsync(id);

            return deletedPlaylist != null ? Ok(mapper.Map<PlaylistDto>(deletedPlaylist)) : NotFound();
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PlaylistDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var playlist = await playlistService.GetByIDAsync(id);
            return playlist != null ? Ok(mapper.Map<PlaylistDto>(playlist)) : NotFound();
        }

        [Authorize]
        [HttpGet("{playlistId}/songs")]
        [ProducesResponseType(typeof(IEnumerable<SongDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task <IActionResult> GetSongsByPlaylistId(
            int playlistId,
            [FromQuery] SongFilterDto filter,
            [FromQuery] SongSortState? sortState
        )
        {
            return Ok(mapper.Map<IEnumerable<SongDto>>(await songService.GetSongsByPlaylistIdAsync(playlistId, filter, sortState)));
        }

        [Authorize]
        [HttpGet("{playlistId}/songs/{songId}")]
        [ProducesResponseType(typeof(SongPlaylistDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSongPlaylist(int songId, int playlistId)
        {
            var songPlaylist = await playlistService.GetSongPlaylistAsync(songId, playlistId);
            return songPlaylist != null ? Ok(mapper.Map<SongPlaylistDto>(songPlaylist)) : NotFound();
        }

        [Authorize]
        [HttpPost("{playlistId}/songs")]
        [ProducesResponseType(typeof(PlaylistDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddSongToPlaylist(SongIdDto songIdDto, int playlistId)
        {
            try
            {
                return Ok(mapper.Map<PlaylistDto>(await playlistService.AddSongToMyPlaylistAsync(songIdDto.Id, playlistId)));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{playlistId}/songs/{songId}")]
        [ProducesResponseType(typeof(PlaylistDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteSongFromPlaylist(int songId, int playlistId)
        {
            try
            {
                return Ok(mapper.Map<PlaylistDto>(await playlistService.DeleteSongFromMyPlaylistAsync(songId, playlistId)));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}