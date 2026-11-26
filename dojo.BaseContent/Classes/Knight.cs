using dojo.API;
namespace dojo.BaseContent
{
    public class Knight : Player
    {   
        public Knight()
        {
            HabilityCooldown = TimeSpan.FromSeconds(6);
        }
        public override void Hability()
        {
            if (Functions.CanAttack(this, OtherPlayer!, 1, 1))
            {    
                OtherPlayer!.OnHit!.Invoke(this, new HitEventArgs() { Damage = Damage * 2, attacker = this });
            }
        }
    }
}