namespace MoonUtils
{
    public static class Colorib
    {
        public static void Run(ConsoleColor color, Action action)
        {
            var Original = Console.ForegroundColor;
            Console.ForegroundColor = color;
            action();
            Console.ForegroundColor = Original;
        }
    }
}
