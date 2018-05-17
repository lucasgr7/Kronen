using System.Linq;
using Kronen.ViewModels;
using Kronen.web.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Kronen.web.Controllers
{
    public class GameController : Controller
    {
        
        [Route("game/{id}/{jogadorId}")]
        public IActionResult Game(long id, string jogadorId){
            VMGame vm = new VMGame(){
                JogadorId = jogadorId,
                GameId  = id
            };
            return View("Index", vm);

        }
        private ViewResult ShowError(string message){
            return View("_ErroGenerico", new VMErroGenerico{ mensagem = message });
        }
    }
}