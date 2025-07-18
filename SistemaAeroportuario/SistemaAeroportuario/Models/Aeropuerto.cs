// Models/Aeropuerto.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Aeropuerto : IJsonStorage
{
    public string CodigoIATA { get; set; }
    public string Nombre { get; set; }
    public string Ciudad { get; set; }
    public string Pais { get; set; }
    public int Terminales { get; set; }
    public int Pistas { get; set; }
    public bool Operativo { get; set; }

    public bool EstaOperativo() => Operativo;
    public string ObtenerUbicacionCompleta() => $"{Ciudad}, {Pais}";
    public void ActualizarEstado(bool estado) => Operativo = estado;

    public void SaveToJson()
    {
        var aeropuertos = LoadFromJson().Cast<Aeropuerto>().ToList();
        aeropuertos.Add(this);
        File.WriteAllText("Aeropuerto.json", JsonConvert.SerializeObject(aeropuertos.Take(100)));
    }

    public List<object> LoadFromJson()
    {
        return File.Exists("Aeropuerto.json")
            ? JsonConvert.DeserializeObject<List<Aeropuerto>>(File.ReadAllText("Aeropuerto.json")).Cast<object>().ToList()
            : new List<object>();
    }

    public void InitializeSampleData()
    {
        if (!File.Exists("Aeropuerto.json"))
        {
            var sample = new List<Aeropuerto>
            {
                new Aeropuerto {
                    CodigoIATA = "TGU",
                    Nombre = "Toncontín",
                    Ciudad = "Tegucigalpa",
                    Pais = "Honduras",
                    Terminales = 1,
                    Pistas = 1,
                    Operativo = true
                },
                new Aeropuerto {
                    CodigoIATA = "SAP",
                    Nombre = "Ramón Villeda Morales",
                    Ciudad = "San Pedro Sula",
                    Pais = "Honduras",
                    Terminales = 2,
                    Pistas = 2,
                    Operativo = true
                },
                new Aeropuerto {
                    CodigoIATA = "RTB",
                    Nombre = "Juan Manuel Gálvez",
                    Ciudad = "Roatán",
                    Pais = "Honduras",
                    Terminales = 1,
                    Pistas = 1,
                    Operativo = true
                }
            };
            File.WriteAllText("Aeropuerto.json", JsonConvert.SerializeObject(sample));
        }
    }
}