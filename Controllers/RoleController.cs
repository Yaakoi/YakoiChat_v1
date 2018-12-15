using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using YakoiChat.Models;
using YakoiChat.Models.Data;

namespace YakoiChat.Controllers
{
    //[Authorize(Roles ="Admin")]
    [Route("[controller]")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly YakoiDbContext _ctx;

        public RoleController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            YakoiDbContext context
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _ctx = context;
        }
        [HttpGet("{id}", Name = "GetRoleB")]
        public async Task<IActionResult> GetRole(string id)
        {
            ApplicationRole roleById = await _roleManager.FindByIdAsync(id);

            if (roleById != null)
            {
                return Ok(roleById);
            }
            else
            {
                ApplicationRole roleByName = await _roleManager.FindByNameAsync(id);
                if (roleByName == null)
                {
                    return Ok(roleByName);

                }
            }
            return NotFound();

        }
       
        [HttpPost("create", Name = "CreateRole")]
        public async Task<object> Create([FromBody] ApplicationRole applicationRole)
        {
            
            if (await _roleManager.FindByNameAsync(applicationRole.Name) == null)
            {
                var role = new ApplicationRole { Name = applicationRole.Name, Description = applicationRole.Description, CreatedDate = DateTime.Now };
                var result =  await _roleManager.CreateAsync(new ApplicationRole(applicationRole.Name, applicationRole.Description, DateTime.Now));
                if (!result.Succeeded)
                {
                    return StatusCode(500, result);
                }
                Uri locationHeader = new Uri(Url.Link("GetRoleById", new { id = role.Id }));
                return Created(locationHeader, role);
            }
            return StatusCode(500, "Role Already exist");
        }

        [HttpPost("{idRole}/add/user/{idUser}", Name = "AddUserInRole")]
        public async Task<object> AddUserInRole (string idRole, string idUser)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(idUser);
            ApplicationRole role = await _roleManager.FindByIdAsync(idRole);
            if(user == null)
            {
                return BadRequest("User not found");
            }
            if(role == null)
            {
                return BadRequest("Role not found");
            }
            IdentityResult idResult = await _userManager.AddToRoleAsync(user,role.Name );
            return Created($"Role/" + role.Id + "/add/user/" + user.Id, idResult);
        }


    }
}
