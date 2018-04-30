using System.Collections.Generic;
using Kronen.lib.Dto;

namespace Kronen.web.Services.Contract.Dto
{
    public class DtoCreateGameRequest
    {
        public Player Creator {get;set;}
        public int NumberPlayers {get;set;}
        public string NameRoom {get;set;}

        public class Player{
            public string id {get;set;}
            public string name {get;set;}
        }

    }
    public class DtoCreateGameResponse : DtoResponseBase
    {
        public GameRoom room {get;set;}
        public class GameRoom{
            public long gameId {get;set;}
            public string name {get;set;}
            public int NumberPlayers {get;set;}
        }
    }
}