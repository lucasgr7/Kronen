
using System;

namespace Kronen.ViewModels
{
    public class VMGameRoom
    {
        public long gameId {get;set;}
        public string name {get;set;}
        public string playerId {get;set;}
        public int numberPlayers {get;set;}
        public VMChat chatRoom {get;set;}
    }
}