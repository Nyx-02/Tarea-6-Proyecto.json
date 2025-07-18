// Models/Avion.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Avion : IJsonStorage
{
    public string Matricula { get; set; }
    public string Modelo { get; set; }
    public int CapacidadPasajeros { get; set; }
    public int AnoFabricacion { get; set; }
    public string EstadoMantenimiento { get; set; }
    public string Aerolinea { get; set; }
    public double HorasVueloTotales { get; set; }

    public bool NecesitaMantenimiento() => EstadoMantenimiento == "Pendiente";
    public void ActualizarHorasVuelo(double horas) => HorasVueloTotales += horas;
    public bool EsAvionComercial() => CapacidadPasajeros > 50;

    public void SaveToJson()
    {
        var aviones = LoadFromJson().Cast<Avion>().ToList();
        aviones.Add(this);
        File.WriteAllText("Avion.json", JsonConvert.SerializeObject(aviones.Take(100)));
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
                    Matricula = "HR-A350",
                    Modelo = "Boeing 737-800",
                    CapacidadPasajeros = 160,
                    AnoFabricacion = 2018,
                    EstadoMantenimiento = "Optimo",
                    Aerolinea = "Aerolínea Honduras",
                    HorasVueloTotales = 12000
                },
                new Avion {
                    Matricula = "HR-A320",
                    Modelo = "Airbus A320",
                    CapacidadPasajeros = 180,
                    AnoFabricacion = 2019,
                    EstadoMantenimiento = "Optimo",
                    Aerolinea = "Aerolínea Honduras",
                    HorasVueloTotales = 9000
                }
            };
            File.WriteAllText("Avion.json", JsonConvert.SerializeObject(sample));
        }
    }
}