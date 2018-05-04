using System.Collections.Generic;
using Kronen.web.Persistence.Domain;
using Kronen.web.Services.Contract.Dto;

namespace Kronen.web.Services.Contract
{
    public interface IGameRoomService
    {
         DtoCreateGameResponse CreateGame(DtoCreateGameRequest dto);
         List<GameRoom> getAllAvailableRooms();
         GameRoom GetRoom(long id);
    }
}