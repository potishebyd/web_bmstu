﻿using System;
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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;


namespace web_bmstu.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("/api/v1/users")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IPlaylistService playlistService;
        private readonly IMapper mapper;
        private readonly UserConverters userConverters;

        public UserController(IUserService userService, IPlaylistService playlistService,
                              IMapper mapper, UserConverters userConverters)
        {
            this.userService = userService;
            this.playlistService = playlistService;
            this.mapper = mapper;
            this.userConverters = userConverters;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll(
            [FromQuery] UserSortState? sortState
        )
        {
            return Ok(mapper.Map<IEnumerable<UserDto>>(userService.GetAllAsync(sortState)));
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(UserIdPasswordDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Add(UserPasswordDto userDto)
        {
            try
            {
                var addedUser = await userService.AddAsync(mapper.Map<UserBL>(userDto));
                return Ok(mapper.Map<UserIdPasswordDto>(addedUser));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        // [HttpPut("{id}")]
        // [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        // [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        // public IActionResult Put(int id, UserPasswordDto user)
        // {
        //     try
        //     {
        //         var updatedUser = userService.Update(mapper.Map<UserBL>(user,
        //                 o => o.AfterMap((src, dest) => dest.Id = id)));

        //         return updatedUser != null ? Ok(mapper.Map<UserDto>(updatedUser)) : NotFound();
        //     }
        //     catch (Exception ex)
        //     {
        //         return Conflict(ex.Message);
        //     }
        // }

        [Authorize]
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Patch(int id, UserPasswordDto user)
        {
            try
            {
                var updatedUser = await userService.UpdateAsync(userConverters.convertPatch(id, user));
                return updatedUser != null ? Ok(mapper.Map<UserDto>(updatedUser)) : NotFound();
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedUser = await userService.DeleteAsync(id);
            return deletedUser != null ? Ok(mapper.Map<UserDto>(deletedUser)) : NotFound();
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await userService.GetByIDAsync(id);
            return user != null ? Ok(mapper.Map<UserDto>(user)) : NotFound();
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(UserIdPasswordDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register(LoginDto loginDto)
        {
            var userDto = new UserPasswordDto
            {
                Login = loginDto.Login,
                Password = loginDto.Password,
                Permission = "user",
                Email = "user@mail.ru"
            };

            return await Add(userDto);
        }

        //[HttpPost("login")]
        //[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        //public IActionResult Login(LoginDto loginDto)
        //{
        //    var result = userService.Login(loginDto);
        //    return result != null ? Ok(mapper.Map<UserDto>(result)) : NotFound();
        //}

        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await userService.LoginAsync(loginDto);
            if (user == null)
            {
                return NotFound("Такого пользователя не существует");
            }

            var identity = GetIdentity(user);
            var now = DateTime.UtcNow;

            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var tokenDto = new TokenDto
            {
                AccessToken = encodedJwt,
                Username = identity.Name
            };

            return Json(tokenDto);
        }

        private ClaimsIdentity GetIdentity(UserBL user)
        {
            var claims = new List<Claim>
             {
                 new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                 new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Permission)
             };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}