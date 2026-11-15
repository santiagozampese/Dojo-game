using dojo.API;
namespace dojo.BaseContent
{
    public class Knight : Player
    {   
        public Knight()
        {
            habilityCooldown = TimeSpan.FromSeconds(6);
        }
        public override void Hability()
        {
            if (Functions.CanAttack(this, otherPlayer!, 1, 1))
            {    
                otherPlayer!.OnHit!.Invoke(this, new HitEventArgs() { damage = damage * 2, attacker = this });
            }
        }
    }
}