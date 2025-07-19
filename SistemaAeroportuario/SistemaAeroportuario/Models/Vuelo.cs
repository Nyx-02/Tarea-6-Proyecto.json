using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Vuelo : IJsonStorage
{
    public string CodigoVuelo { get; set; }
    public string Origen { get; set; }
    public string Destino { get; set; }
    public DateTime HoraSalida { get; set; }
    public DateTime HoraLlegada { get; set; }
    public string Estado { get; set; }  // "Activo" o "Inactivo"
    public string AvionAsignado { get; set; }
    public int AsientosDisponibles { get; set; }  // Máximo 100

    public string GenerarCodigoVuelo() => $"AV-{new Random().Next(100, 999)}";

    public void AsignarAvion()
    {
        var aviones = new Avion().LoadFromJson().Cast<Avion>()
            .Where(a => a.EstadoMantenimiento == "Optimo").ToList();

        Console.WriteLine("\nAVIONES DISPONIBLES:");
        foreach (var avion in aviones)
            Console.WriteLine($"- {avion.Matricula} ({avion.Modelo})");

        Console.Write("\nSeleccione matrícula: ");
        AvionAsignado = Console.ReadLine();
        AsientosDisponibles = 100;
    }

    public void SaveToJson()
    {
        var vuelos = LoadFromJson().Cast<Vuelo>().ToList();
        vuelos.Add(this);
        File.WriteAllText("Vuelo.json", JsonConvert.SerializeObject(vuelos));
    }

    public List<object> LoadFromJson()
    {
        return File.Exists("Vuelo.json")
            ? JsonConvert.DeserializeObject<List<Vuelo>>(File.ReadAllText("Vuelo.json")).Cast<object>().ToList()
            : new List<object>();
    }

    public void InitializeSampleData()
    {
        if (!File.Exists("Vuelo.json"))
        {
            var sample = new List<Vuelo>
            {
                new Vuelo {
                    CodigoVuelo = GenerarCodigoVuelo(),
                    Origen = "TGU",
                    Destino = "SAP",
                    HoraSalida = DateTime.Now.AddHours(2),
                    HoraLlegada = DateTime.Now.AddHours(4),
                    Estado = "Activo",
                    AsientosDisponibles = 100
                }
            };
            File.WriteAllText("Vuelo.json", JsonConvert.SerializeObject(sample));
        }
    }
}