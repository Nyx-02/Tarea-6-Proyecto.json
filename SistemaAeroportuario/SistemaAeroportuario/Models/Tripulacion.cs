// Models/Tripulacion.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Tripulacion : Persona, IJsonStorage
{
    public string Cargo { get; set; }
    public int HorasVuelo { get; set; }
    public DateTime FechaContratacion { get; set; }
    public string Licencia { get; set; }
    public bool CertificadoMedico { get; set; }
    public string Idiomas { get; set; }
    public bool Disponible { get; set; }

    public override string ObtenerTipo() => "Tripulacion";

    public bool ValidarLicencia() => !string.IsNullOrEmpty(Licencia) && Licencia.Length == 10;
    public void AgregarHorasVuelo(int horas) => HorasVuelo += horas;
    public bool EstaDisponible() => Disponible;

    public void SaveToJson()
    {
        var tripulacion = LoadFromJson().Cast<Tripulacion>().ToList();
        tripulacion.Add(this);
        File.WriteAllText("Tripulacion.json", JsonConvert.SerializeObject(tripulacion.Take(100)));
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
                    TipoIdentificacion = "Licencia",
                    NumeroIdentificacion = "T987654",
                    Nombre = "Ana",
                    Apellido = "Gomez",
                    Nacionalidad = "Honduras",
                    FechaNacimiento = new DateTime(1990, 8, 22),
                    Genero = "Femenino",
                    Email = "ana@aerolinea.com",
                    Cargo = "Piloto",
                    HorasVuelo = 5000,
                    Disponible = true
                }
            };
            File.WriteAllText("Tripulacion.json", JsonConvert.SerializeObject(sample));
        }
    }
}