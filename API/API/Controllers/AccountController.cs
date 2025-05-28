using API.Data;
using API.Dtos;
using API.Entites;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers;


public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper) : BaseApiController
{
    [HttpPost("register")]  //account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

        var user = mapper.Map<AppUser>(registerDto);
        user.UserName = registerDto.Username.ToLower();

        var resfreshToken = tokenService.GenerateRefreshToken();
        var expiresTime = DateTime.UtcNow.AddDays(1);

        user.RefreshExpiriesTime = expiresTime;
        user.RefreshToken = resfreshToken;

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);
      
        return new UserDto
        {
            UserName = registerDto.Username,
            KnownAs = user.KnownAs,
            Gender = user.Gender,
            Token = await tokenService.CreateToken(user),
            RefreshToken = resfreshToken,
            RefreshExpiriesTime = expiresTime,
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await userManager.Users
            .Include(c=>c.Photos)
            .FirstOrDefaultAsync(c=>c.NormalizedUserName == loginDto.UserName.ToUpper());

        if (user == null || user.UserName == null) return Unauthorized("Invalid userName");

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result) return Unauthorized();
        var resfreshToken = tokenService.GenerateRefreshToken();
        var expiresTime = DateTime.UtcNow.AddDays(1);
        
        user.RefreshExpiriesTime = expiresTime;
        user.RefreshToken = resfreshToken;

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded) return StatusCode(500, "Failed to update refresh token");

        return new UserDto
        {
            UserName = loginDto.UserName,
            KnownAs = user.KnownAs,
            Gender = user.Gender,
            Token = await tokenService.CreateToken(user),
            RefreshToken = resfreshToken,
            RefreshExpiriesTime = expiresTime,
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
        };
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Refresh([FromBody] TokenApiDto tokenApiModel)
    {
        if (tokenApiModel is null)
            return BadRequest("Invalid client request");

        var principal = await tokenService.GetPrincipalFromExpiredTokenAsync(tokenApiModel.AccessToken);
        if (principal == null)
            return Unauthorized("Invalid access token");

        var userName =  principal.Identity?.Name;
        if (string.IsNullOrEmpty(userName))
            return Unauthorized();

        //var user = await userManager.FindByNameAsync(userName);
        //if (user == null)
        //    return Unauthorized();

        var userDetails =  await userManager.Users
            .Include(c => c.Photos)
            .FirstOrDefaultAsync(c => c.NormalizedUserName == userName.ToUpper());

        if (userDetails == null || userDetails.RefreshToken != tokenApiModel.RefreshToken || userDetails.RefreshExpiriesTime <= DateTime.UtcNow)
            return Unauthorized("Invalid refresh token");

        // Rotate refresh token
        var newRefreshToken = tokenService.GenerateRefreshToken();
        userDetails.RefreshToken = newRefreshToken;
        var expiresTime = DateTime.UtcNow.AddDays(1);
        userDetails.RefreshExpiriesTime = expiresTime;

        var updateResult = await userManager.UpdateAsync(userDetails);


        if (!updateResult.Succeeded) return StatusCode(500, "Failed to update refresh token");

        return new UserDto
        {
            UserName = userDetails.UserName!,
            KnownAs = userDetails.KnownAs!,
            Gender = userDetails.Gender!,
            Token = await tokenService.CreateToken(userDetails),
            RefreshToken = newRefreshToken,
            RefreshExpiriesTime = expiresTime,
            PhotoUrl = userDetails.Photos.FirstOrDefault(x => x.IsMain)?.Url
        };
    }


    private async Task<bool> UserExists(string userName)
    {
        return await userManager.Users.AnyAsync(c => c.NormalizedUserName == userName.ToUpper());
    }
}

