using dojo.API;

namespace dojo.Core
{
    public static class Program
    {
        public static Player? player1;
        public static Player? player2;
        public static event EventHandler<InputEventArgs>? input;
        public static List<Modpack> modpacks = new();
        public static void Main(string[] args)
        {
            Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs e) => Console.CursorVisible = true;
            
            ClassesManager.Init();
            var modClasses = DllLoader.LoadMods("mods");
            foreach (var c in modClasses)
            {
                ClassRegistry.AddClass(c);
            }

            while (true)
            {
                try
                {
                    Console.CursorVisible = true;
                    Console.Clear();
                    Console.WriteLine("""
                    1) Play
                    2) Class Help
                    """);

                    Console.Write("\nChoice: ");
                    int choice;

                    do
                    {
                        if (int.TryParse(Console.ReadLine(), out choice))
                        {
                            Math.Max(choice, 2);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    while (choice < 0 || choice > 2);

                    switch (choice)
                    {
                        case 1:
                            PreGame();
                            break;
                        case 2:
                            Help();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error in menu: {e.Message}");
                }
            }
        }
        private static void Help()
        {
            Console.Clear();
            string text = "";
            var color = ConsoleHelper.Blue;

            foreach (var c in ClassRegistry.classes)
            {     
                text += $"{color}{c.Name}) {c.Description}{ConsoleHelper.ResetColor}\n";
                if (color == ConsoleHelper.Blue)
                {
                    color = ConsoleHelper.Red;
                }
                else
                {
                    color = ConsoleHelper.Blue;
                }
            }
            int i = 1;
            foreach (var modpack in modpacks)
            {
                text += $"{ConsoleHelper.Yellow}📦 {i}) {modpack.Name}{ConsoleHelper.ResetColor}";
                i++;
            }
            Console.WriteLine(text);
            Console.WriteLine("\nTo see modpack help, press the corresponding number");
            Console.WriteLine($"Press enter to go back.");
            var line = Console.ReadLine();
            int choice;
            if (int.TryParse(line, out choice))
            {
                Console.Clear();
                var modpack = modpacks[choice - 1];
                text = $"{ConsoleHelper.Yellow}📦🔓 {modpack.Name}{ConsoleHelper.ResetColor}\n\n";
                foreach (var mod in modpack.Mods)
                {
                    text += $"{color}{mod.Name}) {mod.Description}{ConsoleHelper.ResetColor}\n";
                    if (color == ConsoleHelper.Blue)
                    {
                        color = ConsoleHelper.Red;
                    }
                    else
                    {
                        color = ConsoleHelper.Blue;
                    }
                }
                Console.WriteLine(text);
                Console.WriteLine($"Press enter to go back.");
                Console.ReadLine();
                Help();
            }
        }
        private static Player? Choose(List<ClassReference> classes, int playerNumber, bool showModPacks = true, Modpack? modpack = null)
        {
            string classesText = "";
            int i = 1;
            foreach (var c in classes)
            {
                classesText += $"{i}) {c.Name}\n";
                i++;
            }

            if (showModPacks)
            {      
                foreach (var mp in modpacks)
                {
                    classesText += $"{ConsoleHelper.Yellow}📦 {i}) {mp.Name}{ConsoleHelper.ResetColor}\n";
                    i++;
                }
            }
            int choice = 0;

            do
            {
                var color = playerNumber == 1 ? ConsoleHelper.Blue : ConsoleHelper.Red;
                string modPackText = "";
                if (modpack != null)
                {
                    modPackText = "📦🔓 " + modpack.Name + "\n";
                }
                Console.Clear();
                Console.WriteLine($"""
                {color}P{playerNumber} Class:
                {ConsoleHelper.Yellow}{modPackText}{color}{classesText}{ConsoleHelper.ResetColor}
                """);

                if (!showModPacks)
                {
                    Console.WriteLine("\nType 'x' to back.");
                }
                else
                {
                    Console.WriteLine();
                }
                Console.Write("\nChoice: ");
                var line = Console.ReadLine();
                if (line!.Trim().ToLower() == "x" && showModPacks == false)
                {
                    return null;
                }
                if (int.TryParse(line, out choice))
                {
                    Math.Max(choice, classes.Count + modpacks.Count);
                }
                else
                {
                    continue;
                }
            }
            while (choice < 1 || choice > classes.Count + modpacks.Count);

            if (choice > classes.Count)
            {
                var modPack = modpacks[choice - classes.Count - 1];
                var p = Choose(modPack.Mods, playerNumber, false, modPack);
                if (p != null)
                {
                    return p;
                }
                return Choose(classes, playerNumber);
            }
            return classes[choice - 1].Instantiate(playerNumber);
        }
        private static void PreGame()
        {
            try
            {        
                player1 = Choose(ClassRegistry.classes.ToList(), 1);
                player2 = Choose(ClassRegistry.classes.ToList(), 2);

                player1!.x = 5;
                player1.y = 5;

                player2!.x = 25;
                player2.y = 10;

                Console.CursorVisible = false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in pregame: {e.Message}");
                Environment.Exit(1);
            }
            StartGame();
        }
        private static void StartGame()
        {
            var cts = new CancellationTokenSource();
            Thread? drawThread = null;
            try
            {          
                var context = new GameContext();

                context.Player1 = player1;
                context.Player2 = player2;

                player1!.SetContext(context);
                player2!.SetContext(context);

                player1!.Config();
                player2!.Config();

                input = null;
                
                input += Player1Input;
                input += Player2Input;

                drawThread = new Thread(() => DrawThread(cts.Token));
                drawThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error initializing game: {e.Message}");
                Environment.Exit(1);
            }

            while (true) // Gameloop
            {
                try
                {
                    
                    Drawer.Draw();
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    input!.Invoke(null, new InputEventArgs() { key = keyInfo.Key });

                    if (player1!.Health <= 0)
                    {
                        cts.Cancel();
                        Console.Clear();
                        Console.WriteLine($"{ConsoleHelper.Red}Player 2 ({player2.Name}) wins!{ConsoleHelper.ResetColor}");
                        Console.WriteLine("Press enter to go back to menu...");
                        Console.ReadLine();
                        break;
                    }
                    if (player2!.Health <= 0)
                    {
                        cts.Cancel();
                        Console.Clear();
                        Console.WriteLine($"{ConsoleHelper.Blue}Player 1 ({player1.Name}) wins!{ConsoleHelper.ResetColor}");
                        Console.WriteLine("Press enter to go back to menu...");
                        Console.ReadLine();
                        break;
                    }
                    CorrectPositions();               
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error in gameloop: {e.Message}");
                    Environment.Exit(1);
                }
            }
        }

        private static void DrawThread(CancellationToken token)
        {
            try
            {      
                while (!token.IsCancellationRequested)
                {      
                    Drawer.Draw();
                    Thread.Sleep(1000);
                }
            }
            catch{}
        }

        private static void CorrectPositions()
        {
            if (player1 != null)
            {         
                if (player1.x > Drawer.width - 1)
                {
                    player1.x = Drawer.width -1;
                }
                else if (player1.x < 0)
                {
                    player1.x = 0;
                }

                if (player1.y > Drawer.height -1)
                {
                    player1.y = Drawer.height - 1;
                }
                else if (player1.y < 0)
                {
                    player1.y = 0;
                }
            }

            if (player2 != null)
            {           
                if (player2.x > Drawer.width - 1)
                {
                    player2.x = Drawer.width - 1;
                }
                else if (player2.x < 0)
                {
                    player2.x = 0;
                }

                if (player2.y > Drawer.height - 1)
                {
                    player2.y = Drawer.height - 1;
                }
                else if (player2.y < 0)
                {
                    player2.y = 0;
                }
            }
        }

        private static void Player1Input(object? sender, InputEventArgs e)
        {
            if (player1 == null) return;

            switch (e.key)
            {
                case ConsoleKey.W:
                    player1.Up();
                    break;
                case ConsoleKey.S:
                    player1.Down();
                    break;
                case ConsoleKey.A:
                    player1.Left();
                    break;
                case ConsoleKey.D:
                    player1.Right();
                    break;
                case ConsoleKey.Spacebar:
                    if (Functions.CanAttack(player1, player2!, 1, 1))
                    {             
                        player1.Attack();
                    }
                    break;
                case ConsoleKey.E:
                    player1.TryUseHability();
                    break;
            }
        }

        private static void Player2Input(object? sender, InputEventArgs e)
        {
            if (player2 == null) return;

            switch (e.key)
            {
                case ConsoleKey.I:
                    player2.Up();
                    break;
                case ConsoleKey.K:
                    player2.Down();
                    break;
                case ConsoleKey.J:
                    player2.Left();
                    break;
                case ConsoleKey.L:
                    player2.Right();
                    break;
                case ConsoleKey.P:
                    if (Functions.CanAttack(player2, player1!, 1, 1))
                    {     
                        player2.Attack();
                    }
                    break;
                case ConsoleKey.O:
                    player2.TryUseHability();
                    break;
            }
        }
        
    }

    public class InputEventArgs
    {
        public ConsoleKey key;
    }
}
