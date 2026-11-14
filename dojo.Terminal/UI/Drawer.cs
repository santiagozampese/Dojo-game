using System.Text;
using dojo.Core;

public static class Drawer
{
    public static int width = 30;
    public static int height = 15;
    public static void Draw()
    {
        Console.Clear();
        StringBuilder drawable = new();
        for (int x = 0; x < width + 2; x++)
        {
            drawable.Append("-");
        }
        drawable.AppendLine();

        for (int y = 0; y < height; y++)
        {
            drawable.Append("|");
            for (int x = 0; x < width; x++)
            {
                drawable.Append(VerifyPosition(x, y));
            }
            drawable.Append("|");
            drawable.AppendLine();
        }

        for (int x = 0; x < width + 2; x++)
        {
            drawable.Append("-");
        }

        Console.SetCursorPosition(0, 0);
        Console.Write(drawable.ToString());

        Console.SetCursorPosition(0, height + 3);
        int cooldown1 = (Program.player1!.habilityCooldown - (DateTime.Now - Program.player1!.lastUseHability)).Seconds + 1;
        int cooldown2 = (Program.player2!.habilityCooldown - (DateTime.Now - Program.player2!.lastUseHability)).Seconds + 1;

        Console.WriteLine($"P1 ({Program.player1.Name}) Health: " + Program.player1!.health + $" Cooldown: { (DateTime.Now - Program.player1.lastUseHability > Program.player1.habilityCooldown ? "Ready" : $"{cooldown1}")}");
        Console.WriteLine($"P2 ({Program.player2.Name}) Health: " + Program.player2!.health + $" Cooldown: { (DateTime.Now - Program.player2.lastUseHability > Program.player2.habilityCooldown ? "Ready" : $"{cooldown2}")}");
    }

    private static string VerifyPosition(int x, int y)
    {
        if (Program.player1!.x == x && Program.player1.y == y)
        {
            return $"{Program.player1.color}{Program.player1.Apparence}{ConsoleHelper.ResetColor}";
        }
        if (Program.player2!.x == x && Program.player2.y == y)
        {
            return $"{Program.player2.color}{Program.player2.Apparence}{ConsoleHelper.ResetColor}";
        }
        return ".";
    }
}