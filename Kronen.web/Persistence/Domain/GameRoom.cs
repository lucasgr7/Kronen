using System.Collections.Generic;

namespace Kronen.web.Persistence.Domain
{
    public class GameRoom
    {
        public long gameId {get;set;}
        public string name {get;set;}
        public int NumberPlayers {get;set;}
        public List<Player> players {get;set;}
        public bool isPlaying {get;set;}
        
    }
}