using Kronen.lib.Dto;

namespace Kronen.web.Api.Contract
{
    public class DtoSetReadyPlayerRoomRequest
    {
        public string PlayerId {get;set;}
        public long RoomId {get;set;}
        public bool Status {get;set;}
    }
    
    public class DtoSetReadyPlayerRoomResponse : DtoResponseBase
    {
        
    }
    
}