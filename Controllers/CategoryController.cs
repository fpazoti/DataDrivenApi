using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataDriven.Data;
using DataDriven.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//Endpoint => URL

namespace DataDriven.Controllers
{
    //https://localhost:5001/categories
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        //https://localhost:5001/categories
        // [Route("")]
        // public string MeuMetodo() 
        // {
        //     return "Ola";
        // }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        //caso tiver cache na aplicaçào inteira, porém somente aqui não gostaríamos de ter cache
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            var categories = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> GetById(
            int id,
            [FromServices] DataContext context)
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles="employee")]
        public async Task<ActionResult<Category>> Post(
            [FromBody] Category model,
            [FromServices] DataContext context)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try 
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                
                //gera um id por inserir no banco e já preenche na model que será retornada
                //Executa como se fosse ExecuteScalar do sql que já retorna o Id inserido
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "não foi possível criar uma categoria" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles="employee")]
        public async Task<ActionResult<Category>> Put(
            int id, 
            [FromBody] Category model,
            [FromServices] DataContext context)
        {
            if(model.Id != id)
                return NotFound(new { message = "Categortia não encontrada"});

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro já foi atualizado" });
            }
            catch(Exception) 
            {
                return BadRequest(new { message = "não foi possível criar uma categoria" });
            }
            
            
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles="manager")]
        public async Task<ActionResult<Category>> Delete(
            int id,
            [FromServices] DataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(category == null)
                return NotFound(new { message = "Categoria não encontrada" });

            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { message = "Categoria removida com sucesso" });

            }
            catch (Exception)
            {
                return BadRequest(new { message = "não foi possível remover uma categoria" });
            }
        }
    }
}