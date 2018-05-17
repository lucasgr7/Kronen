using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Kronen.web.Persistence;
using Kronen.web.Persistence.Domain;
using Kronen.web.Services.Contract;
using Kronen.web.Services.Contract.Dto;

namespace Kronen.web.Services
{
    public class GameService : IGameService
    {
        public DtoCreateDeckResponse GerarBaralho(DtoCreateDeckRequest dto)
        {
            var response = new DtoCreateDeckResponse();
            response.Cartas = new List<Card>();
            var naipes = new List<Naipe>(){
                new Naipe(){
                    Simbolo = "♣",
                    Texto = "paus"
                },
                new Naipe(){
                    Simbolo = "♦",
                    Texto = "ouro"
                },
                new Naipe(){
                    Simbolo = "♥",
                    Texto = "coracao"
                },
                new Naipe(){
                    Simbolo = "♠",
                    Texto = "espadilha"
                },
            };
            List<string> Valores = new List<string>(){
                //"2","3","4","5","6","7","8","9","10","J",
                "Q","K","A"
            };
            naipes.ForEach(x => {
                Valores.ForEach(y => {
                    try{
                        var GeneratedIdResponse = this.GetUniqueKey(new DtoGerarUniqueKeyGameRequest(){
                            GameId = dto.GameId
                        });
                        if(GeneratedIdResponse.ExistErrors()){
                            response.AddError(GeneratedIdResponse.getErrors());
                        }else{
                            response.Cartas.Add(new Card(){
                                Naipe = x,
                                Valor = y,
                                Especial = false,
                                Id = GeneratedIdResponse.GeneratedId,
                                ValorNumerico = ConvertNaipeToNumber(y)
                            });
                        }
                    }
                    catch(Exception ex){
                        response.AddError(TipoErro.ErroInesperado, ex);
                    }
                });
            });
            if(response.ExistErrors()){
                return response;
            }else{
                Random rng = new Random();  
                //Shuffle the cards
                int n = response.Cartas.Count;
                while(n > 1){
                    n --;
                    int k = rng.Next(n + 1);
                    Card c = response.Cartas[k];
                    response.Cartas[k] = response.Cartas[n];
                    response.Cartas[n] = c;
                }
            }

            return response;
        }

        public DtoGerarUniqueKeyGameResponse GetUniqueKey(DtoGerarUniqueKeyGameRequest dto){
            DtoGerarUniqueKeyGameResponse response = new DtoGerarUniqueKeyGameResponse();
            if(dto.GameId == null || dto.GameId < 0){
                response.AddError(TipoErro.GameIdInvalido);
                return response;
            }
            try{
                var game = GameRepository.Jogos.Where(x=> x.GameId == dto.GameId).FirstOrDefault();
                if(game == null){
                    response.AddError(TipoErro.GameNaoEncontrado);
                    return response;
                }
                game.IndexCartas += 1;
                response.GeneratedId = game.IndexCartas;            
            }
            catch(Exception ex){
                response.AddError(TipoErro.ErroInesperado, ex);
            }
            return response;
        }

        public DtoSendPlayResponse SendPlay(DtoSendPlayRequest dto)
        {
            DtoSendPlayResponse response = new DtoSendPlayResponse();
            if(dto.colunas == null || dto.colunas.Count == 0){
                response.AddError(TipoErro.ColunasInvalida);
                return response;
            }
            try{
                var jogo = GameRepository.Jogos.Where(x => x.GameId == dto.gameId).FirstOrDefault();
                if(jogo == null){
                    response.AddError(TipoErro.GameNaoEncontrado);
                    return response;
                }
                Card cartaUtilizada = null;
                if(dto.playerId == jogo.ActivePlayer || jogo.ActivePlayer == null){
                    dto.colunas.ForEach(x => {
                        x.linhas.ForEach(y => {
                            y.data.ForEach(z => {
                                jogo.CartasVisiveis.ForEach(a => {
                                    if(z.id == a.Id){
                                        cartaUtilizada = a;
                                    }
                                });
                            });
                        });
                    });
                    if(cartaUtilizada == null){
                        response.AddError(TipoErro.CartaUtlizadaInvalida);
                        return response;
                    }else{
                        jogo.CartasVisiveis.Remove(cartaUtilizada);
                    }
                    var tabuleiro = jogo.Tabuleiros.Where(x => x.Jogador.id == dto.playerId).FirstOrDefault();
                    tabuleiro.Colunas = new List<Coluna>();
                    dto.colunas.ForEach(x => {
                        tabuleiro.Colunas.Add(new Coluna(){
                            Posicao = x.v,
                            Linhas = x.linhas.Select(y => {
                                return new Linha(){
                                    Posicao = y.linha,
                                    Data = y.data.Select(z => {
                                        return new Card(){
                                            Naipe = new Naipe(){
                                                Simbolo = z.naipe.simbolo,
                                                Texto = z.naipe.texto
                                            },
                                            Valor = z.valor,
                                            ValorNumerico = z.valorNumerico,
                                            Id = z.id,
                                            Especial = z.especial
                                        };
                                    }).ToList()
                                };
                            }).ToList()
                        });
                    });
                    var orderAtual = jogo.Tabuleiros.Where(x => x.Jogador.id == dto.playerId).FirstOrDefault().Jogador.order;
                    var proximoJogador = jogo.Tabuleiros.Where(x => x.Jogador.order == orderAtual + 1).FirstOrDefault();
                    if(proximoJogador == null)
                        jogo.ActivePlayer = jogo.Tabuleiros.Where(x => x.Jogador.order == 1).FirstOrDefault().Jogador.id;
                    else
                        jogo.ActivePlayer = proximoJogador.Jogador.id;
                }else{
                    response.AddError(TipoErro.JogadaInvalidaTurno);
                }
            }
            catch(Exception ex){
                response.AddError(TipoErro.ErroInesperado, ex);
            }
            return response;
        }

        protected short ConvertNaipeToNumber(string naipe){
            switch (naipe.ToUpper()){
                case "J":
                    return 11;
                case "Q":
                    return 12;
                case "K":
                    return 13;
                case "A":
                    return 14;
            }
            return short.Parse(naipe);
        }

        public enum TipoErro{
            [Description("Erro inesperado")]
            ErroInesperado = 0,
            [Description("Game Id Invalido")]
            GameIdInvalido = 1,
            [Description("Game não encontrado ou não iniciado")]
            GameNaoEncontrado = 2,
            [Description("Baralho não gerado corretamente para o jogo pegar as cartas disponiveis")]
            BaralhoNaoGerado= 3,
            [Description("ColunasInvalida")]
            ColunasInvalida= 4,
            [Description("Jogada invalida, não é o seu turno!")]
            JogadaInvalidaTurno = 5,
            [Description("Não foi possível identificar a carta utilizada na partida!")]
            CartaUtlizadaInvalida = 6
        }

    }
}