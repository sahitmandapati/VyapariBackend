using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vyapari.Data.Entities;
using Vyapari.Infra;
using Vyapari.Service;

[ApiController] // Indicates that the controller is an API controller.
[Route("[controller]")] // Specifies the route template for the controller.
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] UserLoginDto userLoginDto)
    {
        try
        {
            var user = await _userService.AuthenticateAsync(userLoginDto.Email, userLoginDto.Password);
            return Ok(user);
        }
        // catch (UserNotFoundException)
        // {
        //     return BadRequest(new { message = "User not found." });
        // }
        // catch (InvalidPasswordException)
        // {
        //     return BadRequest(new { message = "Password is incorrect." });
        // }
        // catch (ArgumentException)
        // {
        //     return BadRequest(new { message = "Email and password are required." });
        // }
        catch (Exception ex)
        {
            // Log the exception here
            return BadRequest(new { message = ex.Message });
            //return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {
        // map dto to entity
        var user = _mapper.Map<User>(userRegisterDto);

        try
        {
            // save 
            await _userService.CreateAsync(user, userRegisterDto.Password);
            return Ok();
        }
        catch (Exception ex)
        {
            // return error message if there was an exception
            return BadRequest(new { message = ex.Message });
        }
    }
}