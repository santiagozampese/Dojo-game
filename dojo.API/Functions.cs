namespace dojo.API
{
    public static class Functions
    {
        public static bool CanAttack(Player attacker, Player attacked, int maxDx, int maxDy, bool lineMode = true)
        {
            int dx = Math.Abs(attacked.x - attacker.x);
            int dy = Math.Abs(attacked.y - attacker.y);

            if (lineMode)
            { 
                if ((dx <= maxDx && dy == 0) || 
                    (dx == 0 && dy <= maxDy) ||
                    (dx == 0 && dy == 0))
                {        
                    return true;
                }
            }
            else
            {
                if (dx <= maxDx && dy <= maxDy || (dx == 0 && dy == 0))
                {
                    return true;
                }
            }
            return false;
        }
        public static void Weaken(Player player, int value, int min = 2)
        {
            if (player.Damage - value <= min)
            {
                player.Damage = min;
                return;
            }

            player.Damage -= value;
        }
        public static void Strengthen(Player player, int value, int max = -1)
        {   
            if (max < 0)
            {
                max = player.BaseDamage * 2;
            }
            if (player.Damage + value > max)
            {
                player.Damage = max;
                return;
            }

            player.Damage += value;
        }
    }
}
