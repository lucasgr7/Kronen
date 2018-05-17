using System.Collections.Generic;
using Kronen.lib.Dto;

namespace Kronen.web.Api.Contract
{
    public class DtoGameRoomStatusRequest
    {
        public bool imReady {get;set;}
    }
    public class DtoGameRoomStatusResponse : DtoResponseBase
    {
        public List<Player> Jogadores {get;set;}
        public bool gameReady {get;set;}
        public class Player{
            public string name {get;set;}
            public bool isReady {get;set;}
        }

    }
}