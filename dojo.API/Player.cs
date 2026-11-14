using System.Timers;

namespace dojo.API
{
    public class Player
    {
        public DateTime lastUseHability = DateTime.MinValue;
        public TimeSpan habilityCooldown = TimeSpan.FromSeconds(5);
        public DateTime lastAttack = DateTime.MinValue;
        public TimeSpan attackCooldown = TimeSpan.FromSeconds(1);
        public string Name {get; set;} = "";
        public int playerNumber = 0;
        public Player? otherPlayer {get; private set;}
        public int health { get; set; } = 100;
        public int damage { get; set; } = 10;
        public int x {get; set;}
        public int y {get; set;}
        public char Apparence {get; set;} = '@';
        public string color {get; set;} = "";

        public EventHandler<HitEventArgs>? OnHit;
        protected IGameContext Context {get; private set;}
        public void SetContext(IGameContext context)
        {
            Context = context;
        }

        public virtual void Hability()
        {}

        public virtual void Attack()
        {
            if (DateTime.Now - lastAttack >= attackCooldown)
            {
                lastAttack = DateTime.Now;
                otherPlayer!.OnHit!.Invoke(this, new HitEventArgs() { damage = damage, attacker = this});
            }
        }

        public bool TryUseHability()
        {
            if (DateTime.Now - lastUseHability >= habilityCooldown)
            {
                lastUseHability = DateTime.Now;
                Hability();
                return true;
            }
            return false;
        }

        public virtual void Config()
        {
            if (playerNumber == 2)
            {
                otherPlayer = Context.Player1;
            }
            else if (playerNumber == 1)
            {
                otherPlayer = Context.Player2;
            }

            color = ConsoleHelper.colors[new Random().Next(0, ConsoleHelper.colors.Length)];
            while (color == otherPlayer!.color)
            {
                color = ConsoleHelper.colors[new Random().Next(0, ConsoleHelper.colors.Length)];
            }
        }
        public Player(int number)
        {
            playerNumber = number;

            OnHit += (sender, e) =>
            {
                if (e.attacker == null) return;
                health -= e.damage;
            };
        }

        public void Up()
        {
            if (y > 0)
            {
                y--;
            }
        }
        public void Down()
        {
            if (y + 1 < Context.MapHeight)
            {
                y++;
            }
        }
        public void Right()
        {
            if (x + 1 < Context.MapWidth)
            {
                x++;
            }
        }
        public void Left()
        {
            if (x > 0)
            {
                x--;
            }
        }
    }

    public class HitEventArgs : EventArgs
    {
        public int damage { get; set; }
        public Player? attacker { get; set; }
    }
    
}