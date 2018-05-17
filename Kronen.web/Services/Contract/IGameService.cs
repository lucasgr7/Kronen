using Kronen.web.Services.Contract.Dto;

namespace Kronen.web.Services.Contract
{
    public interface IGameService
    {
        DtoCreateDeckResponse GerarBaralho(DtoCreateDeckRequest dto);
        DtoGerarUniqueKeyGameResponse GetUniqueKey(DtoGerarUniqueKeyGameRequest dto);
        DtoSendPlayResponse SendPlay(DtoSendPlayRequest dto);
    }
}