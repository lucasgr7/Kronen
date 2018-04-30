using Kronen.web.Services.Contract.Dto;

namespace Kronen.web.Services.Contract
{
    public interface IGameRoomService
    {
         DtoCreateGameResponse CreateGame(DtoCreateGameRequest dto);
    }
}