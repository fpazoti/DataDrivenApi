using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataDriven.Data;
using DataDriven.Models;
using DataDriven.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataDriven.Controllers
{
    [Route("users")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices]DataContext context)
        {
            return await context.Users.AsNoTracking().ToListAsync();
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post(
            [FromBody] User model,
            [FromServices] DataContext context)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try 
            {
                //Força sempre employee pois qq um cria usuário, depois em um put que só
                //o gerente por exemplo pode fazer, ele troca a role
                model.Role = "employee";

                context.Users.Add(model);
                await context.SaveChangesAsync();
                
                //gera um id por inserir no banco e já preenche na model que será retornada
                //Executa como se fosse ExecuteScalar do sql que já retorna o Id inserido
                
                //Esconde a senha
                model.Password = "";
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "não foi possível criar um usuário." });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromBody] User model,
            [FromServices] DataContext context)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(x => x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if(user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);

            //esconde a senha
            user.Password = "";
            return new 
            {
               user,
               token
            };
        }
    }
}