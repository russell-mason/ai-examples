namespace AIExamples.Shared.Extensions;

public static class ConsoleExtensions
{
    extension(Console)
    {
        public static void WriteInColor(string value, ConsoleColor color) =>
            UseColor(color, () => Console.Write(value));

        public static void WriteLineInColor(string value, ConsoleColor color) =>
            UseColor(color, () => Console.WriteLine(value));

        public static void WriteSeparator() =>
            UseColor(ConsoleColor.Blue,
                     () => Console.WriteLine($"{Environment.NewLine}----------{Environment.NewLine}"));

        public static void WriteExampleSeparator<T>() where T : IExample =>
            UseColor(ConsoleColor.Blue,
                     () => Console.WriteLine($"{Environment.NewLine}---------- {typeof(T).FullName} ----------{Environment.NewLine}"));

        public static void WriteTitle(string value) =>
            UseColor(ConsoleColor.Yellow, () => Console.WriteLine($"{Environment.NewLine}{value}"));

        public static void WriteError(string message)
        {
            UseColor(ConsoleColor.Red, () =>
            {
                Console.WriteLine();
                Console.WriteLine(message);
                Console.WriteLine();
            });
        }

        public static void WriteError(Exception exception)
        {
            UseColor(ConsoleColor.Red, () =>
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("******************************************************************************************");
                Console.WriteLine("******************************************************************************************");
                Console.WriteTitle("This example failed to complete ...");
                Console.WriteLine();
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
                Console.WriteLine();
                Console.WriteLine("******************************************************************************************");
                Console.WriteLine("******************************************************************************************");
            });
        }

        private static void UseColor(ConsoleColor color, Action action)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            try
            {
                action();
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }
    }
}
