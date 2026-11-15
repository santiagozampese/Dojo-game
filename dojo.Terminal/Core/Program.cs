using dojo.API;

namespace dojo.Core
{
    public static class Program
    {
        public static Player? player1;
        public static Player? player2;
        public static event EventHandler<InputEventArgs>? input;
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

                    Console.Write("Choice: ");
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
            Console.WriteLine(text);
            Console.WriteLine($"\nPress any key to back.");
            Console.ReadKey();
        }
        private static void PreGame()
        {
            try
            {        
                string classesText = "";
                int i = 1;
                foreach (var c in ClassRegistry.classes)
                {
                    classesText += $"{i}) {c.Name}\n";
                    i++;
                }
                
                int choice = 0;

                do
                {
                    Console.Clear();
                    Console.WriteLine($"""
                    {ConsoleHelper.Blue}P1 Class:
                    {classesText}{ConsoleHelper.ResetColor}
                    """);

                    Console.Write("\nChoice: ");
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        Math.Max(choice, ClassRegistry.classes.Count);
                    }
                    else
                    {
                        continue;
                    }
                }
                while (choice < 1 || choice - 1 > ClassRegistry.classes.Count);

                player1 = ClassRegistry.classes[choice - 1].Intantiate(1);
                player1.x = 5;
                player1.y = 5;

                do
                {
                    Console.Clear();
                    Console.WriteLine($"""
                    {ConsoleHelper.Red}P2 Class:
                    {classesText}{ConsoleHelper.ResetColor}
                    """);

                    Console.Write("\nChoice: ");
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        Math.Max(choice, ClassRegistry.classes.Count);
                    }
                    else
                    {
                        continue;
                    }
                }
                while (choice < 1 || choice - 1 > ClassRegistry.classes.Count);

                player2 = ClassRegistry.classes[choice - 1].Intantiate(2);
                player2.x = 25;
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

                    if (player1!.health <= 0)
                    {
                        cts.Cancel();
                        Console.Clear();
                        Console.WriteLine($"{ConsoleHelper.Red}Player 2 ({player2.Name}) wins!{ConsoleHelper.ResetColor}");
                        Console.WriteLine("Press enter to go back to menu...");
                        Console.ReadLine();
                        break;
                    }
                    if (player2!.health <= 0)
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
                case ConsoleKey.D0:
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
