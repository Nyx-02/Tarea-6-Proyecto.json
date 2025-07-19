using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public class Avion : IJsonStorage
{
    public string Matricula { get; set; }  // Formato "XX-123"
    public string Modelo { get; set; }
    public int CapacidadPasajeros { get; set; }  // Máximo 100
    public string EstadoMantenimiento { get; set; }  // "Optimo" o "Reparación"

    public void ValidarMatricula()
    {
        while (!Regex.IsMatch(Matricula, @"^[A-Z]{2}-\d{3}$"))
        {
            ConsoleUtils.MostrarError("Formato inválido. Use: XX-123 (ej: HR-101)");
            Console.Write("Matrícula: ");
            Matricula = Console.ReadLine();
        }
    }

    public void SaveToJson()
    {
        var aviones = LoadFromJson().Cast<Avion>().ToList();
        aviones.Add(this);
        File.WriteAllText("Avion.json", JsonConvert.SerializeObject(aviones));
    }

    public List<object> LoadFromJson()
    {
        return File.Exists("Avion.json")
            ? JsonConvert.DeserializeObject<List<Avion>>(File.ReadAllText("Avion.json")).Cast<object>().ToList()
            : new List<object>();
    }

    public void InitializeSampleData()
    {
        if (!File.Exists("Avion.json"))
        {
            var sample = new List<Avion>
            {
                new Avion {
                    Matricula = "HR-101",
                    Modelo = "Boeing 737",
                    CapacidadPasajeros = 100,
                    EstadoMantenimiento = "Optimo"
                }
            };
            File.WriteAllText("Avion.json", JsonConvert.SerializeObject(sample));
        }
    }
}