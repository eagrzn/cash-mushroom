using FrogsTalks.Application;
using Microsoft.AspNetCore.Mvc;

namespace CashMushroom.Application.Facade.Controllers
{
    [Route("[controller]")]
    public class BillController : Controller
    {
        public BillController(ApplicationFacade app)
        {
            _app = app;
        }

        [HttpGet]
        public Bill Get() => _app.Get<Bill>(Tenant.Id);

        private readonly ApplicationFacade _app;
    }
}