using FrogsTalks.Application;
using Microsoft.AspNetCore.Mvc;

namespace CashMushroom.Application.Facade.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        public ProductController(ApplicationFacade app)
        {
            _app = app;
        }

        [HttpPost]
        public void Post([FromBody]RecordCosts cmd)
        {
            _app.Do(cmd);
        }

        private readonly ApplicationFacade _app;
    }
}