using System.Timers;

namespace dojo.API
{
    public class Player
    {
        public DateTime LastUseHability = DateTime.MinValue;
        public TimeSpan HabilityCooldown = TimeSpan.FromSeconds(5);
        public DateTime LastAttack = DateTime.MinValue;
        public TimeSpan AttackCooldown = TimeSpan.FromSeconds(1);
        public string Name {get; set;} = "";
        public int PlayerNumber = 0;
        public Player? OtherPlayer {get; private set;}

        public int BaseHealth { get; set; } = 100;
        public int BaseDamage { get; set; } = 10;
        public int Health { get; set; }
        public int Damage { get; set; }
        public char Direction { get; set; } = 'u';
        public int x {get; set;}
        public int y {get; set;}
        public char Apparence {get; set;} = '@';
        public string Color {get; set;} = "";

        public EventHandler<HitEventArgs>? OnHit;
        protected IGameContext? Context {get; private set;}

        public void SetContext(IGameContext context)
        {
            Context = context;
        }

        public virtual void Hability()
        {}

        public virtual void Attack()
        {
            if (DateTime.Now - LastAttack >= AttackCooldown)
            {
                LastAttack = DateTime.Now;
                OtherPlayer!.OnHit!.Invoke(this, new HitEventArgs() { Damage = Damage, attacker = this});
            }
        }

        public bool TryUseHability()
        {
            if (DateTime.Now - LastUseHability >= HabilityCooldown)
            {
                LastUseHability = DateTime.Now;
                Hability();
                return true;
            }
            return false;
        }

        public virtual void Config()
        {
            try
            {
                Health = BaseHealth;
                Damage = BaseDamage;

                if (PlayerNumber == 2)
                {
                    OtherPlayer = Context.Player1;
                }
                else if (PlayerNumber == 1)
                {
                    OtherPlayer = Context.Player2;
                }

                Color = ConsoleHelper.colors[new Random().Next(0, ConsoleHelper.colors.Length)];
                while (Color == OtherPlayer!.Color)
                {
                    Color = ConsoleHelper.colors[new Random().Next(0, ConsoleHelper.colors.Length)];
                }

                OnHit += (sender, e) =>
                {
                    if (e.attacker == null) return;
                    Health -= e.Damage;
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in player {PlayerNumber} config: {e.Message}");
            }
        }

        public void Up()
        {
            if (y > 0)
            {
                y--;
                Direction = 'u';
            }
        }
        public void Down()
        {
            if (y + 1 < Context.MapHeight)
            {
                y++;
                Direction = 'd';
            }
        }
        public void Right()
        {
            if (x + 1 < Context.MapWidth)
            {
                x++;
                Direction = 'r';
            }
        }
        public void Left()
        {
            if (x > 0)
            {
                x--;
                Direction = 'l';
            }
        }
    }

    public class HitEventArgs : EventArgs
    {
        public int Damage { get; set; }
        public Player? attacker { get; set; }
    }
    
}
