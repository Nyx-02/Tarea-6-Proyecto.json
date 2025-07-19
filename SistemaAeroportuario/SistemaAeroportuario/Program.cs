using System;

public class Program
{
    private static bool isAdmin = true ;  // Cambiar a false para probar usuario

    private static void Main(string[] args)
    {
        DataInitializer.Initialize();

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("════════ SISTEMA AEROPORTUARIO ════════");
            Console.ResetColor();

            Console.WriteLine("\n1. Vuelos");
            Console.WriteLine("2. Vuelos Disponibles");
            Console.WriteLine("3. Aviones");
            Console.WriteLine("4. Tripulación");
            Console.WriteLine("5. Aeropuertos");
            Console.WriteLine("6. Boletos");
            Console.WriteLine("7. Pasajeros");
            Console.WriteLine("8. Salir");

            Console.Write("\nSeleccione una opción: ");
            if (!int.TryParse(Console.ReadLine(), out int opcion))
            {
                ConsoleUtils.MostrarError("¡Entrada inválida!");
                continue;
            }

            switch (opcion)
            {
                case 1: GestionarCRUD<Vuelo>(); break;
                case 2: MostrarVuelosDisponibles(); break;
                case 3: GestionarCRUD<Avion>(); break;
                case 4: GestionarCRUD<Tripulacion>(); break;
                case 5: GestionarCRUD<Aeropuerto>(); break;
                case 6: Boleto.ComprarBoleto(); break;
                case 7: GestionarCRUD<Pasajero>(); break;
                case 8: return;
                default: ConsoleUtils.MostrarError("Opción no válida."); break;
            }
        }
    }

    private static void GestionarCRUD<T>() where T : class, IJsonStorage, new()
    {
        new CRUDOperations<T>().MenuCRUD();
    }

    private static void MostrarVuelosDisponibles()
    {
        var vuelos = new Vuelo().LoadFromJson().Cast<Vuelo>()
            .Where(v => v.Estado == "Activo")
            .ToList();

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("════════ VUELOS DISPONIBLES ════════");
        Console.ResetColor();

        foreach (var vuelo in vuelos)
        {
            Console.WriteLine($"✈ {vuelo.CodigoVuelo} | {vuelo.Origen} → {vuelo.Destino} | Salida: {vuelo.HoraSalida:HH:mm}");
        }
        ConsoleUtils.PresioneParaContinuar();
    }
}