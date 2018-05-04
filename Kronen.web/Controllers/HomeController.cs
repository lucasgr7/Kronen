using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kronen.Models;
using Kronen.Controllers.Dto;
using Kronen.ViewModels;
using Microsoft.AspNetCore.Http;
using Kronen.web.Persistence.Domain;
using Newtonsoft.Json;
using Kronen.web.Services.Contract;
using Kronen.web.Services.Contract.Dto;

namespace Kronen.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IGameRoomService gameRoomService;

        public HomeController(IGameRoomService _gameRoomService)
        {
            gameRoomService = _gameRoomService;
        }

        public IActionResult Login([FromForm] string nome){
            if(nome == null || string.IsNullOrEmpty(nome)){
                return View("LoginInvalido");
            }
            VMLobby vm = new VMLobby(){
                chatRoom = new VMChat(){
                    urlSocket = "/ws",
                    chatName = "Lobby",
                    userName = nome
                }
            };
            var playerId = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("user",JsonConvert.SerializeObject(new Player{
                id = playerId,
                name = nome
            }));
            return View("Lobby", vm);
        }

        public IActionResult CreateRoom([FromForm] DtoCriarSalaRequest dto)
        {
            Player user = JsonConvert.DeserializeObject<Player>(HttpContext.Session.GetString("user"));
            
            var dtoRequest = new DtoCreateGameRequest(){
                Creator = new DtoCreateGameRequest.Player{
                    name = user.name,
                    id = user.id
                },
                NumberPlayers = dto.numberPlayers,
                NameRoom = dto.roomName
            };
            var responseService = gameRoomService.CreateGame(dtoRequest);
            if(responseService.ExistErrors()){
                return View("Error");
            }
            return RedirectToAction("EnterRoom", new {id = responseService.room.gameId});
        }
        [Route("home/room/{id}")]
        public IActionResult EnterRoom(long id){
            if(id == 0){
                return Index();
            }
            var room = gameRoomService.GetRoom(id);
            if(room != null){
                if(!room.isPlaying){
                    VMGameRoom vm = new VMGameRoom(){
                        name = room.name,
                        NumberPlayers = room.NumberPlayers,
                        gameId = room.gameId
                    };
                    return View("GameRoom", vm);
                }else{
                    return View("ErroGenerico", null);
                }
            }else{
                return View("ErroGenerico", null);
            }
        }


    }
}
