using System.Collections.Generic;

namespace Kronen.web.Persistence.Domain
{
    public class Tabuleiro
    {
        public Player Jogador {get;set;}
        public List<Coluna> Colunas {get;set;}
    }
}