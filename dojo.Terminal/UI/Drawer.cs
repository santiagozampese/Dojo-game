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
        int cooldown1 = (Program.player1!.HabilityCooldown - (DateTime.Now - Program.player1!.LastUseHability)).Seconds + 1;
        int cooldown2 = (Program.player2!.HabilityCooldown - (DateTime.Now - Program.player2!.LastUseHability)).Seconds + 1;

        var cooldownText1 = $" Cooldown: { (DateTime.Now - Program.player1.LastUseHability > Program.player1.HabilityCooldown ? "Ready" : $"{cooldown1}")}";
        var cooldownText2 = $" Cooldown: { (DateTime.Now - Program.player2.LastUseHability > Program.player2.HabilityCooldown ? "Ready" : $"{cooldown2}")}";

        Console.WriteLine($"P1 ({Program.player1.Name}) H: {Program.player1!.Health} D: {Program.player1!.Damage} {cooldownText1}");
        Console.WriteLine($"P2 ({Program.player2.Name}) H: {Program.player2!.Health} D: {Program.player2!.Damage} {cooldownText2}");
    }

    private static string VerifyPosition(int x, int y)
    {
        if (Program.player1!.x == x && Program.player1.y == y)
        {
            return $"{Program.player1.Color}{Program.player1.Apparence}{ConsoleHelper.ResetColor}";
        }
        if (Program.player2!.x == x && Program.player2.y == y)
        {
            return $"{Program.player2.Color}{Program.player2.Apparence}{ConsoleHelper.ResetColor}";
        }
        return ".";
    }
}