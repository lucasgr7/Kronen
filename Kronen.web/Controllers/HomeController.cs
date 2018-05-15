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
using Kronen.web.Services;
using Kronen.lib.Dto;
using Kronen.web.Persistence;

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
            if(GameRepository.ActivePlayers== null)
                GameRepository.ActivePlayers = new List<Player>();

            var player = new Player{
                id = playerId,
                name = nome
            };
            
            GameRepository.ActivePlayers.Add(player);
            HttpContext.Session.SetString("user",JsonConvert.SerializeObject(player));
            return View("Lobby", vm);
        }
        public IActionResult Lobby(){
            Player user = JsonConvert.DeserializeObject<Player>(HttpContext.Session.GetString("user"));
            VMLobby vm = new VMLobby(){
                chatRoom = new VMChat(){
                    urlSocket = "/ws",
                    chatName = "Lobby",
                    userName = user.name
                }
            };
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
                return ShowError("Erro ao gerar a sala", responseService.getErrors());
            }
            return RedirectToAction("EnterRoom", new {id = responseService.room.gameId});
        }


        [Route("home/room/{id}")]
        public IActionResult EnterRoom(long id){
            if(id == 0){
                return Index();
            }
            Player user = JsonConvert.DeserializeObject<Player>(HttpContext.Session.GetString("user"));
            var room = gameRoomService.GetRoom(id);

            if(room != null){
                if(!room.isPlaying){
                    var playersInRoom = ChatService.RoomsSockets.Where(x => x.RoomId == id).ToList();
                    if(playersInRoom.Count < room.NumberPlayers){
                        VMGameRoom vm = new VMGameRoom(){
                            name = room.name,
                            numberPlayers = room.NumberPlayers,
                            gameId = room.gameId,
                            playerId = user.id,
                            chatRoom = new VMChat(){
                                urlSocket = "/ws/room/"+ id.ToString() + "/" + user.id,
                                chatName = "Lobby",
                                userName = user.name
                            }
                        };
                        return View("GameRoom", vm);
                    }else
                    {
                        return ShowError("Sala ja se encontrada lotada de jogadores!");
                    }
                }
                else
                {
                    return ShowError("Partida iniciada, naõ é possível assisti-la ainda!");
                }
            }else{
                return ShowError("Sala da partida não pode ser nulo!");
            }
        }
        private ViewResult ShowError(string message){
            return View("_ErroGenerico", new VMErroGenerico{ mensagem = message });
        }
        private IActionResult ShowError(string v, List<KronenError> list)
        {
            var messageErrors = v + " - " + string.Join("; ", list.Select(x => x.message));
            return View("_ErroGenerico", new VMErroGenerico{ mensagem = messageErrors });
        }
    }
}
