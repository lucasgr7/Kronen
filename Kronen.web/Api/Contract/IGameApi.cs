using Microsoft.AspNetCore.Mvc;

namespace Kronen.web.Api.Contract
{
    public interface IGameApi
    {
        IActionResult GetGame(DtoGameRequest dto);
        IActionResult SendPlay(DtoEnvioJogadaRequest dto);
    }
}