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
    [Route("/api/v1/recordingStudios")]
    public class RecordingStudioController : Controller
    {

        private IRecordingStudioService recordingStudioService;
        private IMapper mapper;
        private RecordingStudioConverters recordingStudioConverters;
        private readonly ILogger<RecordingStudioController> _logger;

        public RecordingStudioController(IRecordingStudioService recordingStudioService, IMapper mapper,
                              RecordingStudioConverters recordingStudioConverters, ILogger<RecordingStudioController> logger)
        {
            this.recordingStudioService = recordingStudioService;
            this.mapper = mapper;
            this.recordingStudioConverters = recordingStudioConverters;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(IEnumerable<RecordingStudio>), StatusCodes.Status200OK)]
        //public IActionResult GetAll(
        //    [FromQuery] RecordingStudioFilterDto filter,
        //    [FromQuery] RecordingStudioSortState? sortState
        //)
        //{
        //    _logger.LogInformation("RecordingStudios (Request: GET)");
        //    return Ok(mapper.Map<IEnumerable<RecordingStudioDto>>(recordingStudioService.GetAll(filter, sortState)));
        //}

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RecordingStudio>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] RecordingStudioFilterDto filter,
            [FromQuery] RecordingStudioSortState? sortState
)
        {
            _logger.LogInformation("RecordingStudios (Request: GET)");
            return Ok(mapper.Map<IEnumerable<RecordingStudioDto>>(await recordingStudioService.GetAllAsync(filter, sortState)));
        }

        //[Authorize]
        //[HttpPost]
        //[ProducesResponseType(typeof(RecordingStudioDto), StatusCodes.Status201Created)]
        //[ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        //public IActionResult Add(RecordingStudioBaseDto recordingStudioDto)
        //{
        //    try
        //    {
        //        var addedRecordingStudio = recordingStudioService.Add(mapper.Map<RecordingStudioBL>(recordingStudioDto));
        //        return Ok(mapper.Map<RecordingStudioDto>(addedRecordingStudio));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Conflict(ex.Message);
        //    }
        //}

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(RecordingStudioDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Add(RecordingStudioBaseDto recordingStudioDto)
        {
            try
            {
                var addedRecordingStudio = await recordingStudioService.AddAsync(mapper.Map<RecordingStudioBL>(recordingStudioDto));
                return Ok(mapper.Map<RecordingStudioDto>(addedRecordingStudio));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        //[Authorize]
        //[HttpPut("{id}")]
        //[ProducesResponseType(typeof(RecordingStudioDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        //public IActionResult Put(int id, RecordingStudioBaseDto recordingStudio)
        //{
        //    try
        //    {
        //        var updatedRecordingStudio = recordingStudioService.Update(mapper.Map<RecordingStudioBL>(recordingStudio,
        //                o => o.AfterMap((src, dest) => dest.Id = id)));

        //        return updatedRecordingStudio != null ? Ok(mapper.Map<RecordingStudioDto>(updatedRecordingStudio)) : NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Conflict(ex.Message);
        //    }
        //}
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RecordingStudioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Put(int id, RecordingStudioBaseDto recordingStudio)
        {
            try
            {
                var updatedRecordingStudio = await recordingStudioService.UpdateAsync(mapper.Map<RecordingStudioBL>(recordingStudio,
                        o => o.AfterMap((src, dest) => dest.Id = id)));

                return updatedRecordingStudio != null ? Ok(mapper.Map<RecordingStudioDto>(updatedRecordingStudio)) : NotFound();
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }


        //[Authorize]
        //[HttpDelete("{id}")]
        //[ProducesResponseType(typeof(RecordingStudioDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        //public IActionResult Delete(int id)
        //{
        //    var deletedRecordingStudio = recordingStudioService.Delete(id);
        //    return deletedRecordingStudio != null ? Ok(mapper.Map<RecordingStudioDto>(deletedRecordingStudio)) : NotFound();
        //}
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(RecordingStudioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedRecordingStudio = await recordingStudioService.DeleteAsync(id);
            return deletedRecordingStudio != null ? Ok(mapper.Map<RecordingStudioDto>(deletedRecordingStudio)) : NotFound();
        }

        //[HttpGet("{id}")]
        //[ProducesResponseType(typeof(RecordingStudioDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        //public IActionResult GetById(int id)
        //{
        //    var recordingStudio = recordingStudioService.GetByID(id);
        //    return recordingStudio != null ? Ok(mapper.Map<RecordingStudioDto>(recordingStudio)) : NotFound();
        //}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RecordingStudioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var recordingStudio = await recordingStudioService.GetByIDAsync(id);
            return recordingStudio != null ? Ok(mapper.Map<RecordingStudioDto>(recordingStudio)) : NotFound();
        }
    }
}