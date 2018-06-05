using System.Linq;
using Kronen.ViewModels;
using Kronen.web.Persistence;
using Kronen.web.Persistence.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Kronen.web.Controllers
{
    public class GameController : Controller
    {
        
        [Route("game/{id}/{jogadorId}")]
        public IActionResult Game(long id, string jogadorId){
            Player user = JsonConvert.DeserializeObject<Player>(HttpContext.Session.GetString("user"));
            
            VMGame vm = new VMGame(){
                JogadorId = jogadorId,
                GameId  = id,
                chatRoom = new VMChat(){
                    urlSocket = "/ws/room/"+ id.ToString() + "/" + user.id,
                    chatName = "Lobby",
                    userName = user.name
                }
            };
            var gameRoom = GameRepository.gameRooms.Where(x => x.gameId == id).FirstOrDefault();
            if(gameRoom != null){
                gameRoom.isPlaying = true;
            }
            return View("Index", vm);

        }
        private ViewResult ShowError(string message){
            return View("_ErroGenerico", new VMErroGenerico{ mensagem = message });
        }
    }
}