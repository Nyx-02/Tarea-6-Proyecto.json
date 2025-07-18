// Models/Boleto.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Boleto : IJsonStorage
{
    public string CodigoBoleto { get; set; }
    public string PasajeroIdentificacion { get; set; }
    public string CodigoVuelo { get; set; }
    public string Clase { get; set; }
    public decimal Precio { get; set; }
    public DateTime FechaEmision { get; set; }
    public bool CheckInCompletado { get; set; }

    public bool EsBoletoValido() => FechaEmision > DateTime.Now.AddMonths(-6);
    public void RealizarCheckIn() => CheckInCompletado = true;
    public decimal CalcularImpuestos() => Precio * 0.15m;

    public void SaveToJson()
    {
        var boletos = LoadFromJson().Cast<Boleto>().ToList();
        boletos.Add(this);
        File.WriteAllText("Boleto.json", JsonConvert.SerializeObject(boletos.Take(100)));
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
                    PasajeroIdentificacion = "P12345678",
                    CodigoVuelo = "AV-101",
                    Clase = "Primera",
                    Precio = 350.00m,
                    FechaEmision = DateTime.Now.AddDays(-2)
                }
            };
            File.WriteAllText("Boleto.json", JsonConvert.SerializeObject(sample));
        }
    }
}