using System.Collections.Generic;
using System.Linq;
using Kronen.Api.Dto;
using Kronen.lib.Dto;
using Kronen.web.Api.Contract;
using Kronen.web.Persistence;
using Kronen.web.Persistence.Domain;
using Kronen.web.Services.Contract;
using Kronen.web.Services.Contract.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Kronen.web.Api
{
    public class GameApi : Controller, IGameApi
    {
        public IGameService service {get;set;}

        public GameApi(IGameService _service){
            this.service = _service;
        }

        [Route("api/game")]
        [HttpPost]
        public IActionResult GetGame([FromBody]DtoGameRequest dto)
        {
            DtoGameResponse response = new DtoGameResponse();
            if(GameRepository.Jogos == null)
                GameRepository.Jogos = new List<Game>();
            var jogo = GameRepository.Jogos.Where(x => x.GameId == dto.GameId).FirstOrDefault();
            if(jogo == null){
                jogo = new Game();
                jogo.GameId = dto.GameId;
                GameRepository.Jogos.Add(jogo);    

                #region GerarBaralho
                var baralhoResponse = service.GerarBaralho(new DtoCreateDeckRequest(){
                    GameId = dto.GameId
                });
                if(baralhoResponse.ExistErrors()){
                    response.AddError(baralhoResponse.getErrors());
                    return Send(response);
                }
                jogo.Baralho = baralhoResponse.Cartas;
                #endregion

                if(jogo.CartasVisiveis == null || jogo.CartasVisiveis.Count == 0){
                    jogo.CartasVisiveis = new List<Card>();
                }
                while(jogo.CartasVisiveis.Count < 5){
                    jogo.CartasVisiveis.Add(jogo.Baralho[0]);
                    jogo.Baralho.RemoveAt(0);
                }

            }
            if(jogo.CartasVisiveis.Count < 5){
                while(jogo.CartasVisiveis.Count < 5 && jogo.Baralho.Count > 1){
                    jogo.CartasVisiveis.Add(jogo.Baralho[0]);
                    jogo.Baralho.RemoveAt(0);
                }
            }
            response.DadosPartida = new Game(){
                CartasVisiveis = jogo.CartasVisiveis,
                GameId = jogo.GameId,
                IndexCartas = jogo.IndexCartas,
                Tabuleiros = jogo.Tabuleiros
            };
            return Send(response);
        }
        protected IActionResult Send(DtoResponseBase dto){
            if(dto.ExistErrors()){
                return BadRequest(dto);
            }else{
                return Ok(dto);
            }
        }

    }
}