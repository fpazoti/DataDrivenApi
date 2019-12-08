using System.Threading.Tasks;
using DataDriven.Data;
using DataDriven.Models;
using Microsoft.AspNetCore.Mvc;


/*
TODO: Fazer depois via Seed do onmodelcreating do EF
*/

namespace DataDriven.Controllers
{
    [Route("v1")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context)
        {
            var employee = new User {Id = 1, Username = "robin", Password="robin", Role = "employee"};
            var manager = new User {Id = 2, Username = "batman", Password="batman", Role = "manager"};
            var category = new Category { Id= 1, Title="Informática"};
            var product = new Product { Id = 1, Category = category, Title = "Mouse", Price= 15 };

            context.Users.Add(employee);
            context.Users.Add(manager);
            context.Categories.Add(category);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            return Ok(new 
            {
               message = "Dados configurados"
            });
        }
    }
}