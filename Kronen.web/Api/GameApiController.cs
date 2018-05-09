using System.Linq;
using Kronen.web.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Kronen.Api.Dto;
using Kronen.web.Services;

namespace Kronen.Api
{
    [Route("api/[controller]")]
    public class LobbyController : Controller
    {
        private readonly IGameRoomService service;
        public LobbyController(IGameRoomService _service){
            this.service = _service;
        }

        [HttpGet]
        public DtoConsultaGameResponse ConsultaGameResponse(DtoConsultaGameRequest dto){
            var response = new DtoConsultaGameResponse();
            var games = service.getAllAvailableRooms();
            if(games != null)
                response.gamesAvailable = games.Select(x => new DtoConsultaGameResponse.Game(){
                    gameId = x.gameId,
                    name = x.name,
                    players = ChatService.RoomsSockets.Where(y => y.RoomId == x.gameId).Count() + "/" +  x.NumberPlayers.ToString() 
                }).ToList();
            return response;
        }
    }
}