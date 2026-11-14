using dojo.API;

namespace dojo.Core
{
    public class GameContext : IGameContext
    {
        public Player? Player1 {get; set;}
        public Player? Player2 {get; set;}
        public int MapWidth => Drawer.width;
        public int MapHeight => Drawer.height;
    }
}