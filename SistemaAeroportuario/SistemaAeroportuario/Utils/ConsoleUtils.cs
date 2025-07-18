public static class ConsoleUtils
{
    public static void MostrarError(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n✗ ERROR: {mensaje}");
        Console.ResetColor();
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }

    public static void MostrarExito(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✓ {mensaje}");
        Console.ResetColor();
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }
}