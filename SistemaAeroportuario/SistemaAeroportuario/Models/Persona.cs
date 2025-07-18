// Models/Persona.cs
using System;

public abstract class Persona
{
    public string TipoIdentificacion { get; set; }
    public string NumeroIdentificacion { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Nacionalidad { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Genero { get; set; }
    public string Email { get; set; }

    public abstract string ObtenerTipo();
}