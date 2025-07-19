public static class ConsoleUtils
{
    public static void MostrarError(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✗ ERROR: {mensaje}");
        Console.ResetColor();
        PresioneParaContinuar();
    }

    public static void MostrarExito(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✓ {mensaje}");
        Console.ResetColor();
        PresioneParaContinuar();
    }

    public static void PresioneParaContinuar()
    {
        Console.WriteLine("\nPresione cualquier tecla para continuar...");
        Console.ReadKey();
    }
}