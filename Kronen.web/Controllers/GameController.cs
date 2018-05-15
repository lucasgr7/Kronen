using Microsoft.AspNetCore.Mvc;

namespace Kronen.web.Controllers
{
    public class GameController : Controller
    {
        
        [Route("game/{id}")]
        public IActionResult Game(){
            //todo: validate all players is ready
            return View("Index", null);

        }
    }
}