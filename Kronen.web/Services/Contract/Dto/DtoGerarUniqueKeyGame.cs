using Kronen.lib.Dto;

namespace Kronen.web.Services.Contract.Dto
{
    public class DtoGerarUniqueKeyGameRequest
    {
        public long GameId {get;set;}
    }
    public class DtoGerarUniqueKeyGameResponse : DtoResponseBase
    {
        public long GeneratedId {get;set;}
    }
}