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
    }
}