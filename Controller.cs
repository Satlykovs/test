using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

public class Controller : ControllerBase
{
    private readonly UserManager<UserClass> _userManager;
    private readonly SignInManager<UserClass> _signInManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserContext users;
    private readonly TokenService _tokenService;

    public Controller(UserManager<UserClass> userManager, SignInManager<UserClass> signInManager,
     RoleManager<Role> roleManager, TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }

    [HttpGet("addrole")]
    public async Task<IActionResult> CreateRole(string roleName)
{
    var roleExists = await _roleManager.RoleExistsAsync(roleName);
    if (roleExists)
    {
        return BadRequest();

    }


    var role = new  Role{Name = roleName};
    var result = await _roleManager.CreateAsync(role);

    if (result.Succeeded)
    {
        return Ok(_roleManager.Roles.ToList());

    }
    else
    {   
        return BadRequest();

    }
}

    [HttpGet("adduser")]
    public async Task<IActionResult> CreateUser(string username, string email, string password)
{
    var user = new UserClass { UserName = username, Email = email };
    var result = await _userManager.CreateAsync(user, password);
    if (result.Succeeded)
    {
        _signInManager.SignInAsync(user, false).Wait();
        return Ok(_userManager.Users.ToList());
    }
    else
    {
        Console.WriteLine(result);
        return BadRequest();
    }
}

    [HttpGet("login")]
    public IActionResult Login(string name, string password)
    {
        var result = _signInManager.PasswordSignInAsync(name, password, false, false).Result;
        if (result.Succeeded)
        {
            Console.WriteLine($"Текущий юзер зареган {User.Identity.IsAuthenticated}");

            return Ok(_tokenService.CreateToken(name));
        }
        else
        {
            return BadRequest();
        }
    }

    [Authorize]
    [HttpGet("all")]
    public IActionResult All()
    {
        if (User.Identity.IsAuthenticated)
        {
        return Ok(_userManager.Users.ToList());
        }
        return Ok("Не авторизован");
    }   

    
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        _signInManager.SignOutAsync().Wait();
        Console.WriteLine($"Текущий юзер зареган: {User.Identity.IsAuthenticated}");
        return Ok();
        
    }
    [HttpGet("giverole")]
    public async Task<IActionResult> GiveRole(string userName, string roleName)
    {
        UserClass user = await _userManager.FindByNameAsync(userName);
        if (user != null)
        {
            if (_roleManager.FindByNameAsync(roleName) != null)
            {
                await _userManager.AddToRoleAsync(user, roleName);
                var newRole = await _userManager.GetRolesAsync(user);
                return Ok(newRole);
            }

    
        }
        return NotFound();
    }
}