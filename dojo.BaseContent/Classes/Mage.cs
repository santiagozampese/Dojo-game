using dojo.API;
namespace dojo.BaseContent
{
    public class Mage : Player
    {
        public Mage()
        {
            BaseHealth = 80;
            BaseDamage = 8;
            HabilityCooldown = TimeSpan.FromSeconds(4);
        }
        public override void Hability()
        {
            if (Functions.CanAttack(this, OtherPlayer!, 12, 12, false))
            {    
                OtherPlayer!.OnHit!.Invoke(this, new HitEventArgs() { Damage = Damage, attacker = this });
            }
        }
    }
}
