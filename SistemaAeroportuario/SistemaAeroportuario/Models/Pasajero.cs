using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class Pasajero : Persona, IJsonStorage
{
    public string NumeroBoleto { get; set; }
    public string VueloAsignado { get; set; }
    public string TipoPasaje { get; set; }  // "Económica", "Ejecutiva", "Primera"
    public bool RequiereAsistencia { get; set; }

    public override void Agregar()
    {
        Boleto.MostrarBoletosDisponibles();
        var boletos = Boleto.ListarBoletosDisponibles();
        if (!boletos.Any()) return;

        Boleto boletoSeleccionado = null;
        while (boletoSeleccionado == null)
        {
            Console.Write("\nIngrese código de boleto: ");
            string codigo = Console.ReadLine().Trim().ToUpper();
            boletoSeleccionado = boletos.FirstOrDefault(b => b.CodigoBoleto == codigo);

            if (boletoSeleccionado == null)
                ConsoleUtils.MostrarError("¡Código inválido o boleto ya asignado!");
        }

        this.NumeroBoleto = boletoSeleccionado.CodigoBoleto;
        this.VueloAsignado = boletoSeleccionado.CodigoVuelo;

        Console.Write("Nombre: ");
        this.Nombre = Console.ReadLine();

        Console.Write("Tipo de pasaje (Económica/Ejecutiva/Primera): ");
        this.TipoPasaje = Console.ReadLine();

        this.SaveToJson();
        ConsoleUtils.MostrarExito($"Pasajero registrado con boleto {this.NumeroBoleto}!");
    }

    public void SaveToJson()
    {
        var pasajeros = LoadFromJson().Cast<Pasajero>().ToList();
        pasajeros.Add(this);
        File.WriteAllText("Pasajero.json", JsonConvert.SerializeObject(pasajeros));
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
                    NumeroBoleto = "B-001",
                    Nombre = "Juan Perez",
                    VueloAsignado = "AV-101",
                    TipoPasaje = "Económica"
                }
            };
            File.WriteAllText("Pasajero.json", JsonConvert.SerializeObject(sample));
        }
    }
}