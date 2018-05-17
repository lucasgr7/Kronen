using Kronen.lib.Dto;

namespace Kronen.web.Api.Contract
{
    public class DtoSetReadyPlayerRoomRequest
    {
        public Player player {get;set;}
        public long roomId {get;set;}
        public bool status {get;set;}
        public class Player{
            public string id {get;set;}
            public string name {get;set;}
        }
    }
    
    public class DtoSetReadyPlayerRoomResponse : DtoResponseBase
    {
        
    }
    
}