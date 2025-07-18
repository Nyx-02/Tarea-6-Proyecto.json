public class Program
{
    private static void Main(string[] args)
    {
        DataInitializer.Initialize();  // Cargar datos predeterminados

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("════════════════════════════════════════════════");
            Console.WriteLine(" SISTEMA DE GESTIÓN AEROPORTUARIA - HONDURAS ");
            Console.WriteLine("════════════════════════════════════════════════");
            Console.ResetColor();

            Console.WriteLine("1. Pasajeros");
            Console.WriteLine("2. Tripulación");
            Console.WriteLine("3. Vuelos");
            Console.WriteLine("4. Aviones");
            Console.WriteLine("5. Aeropuertos");
            Console.WriteLine("6. Boletos");
            Console.WriteLine("7. Vuelos Disponibles");
            Console.WriteLine("8. Salir");
            Console.Write("\nSeleccione una opción: ");

            if (!int.TryParse(Console.ReadLine(), out int opcion))
            {
                ConsoleUtils.MostrarError("Entrada inválida");
                continue;
            }

            if (opcion == 8) break;

            switch (opcion)
            {
                case 1: GestionarCRUD<Pasajero>(); break;
                case 2: GestionarCRUD<Tripulacion>(); break;
                case 3: GestionarCRUD<Vuelo>(); break;
                case 4: GestionarCRUD<Avion>(); break;
                case 5: GestionarCRUD<Aeropuerto>(); break;
                case 6: GestionarCRUD<Boleto>(); break;
                case 7: MostrarVuelosDisponibles(); break;
                default: ConsoleUtils.MostrarError("Opción no válida"); break;
            }
        }
    }

    private static void MostrarVuelosDisponibles()
    {
        try
        {
            var vuelos = new Vuelo().LoadFromJson().Cast<Vuelo>().ToList();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("════════════════════════════════════════════════");
            Console.WriteLine("        VUELOS DISPONIBLES - HONDURAS         ");
            Console.WriteLine("════════════════════════════════════════════════");
            Console.ResetColor();
            Console.WriteLine();

            if (!vuelos.Any())
            {
                Console.WriteLine("No hay vuelos disponibles en este momento.");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            foreach (var vuelo in vuelos.Where(v => v.Estado == "Programado" || v.Estado == "Confirmado"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✈ Vuelo: {vuelo.CodigoVuelo} [{vuelo.Estado}]");
                Console.ResetColor();

                var origen = new Aeropuerto().LoadFromJson().Cast<Aeropuerto>()
                    .FirstOrDefault(a => a.CodigoIATA == vuelo.Origen);
                var destino = new Aeropuerto().LoadFromJson().Cast<Aeropuerto>()
                    .FirstOrDefault(a => a.CodigoIATA == vuelo.Destino);

                Console.WriteLine($"  Origen:    {origen?.Nombre ?? vuelo.Origen} ({vuelo.Origen})");
                Console.WriteLine($"  Destino:   {destino?.Nombre ?? vuelo.Destino} ({vuelo.Destino})");
                Console.WriteLine($"  Salida:    {vuelo.HoraSalida:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"  Llegada:   {vuelo.HoraLlegada:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"  Duración:  {vuelo.CalcularDuracion().Hours}h {vuelo.CalcularDuracion().Minutes}m");
                Console.WriteLine($"  Asientos disponibles: {vuelo.AsientosDisponibles}");
                Console.WriteLine("────────────────────────────────────────────────");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            ConsoleUtils.MostrarError($"Error: {ex.Message}");
        }
    }

    private static void GestionarCRUD<T>() where T : class, IJsonStorage, new()
    {
        var crud = new CRUDOperations<T>();
        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"════════ GESTIÓN DE {typeof(T).Name.ToUpper()}S ════════");
            Console.ResetColor();
            Console.WriteLine("1. Agregar");
            Console.WriteLine("2. Buscar");
            Console.WriteLine("3. Actualizar");
            Console.WriteLine("4. Eliminar");
            Console.WriteLine("5. Listar Todos");
            Console.WriteLine("6. Regresar");
            Console.Write("\nSeleccione: ");

            if (!int.TryParse(Console.ReadLine(), out int opcion))
            {
                ConsoleUtils.MostrarError("Entrada inválida");
                continue;
            }

            if (opcion == 6) break;

            switch (opcion)
            {
                case 1: crud.Agregar(); break;
                case 2: crud.Buscar(); break;
                case 3: crud.Actualizar(); break;
                case 4: crud.Eliminar(); break;
                case 5: crud.Listar(); break;
                default: ConsoleUtils.MostrarError("Opción inválida"); break;
            }
        }
    }
}