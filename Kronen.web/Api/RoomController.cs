using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Kronen.web.Api.Contract;
using Kronen.web.Persistence;
using Kronen.web.Persistence.Domain;
using Kronen.web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kronen.web.Api
{
    [Route("api/room")]
    public class RoomController : Controller
    {

        [HttpGet]
        public IActionResult RoomStatus([FromQuery] int? id){
            var response = new DtoGameRoomStatusResponse();
            if(id == null || id < 0){
                response.AddError(TipoErro.IdInvalido);
                return BadRequest(response);
            }
            var roomSockets = ChatService.RoomsSockets.Where(x => x.RoomId == id).ToList();
            var playersInRoom = roomSockets.Select(x => x.Player.Id).ToList();
            var jogadores = GameRepository.ActivePlayers.Where(x => playersInRoom.Contains(x.id)).ToList();
            response.Jogadores = jogadores.Select(x => new DtoGameRoomStatusResponse.Player(){
                isReady = roomSockets.Where(y => y.Player.Id == x.id).Select(y => y.Player.IsReady).First(),
                name = x.name
            }).ToList();
            var jogoAtivo = GameRepository.Jogos.Where(x => x.GameId == id).FirstOrDefault();
            if(jogoAtivo!= null)
                response.gameReady = true;
            return Ok(response);
        }
        [HttpPost]
        public IActionResult SetReady([FromBody] DtoSetReadyPlayerRoomRequest dto){
            var response = new DtoSetReadyPlayerRoomResponse();
            if(dto==null){
                response.AddError(TipoErro.DtoInvalido);
                return BadRequest(response);
            }
            if(string.IsNullOrEmpty(dto.player.id)){
                response.AddError(TipoErro.PlayerIdInvalido);
                return BadRequest(response);
            }
            var websocket = ChatService.RoomsSockets.Where(x => x.Player.Id == dto.player.id && x.RoomId == dto.roomId).FirstOrDefault();
            websocket.Player.IsReady = dto.status;
            var jogo = GameRepository.gameRooms.Where(x => x.gameId == dto.roomId).FirstOrDefault();
            if(jogo != null){
                if(jogo.Players == null)
                    jogo.Players = new List<Player>();
                jogo.Players.Add(new Player(){
                    id = dto.player.id,
                    name = dto.player.name
                });
            }else{
                response.AddError(TipoErro.JogoNaoExiste);
            }
            return Ok(response);
        }
    }
    public enum TipoErro{
        [Description("Parâmetro de entrada ID inválido")]
        IdInvalido = 1,
        [Description("Dto invalido")]
        DtoInvalido = 2,
        [Description("Player ID invalido")]
        PlayerIdInvalido = 3,
        [Description("Jogo não existe ainda")]
        JogoNaoExiste = 4
    }
}