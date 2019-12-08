using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataDriven.Data;
using DataDriven.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//Endpoint => URL

namespace DataDriven.Controllers
{
    //https://localhost:5001/products
    [Route("products")]
    public class ProductController : ControllerBase
    {
       
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            //O include é um join.
            //Pode ter vários includes como vários joins
            var products = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById(
            int id,
            [FromServices] DataContext context)
        {
            var product = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(product);
        }

        [HttpGet] //products/categories/1
        [Route("categories/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetByCategory(
            int id,
            [FromServices] DataContext context)
        {
            var products = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.Category.Id == id)
                .ToListAsync();
                
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles="employee")]
        public async Task<ActionResult<Product>> Post(
            [FromBody] Product model,
            [FromServices] DataContext context)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try 
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                
                //gera um id por inserir no banco e já preenche na model que será retornada
                //Executa como se fosse ExecuteScalar do sql que já retorna o Id inserido
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "não foi possível criar um produto." });
            }
        }

        // [HttpPut]
        // [Route("{id:int}")]
        // public async Task<ActionResult<Category>> Put(
        //     int id, 
        //     [FromBody] Category model,
        //     [FromServices] DataContext context)
        // {
        //     if(model.Id != id)
        //         return NotFound(new { message = "Categortia não encontrada"});

        //     if(!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     try
        //     {
        //         context.Entry<Category>(model).State = EntityState.Modified;
        //         await context.SaveChangesAsync();
        //         return Ok(model);
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         return BadRequest(new { message = "Este registro já foi atualizado" });
        //     }
        //     catch(Exception) 
        //     {
        //         return BadRequest(new { message = "não foi possível criar uma categoria" });
        //     }
            
            
        // }

        // [HttpDelete]
        // [Route("{id:int}")]
        // public async Task<ActionResult<Category>> Delete(
        //     int id,
        //     [FromServices] DataContext context)
        // {
        //     var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        //     if(category == null)
        //         return NotFound(new { message = "Categoria não encontrada" });

        //     try
        //     {
        //         context.Categories.Remove(category);
        //         await context.SaveChangesAsync();
        //         return Ok(new { message = "Categoria removida com sucesso" });

        //     }
        //     catch (Exception)
        //     {
        //         return BadRequest(new { message = "não foi possível remover uma categoria" });
        //     }
        // }
    }
}