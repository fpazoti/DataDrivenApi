using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DataDriven.Controllers.V2
{
    //https://localhost:5001/categories
    [Route("v2/categories")]
    public class CategoryController : ControllerBase
    {
       
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<dynamic> Get()
        {
            return new 
            {
               message = "Exemplo de versionamento"
            };
        }
    }
}
