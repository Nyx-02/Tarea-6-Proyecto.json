// Models/Vuelo.cs
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
    public string Estado { get; set; }
    public string AvionAsignado { get; set; }
    public int AsientosDisponibles { get; set; }

    public bool VueloEnHorario() => DateTime.Now < HoraSalida;
    public void CambiarEstado(string nuevoEstado) => Estado = nuevoEstado;
    public TimeSpan CalcularDuracion() => HoraLlegada - HoraSalida;

    public void SaveToJson()
    {
        var vuelos = LoadFromJson().Cast<Vuelo>().ToList();
        vuelos.Add(this);
        File.WriteAllText("Vuelo.json", JsonConvert.SerializeObject(vuelos.Take(100)));
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
                    CodigoVuelo = "AV-101",
                    Origen = "TGU",
                    Destino = "SAP",
                    HoraSalida = DateTime.Now.AddDays(1).AddHours(6),
                    HoraLlegada = DateTime.Now.AddDays(1).AddHours(7),
                    Estado = "Programado",
                    AvionAsignado = "B737-800",
                    AsientosDisponibles = 120
                },
                new Vuelo {
                    CodigoVuelo = "AV-202",
                    Origen = "SAP",
                    Destino = "RTB",
                    HoraSalida = DateTime.Now.AddDays(2).AddHours(10),
                    HoraLlegada = DateTime.Now.AddDays(2).AddHours(11),
                    Estado = "Programado",
                    AvionAsignado = "A320",
                    AsientosDisponibles = 150
                },
                new Vuelo {
                    CodigoVuelo = "AV-303",
                    Origen = "TGU",
                    Destino = "LCE",
                    HoraSalida = DateTime.Now.AddDays(3).AddHours(14),
                    HoraLlegada = DateTime.Now.AddDays(3).AddHours(15),
                    Estado = "Confirmado",
                    AvionAsignado = "E190",
                    AsientosDisponibles = 80
                }
            };
            File.WriteAllText("Vuelo.json", JsonConvert.SerializeObject(sample));
        }
    }
}