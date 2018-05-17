using System.Collections.Generic;
using Kronen.lib.Dto;

namespace Kronen.web.Api.Contract
{
    public class DtoEnvioJogadaRequest
    {
        public List<Coluna> colunas {get;set;}
        public string jogadorId {get;set;}
        public long gameId {get;set;}

        public class Coluna {
            public short v {get;set;}
            public List<Linha> linhas {get;set;}
        }
        public class Linha {
            public short linha {get;set;}
            public List<Carta> data {get;set;}
        }
        public class Carta{
            public Naipe naipe {get;set;}
            public string valor {get;set;}
            public short valorNumerico {get;set;}
            public bool especial {get;set;}
            public long id {get;set;}
            
        }
        public class Naipe {
            public string simbolo {get;set;}
            public string texto {get;set;}
        }
    }
    public class DtoEnvioJogadaResponse : DtoResponseBase{

    }
}