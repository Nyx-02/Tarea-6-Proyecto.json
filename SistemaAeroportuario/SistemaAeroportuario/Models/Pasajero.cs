// Models/Pasajero.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Pasajero : Persona, IJsonStorage
{
    public string TipoPasaje { get; set; }
    public bool RequiereAsistencia { get; set; }
    public string NumeroAsiento { get; set; }
    public string Alergias { get; set; }
    public int Millas { get; set; }
    public string PreferenciasComida { get; set; }
    public bool VisaValida { get; set; }

    public override string ObtenerTipo() => "Pasajero";

    public bool VerificarVigenciaVisa() => VisaValida;
    public void ActualizarMillas(int millas) => Millas += millas;
    public string ObtenerPreferencias() => PreferenciasComida;

    public void SaveToJson()
    {
        var pasajeros = LoadFromJson().Cast<Pasajero>().ToList();
        pasajeros.Add(this);
        File.WriteAllText("Pasajero.json", JsonConvert.SerializeObject(pasajeros.Take(100)));
    }

    public List<object> LoadFromJson()
    {
        return File.Exists("Pasajero.json")
            ? JsonConvert.DeserializeObject<List<Pasajero>>(File.ReadAllText("Pasajero.json")).Cast<object>().ToList()
            : new List<object>();
    }

    public void InitializeSampleData()
    {
        if (!File.Exists("Pasajero.json"))
        {
            var sample = new List<Pasajero>
            {
                new Pasajero {
                    TipoIdentificacion = "Pasaporte",
                    NumeroIdentificacion = "P12345678",
                    Nombre = "Juan",
                    Apellido = "Perez",
                    Nacionalidad = "Honduras",
                    FechaNacimiento = new DateTime(1985, 5, 15),
                    Genero = "Masculino",
                    Email = "juan@example.com",
                    TipoPasaje = "Primera Clase",
                    RequiereAsistencia = true,
                    VisaValida = true
                }
            };
            File.WriteAllText("Pasajero.json", JsonConvert.SerializeObject(sample));
        }
    }
}