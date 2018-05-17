using System.Collections.Generic;
using System.ComponentModel;
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
                jogo.Tabuleiros = new List<Tabuleiro>();
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
                short contagem = 0;
                var gameRoom = GameRepository.gameRooms.Where(x=> x.gameId == dto.GameId).FirstOrDefault();
                gameRoom.Players.ForEach(x => {
                    List<Coluna> colunas = new List<Coluna>();
                    List<Linha> linhas = new List<Linha>();
                    contagem++;
                    
                    for(short i = 0; i < 3; i++){
                        var c = new Coluna(){
                            Linhas = new List<Linha>(),
                            Posicao = i
                        };
                        for(short f = 0; f < 3; f++){
                            var key = service.GetUniqueKey(new DtoGerarUniqueKeyGameRequest(){
                                GameId = dto.GameId
                            });
                            
                            c.Linhas.Add(new Linha(){
                                Posicao = f,
                                Number = key.GeneratedId,
                                Data = new List<Card>()
                            });
                        }
                        colunas.Add(c);
                    }
                    jogo.Tabuleiros.Add(new Tabuleiro(){
                        Jogador = new Player(){
                            id = x.id,
                            name = x.name,
                            order = contagem
                        },
                        Colunas = colunas
                    });
                });
                jogo.ActivePlayer = jogo.Tabuleiros.Where(x => x.Jogador.order == 1).FirstOrDefault().Jogador.id;
            }
            if(jogo.CartasVisiveis.Count < 5){
                while(jogo.CartasVisiveis.Count < 5 && jogo.Baralho.Count > 1){
                    jogo.CartasVisiveis.Add(jogo.Baralho[0]);
                    jogo.Baralho.RemoveAt(0);
                }
            }
            //todo: Decidir qual jogo inicia a partida
            response.DadosPartida = new Game(){
                CartasVisiveis = jogo.CartasVisiveis,
                GameId = jogo.GameId,
                IndexCartas = jogo.IndexCartas,
                Tabuleiros = jogo.Tabuleiros,
                ActivePlayer = jogo.ActivePlayer
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

        [Route("api/game/send")]
        [HttpPost]
        public IActionResult SendPlay([FromBody]DtoEnvioJogadaRequest dto)
        {
            DtoEnvioJogadaResponse response = new DtoEnvioJogadaResponse();
            if(dto == null){
                response.AddError(TipoErro.DtoNulo);
                return Send(response);
            }
            if(dto.gameId == 0)
                response.AddError(TipoErro.JogoInvalido);
            if(string.IsNullOrEmpty(dto.jogadorId))
                response.AddError(TipoErro.JogadorInvalido);
            if(response.ExistErrors())
                return Send(response);

            DtoSendPlayResponse serviceResponse = service.SendPlay(new DtoSendPlayRequest(){
                gameId = dto.gameId,
                playerId = dto.jogadorId,
                colunas = dto.colunas.Select(x => {
                    return new DtoSendPlayRequest.Coluna(){
                        v = x.v,
                        linhas = x.linhas.Select(y => {
                            return new DtoSendPlayRequest.Linha(){
                                linha = y.linha,
                                data = y.data.Select(z => {
                                    return new DtoSendPlayRequest.Carta(){
                                        especial = z.especial,
                                        id = z.id,
                                        valor = z.valor,
                                        valorNumerico = z.valorNumerico,
                                        naipe = new DtoSendPlayRequest.Naipe(){
                                            simbolo = z.naipe.simbolo,
                                            texto = z.naipe.texto
                                        }
                                    };
                                }).ToList()
                            };
                        }).ToList()
                    };
                }).ToList()
            });
            if(serviceResponse.ExistErrors()){
                response.AddError(serviceResponse.getErrors());
            }
            return Send(response);
        }
        public enum TipoErro{
            [Description("Dto nulo")]
            DtoNulo = 0,
            [Description("Erro Inesperado")]
            ErroInesperado = 1,
            [Description("Jogador inválido")]
            JogadorInvalido = 2,
            [Description("Game Id Invalido")]
            JogoInvalido = 3
        }
    }
}