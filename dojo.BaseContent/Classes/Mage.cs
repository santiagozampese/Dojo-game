using dojo.API;
namespace dojo.BaseContent
{
    public class Mage : Player
    {
        public Mage(int number) : base(number)
        {
            health = 80;
            damage = 8;
            habilityCooldown = TimeSpan.FromSeconds(4);
        }
        public override void Hability()
        {
            if (Functions.CanAttack(this, otherPlayer!, 12, 12, false))
            {    
                otherPlayer!.OnHit!.Invoke(this, new HitEventArgs() { damage = damage, attacker = this });
            }
        }
    }
}
