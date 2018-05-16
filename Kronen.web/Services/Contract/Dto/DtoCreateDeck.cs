using System.Collections.Generic;
using Kronen.lib.Dto;
using Kronen.web.Persistence.Domain;

namespace Kronen.web.Services.Contract.Dto
{
    public class DtoCreateDeckRequest {
        public long GameId {get;set;}
    }
    public class DtoCreateDeckResponse : DtoResponseBase{
        public List<Card> Cartas {get;set;}
    }
}