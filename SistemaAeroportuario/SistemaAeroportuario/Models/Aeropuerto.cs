using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Aeropuerto : IJsonStorage
{
    public string CodigoIATA { get; set; }  // Auto-generado (ej: "TGU")
    public string Nombre { get; set; }
    public string Ciudad { get; set; }
    public string Pais { get; set; }

    public void GenerarCodigoIATA()
    {
        CodigoIATA = new string(Nombre.ToUpper().Take(3).ToArray());
    }

    public void SaveToJson()
    {
        var aeropuertos = LoadFromJson().Cast<Aeropuerto>().ToList();
        aeropuertos.Add(this);
        File.WriteAllText("Aeropuerto.json", JsonConvert.SerializeObject(aeropuertos));
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
                    Nombre = "Toncontín",
                    Ciudad = "Tegucigalpa",
                    Pais = "Honduras"
                }
            };
            sample.ForEach(a => a.GenerarCodigoIATA());
            File.WriteAllText("Aeropuerto.json", JsonConvert.SerializeObject(sample));
        }
    }
}