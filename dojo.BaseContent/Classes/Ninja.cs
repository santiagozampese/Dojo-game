using dojo.API;
namespace dojo.BaseContent;

public class Ninja : Player
{
   public Ninja()
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

