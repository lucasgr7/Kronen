using System.Collections.Generic;

namespace Kronen.Api.Dto
{
    public class DtoConsultaGameRequest
    {
        
    }
    public class DtoConsultaGameResponse
    {
        public List<Game> gamesAvailable {get;set;}
        public class Game{
            public long gameId{get;set;}
            public string name {get;set;}
            public string players {get;set;}
        }
    }
}