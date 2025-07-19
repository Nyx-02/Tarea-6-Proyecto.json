using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Tripulacion : Persona, IJsonStorage
{
    public string Cargo { get; set; }  // "Piloto", "Asistente", "Ingeniero"
    public string Licencia { get; set; }  // Auto-generada
    public bool Disponible { get; set; }

    public void SeleccionarCargo()
    {
        string[] cargosValidos = { "Piloto", "Asistente", "Ingeniero" };
        while (!cargosValidos.Contains(Cargo))
        {
            Console.WriteLine("Cargos válidos: " + string.Join(", ", cargosValidos));
            Console.Write("Seleccione cargo: ");
            Cargo = Console.ReadLine();
        }
        Licencia = $"LIC-{new Random().Next(1000, 9999)}";
    }

    public void SaveToJson()
    {
        var tripulacion = LoadFromJson().Cast<Tripulacion>().ToList();
        tripulacion.Add(this);
        File.WriteAllText("Tripulacion.json", JsonConvert.SerializeObject(tripulacion));
    }

    public List<object> LoadFromJson()
    {
        return File.Exists("Tripulacion.json")
            ? JsonConvert.DeserializeObject<List<Tripulacion>>(File.ReadAllText("Tripulacion.json")).Cast<object>().ToList()
            : new List<object>();
    }

    public void InitializeSampleData()
    {
        if (!File.Exists("Tripulacion.json"))
        {
            var sample = new List<Tripulacion>
            {
                new Tripulacion {
                    Nombre = "Ana Gomez",
                    Cargo = "Piloto",
                    Licencia = "LIC-1234",
                    Disponible = true
                }
            };
            File.WriteAllText("Tripulacion.json", JsonConvert.SerializeObject(sample));
        }
    }

    public override void Agregar()
    {
        throw new NotImplementedException();
    }
}