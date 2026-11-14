using dojo.API;
namespace modNinja;

public class Ninja : Player
{
   public Ninja(int number) : base(number)
   {
      habilityCooldown = TimeSpan.FromSeconds(8);
   }
   public override void Hability()
   {
      var r = new Random();
      x = r.Next(0, Context.MapWidth);
      y = r.Next(0, Context.MapHeight);
   }
}

