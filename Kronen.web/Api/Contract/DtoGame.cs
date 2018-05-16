using Kronen.lib.Dto;
using Kronen.web.Persistence.Domain;

namespace Kronen.web.Api.Contract
{
    public class DtoGameRequest
    {
        public string PlayerId {get;set;}
        public long GameId {get;set;}
    }
    public class DtoGameResponse : DtoResponseBase{
        public long GameId {get;set;}
        public Game DadosPartida {get;set;}
    }
}