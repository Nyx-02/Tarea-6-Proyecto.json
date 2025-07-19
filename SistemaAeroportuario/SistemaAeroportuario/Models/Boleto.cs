using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Boleto : IJsonStorage
{
    public string CodigoBoleto { get; set; }
    public string CodigoVuelo { get; set; }
    public string Clase { get; set; }  // "Económica", "Ejecutiva", "Primera"
    public decimal Precio { get; set; }
    public DateTime FechaEmision { get; set; }
    public bool CheckInCompletado { get; set; }

    // Método para comprar boleto (completo)
    public static void ComprarBoleto()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("════════ COMPRAR BOLETO ════════");
        Console.ResetColor();

        var vuelosDisponibles = new Vuelo().LoadFromJson().Cast<Vuelo>()
            .Where(v => v.Estado == "Activo" && v.AsientosDisponibles > 0)
            .ToList();

        if (!vuelosDisponibles.Any())
        {
            ConsoleUtils.MostrarError("No hay vuelos disponibles para compra.");
            return;
        }

        Console.WriteLine("\nVUELOS DISPONIBLES:");
        foreach (var vuelo in vuelosDisponibles)
        {
            Console.WriteLine($"✈ {vuelo.CodigoVuelo} | {vuelo.Origen} → {vuelo.Destino} | Salida: {vuelo.HoraSalida:HH:mm}");
        }

        Vuelo vueloSeleccionado = null;
        while (vueloSeleccionado == null)
        {
            Console.Write("\nIngrese código de vuelo: ");
            string codigo = Console.ReadLine().Trim().ToUpper();
            vueloSeleccionado = vuelosDisponibles.FirstOrDefault(v => v.CodigoVuelo == codigo);

            if (vueloSeleccionado == null)
                ConsoleUtils.MostrarError("¡Vuelo no encontrado!");
        }

        Console.WriteLine("\nCLASES DISPONIBLES:");
        Console.WriteLine("1. Económica ($150)");
        Console.WriteLine("2. Ejecutiva ($300)");
        Console.WriteLine("3. Primera Clase ($500)");

        string clase = "";
        decimal precio = 0;
        while (true)
        {
            Console.Write("Seleccione clase (1-3): ");
            switch (Console.ReadLine())
            {
                case "1": clase = "Económica"; precio = 150; break;
                case "2": clase = "Ejecutiva"; precio = 300; break;
                case "3": clase = "Primera Clase"; precio = 500; break;
                default:
                    ConsoleUtils.MostrarError("Opción inválida. Use 1, 2 o 3.");
                    continue;
            }
            break;
        }

        var boleto = new Boleto
        {
            CodigoBoleto = $"B-{new Random().Next(100, 999)}",
            CodigoVuelo = vueloSeleccionado.CodigoVuelo,
            Clase = clase,
            Precio = precio,
            FechaEmision = DateTime.Now
        };
        boleto.SaveToJson();

        vueloSeleccionado.AsientosDisponibles--;
        vueloSeleccionado.SaveToJson();

        ConsoleUtils.MostrarExito($"¡Boleto {boleto.CodigoBoleto} comprado por ${precio}!");
    }

    // Métodos de IJsonStorage (completos)
    public void SaveToJson()
    {
        var boletos = LoadFromJson().Cast<Boleto>().ToList();
        boletos.Add(this);
        File.WriteAllText("Boleto.json", JsonConvert.SerializeObject(boletos));
    }

    public List<object> LoadFromJson()
    {
        return File.Exists("Boleto.json")
            ? JsonConvert.DeserializeObject<List<Boleto>>(File.ReadAllText("Boleto.json")).Cast<object>().ToList()
            : new List<object>();
    }

    public void InitializeSampleData()
    {
        if (!File.Exists("Boleto.json"))
        {
            var sample = new List<Boleto>
            {
                new Boleto {
                    CodigoBoleto = "B-001",
                    CodigoVuelo = "AV-101",
                    Clase = "Económica",
                    Precio = 150,
                    FechaEmision = DateTime.Now.AddDays(-1)
                }
            };
            File.WriteAllText("Boleto.json", JsonConvert.SerializeObject(sample));
        }
    }

    internal static object ListarBoletosDisponibles()
    {
        throw new NotImplementedException();
    }
}