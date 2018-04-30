using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using Kronen.web.Persistence;
using Kronen.web.Persistence.Domain;
using Kronen.web.Services.Contract;
using Kronen.web.Services.Contract.Dto;

namespace Kronen.web.Services
{
    public class GameRoomService : IGameRoomService
    {
        
        public DtoCreateGameResponse CreateGame(DtoCreateGameRequest dto)
        {
            var response = new DtoCreateGameResponse();
            if(dto.Creator == null || string.IsNullOrEmpty(dto.Creator.name)){
                response.AddError(Errors.CreatorInvalid);
            }
            if(dto.NumberPlayers == 0){
                response.AddError(Errors.InvalidNumberOfPlayers);
            }
            if(GameRepository.gameRooms == null){
                GameRepository.gameRooms = new List<GameRoom>();
            }
            response.room = new DtoCreateGameResponse.GameRoom();
            var gameId = GameRepository.gameRooms.Count + 1;
            
            GameRepository.gameRooms.Add(new GameRoom(){
                gameId = gameId,
                name = dto.NameRoom,
                NumberPlayers = dto.NumberPlayers,
                players = new List<Player>(){
                    new Player(){
                        id = dto.Creator.id,
                        name = dto.Creator.name
                    }
                }
            });

            response.room.gameId = gameId;
            response.room.name = dto.NameRoom;
            response.room.NumberPlayers = dto.NumberPlayers;
            //todo: Regra não ter um jogador como criador de duas salas
            return response;
        }
        public List<GameRoom> getAllAvailableRooms(){
            return GameRepository.gameRooms.Where(x => !x.isPlaying).ToList();
        }
        public enum Errors{
            [Description("Creator of the game not informed")]
            CreatorInvalid = 1,
            [Description("Invalid number of players for the room the minimal is 2")]
            InvalidNumberOfPlayers = 2,
        }
    }
}