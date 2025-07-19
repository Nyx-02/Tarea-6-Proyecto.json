public abstract class Persona
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Identificacion { get; set; }

    public abstract void Agregar();
}