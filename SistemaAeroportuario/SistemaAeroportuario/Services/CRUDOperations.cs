using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;

public class CRUDOperations<T> where T : class, IJsonStorage, new()
{
    public void Agregar()
    {
        try
        {
            var entidad = new T();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"══════ AGREGAR {typeof(T).Name.ToUpper()} ══════");
            Console.ResetColor();
            Console.WriteLine();
            AsignarValores(entidad);
            entidad.SaveToJson();
            ConsoleUtils.MostrarExito("Registro agregado exitosamente!");
        }
        catch (Exception ex)
        {
            ConsoleUtils.MostrarError($"Error al agregar: {ex.Message}");
        }
    }

    public void Buscar()
    {
        try
        {
            string identificador;
            if (typeof(T) == typeof(Pasajero) || typeof(T) == typeof(Tripulacion))
            {
                Console.Write("\nIngrese Número de Identificación: ");
                identificador = Console.ReadLine();
            }
            else if (typeof(T) == typeof(Vuelo))
            {
                Console.Write("\nIngrese Código de Vuelo (Ej: AV-101): ");
                identificador = Console.ReadLine();
            }
            else if (typeof(T) == typeof(Avion))
            {
                Console.Write("\nIngrese Matrícula (Ej: HR-A350): ");
                identificador = Console.ReadLine();
            }
            else if (typeof(T) == typeof(Aeropuerto))
            {
                Console.Write("\nIngrese Código IATA (Ej: TGU): ");
                identificador = Console.ReadLine();
            }
            else if (typeof(T) == typeof(Boleto))
            {
                Console.Write("\nIngrese Código de Boleto (Ej: B-001): ");
                identificador = Console.ReadLine();
            }
            else
            {
                Console.Write("\nIngrese ID o código: ");
                identificador = Console.ReadLine();
            }

            var encontrado = new T().LoadFromJson().FirstOrDefault(e =>
                (e is Persona p && p.NumeroIdentificacion == identificador) ||
                (e is Vuelo v && v.CodigoVuelo == identificador) ||
                (e is Avion a && a.Matricula == identificador) ||
                (e is Aeropuerto ap && ap.CodigoIATA == identificador) ||
                (e is Boleto b && b.CodigoBoleto == identificador));

            if (encontrado == null)
            {
                ConsoleUtils.MostrarError("Registro no encontrado");
                return;
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"══════ DETALLES DEL REGISTRO ══════");
            Console.ResetColor();
            Console.WriteLine();
            MostrarRegistro(encontrado);
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            ConsoleUtils.MostrarError($"Error al buscar: {ex.Message}");
        }
    }

    public void Actualizar()
    {
        try
        {
            string identificador;
            if (typeof(T) == typeof(Pasajero) || typeof(T) == typeof(Tripulacion))
            {
                Console.Write("\nIngrese Número de Identificación: ");
                identificador = Console.ReadLine();
            }
            else if (typeof(T) == typeof(Vuelo))
            {
                Console.Write("\nIngrese Código de Vuelo: ");
                identificador = Console.ReadLine();
            }
            else if (typeof(T) == typeof(Avion))
            {
                Console.Write("\nIngrese Matrícula: ");
                identificador = Console.ReadLine();
            }
            else if (typeof(T) == typeof(Aeropuerto))
            {
                Console.Write("\nIngrese Código IATA: ");
                identificador = Console.ReadLine();
            }
            else if (typeof(T) == typeof(Boleto))
            {
                Console.Write("\nIngrese Código de Boleto: ");
                identificador = Console.ReadLine();
            }
            else
            {
                Console.Write("\nIngrese ID o código: ");
                identificador = Console.ReadLine();
            }

            var entidades = new T().LoadFromJson().Cast<object>().ToList();
            var entidad = entidades.FirstOrDefault(e =>
                (e is Persona p && p.NumeroIdentificacion == identificador) ||
                (e is Vuelo v && v.CodigoVuelo == identificador) ||
                (e is Avion a && a.Matricula == identificador) ||
                (e is Aeropuerto ap && ap.CodigoIATA == identificador) ||
                (e is Boleto b && b.CodigoBoleto == identificador));

            if (entidad == null)
            {
                ConsoleUtils.MostrarError("Registro no encontrado");
                return;
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"══════ ACTUALIZAR {typeof(T).Name.ToUpper()} ══════");
            Console.ResetColor();
            Console.WriteLine("\nDATOS ACTUALES:");
            MostrarRegistro(entidad);
            Console.WriteLine("\nINGRESE NUEVOS DATOS:");
            AsignarValores(entidad);

            File.WriteAllText($"{typeof(T).Name}.json", JsonConvert.SerializeObject(entidades.Take(100)));
            ConsoleUtils.MostrarExito("Registro actualizado exitosamente!");
        }
        catch (Exception ex)
        {
            ConsoleUtils.MostrarError($"Error al actualizar: {ex.Message}");
        }
    }

    public void Eliminar()
    {
        try
        {
            string identificador;
            if (typeof(T) == typeof(Pasajero) || typeof(T) == typeof(Tripulacion))
            {
                Console.Write("\nIngrese Número de Identificación: ");
                identificador = Console.ReadLine();
            }
            else if (typeof(T) == typeof(Vuelo))
            {
                Console.Write("\nIngrese Código de Vuelo: ");
                identificador = Console.ReadLine();
            }
            else if (typeof(T) == typeof(Avion))
            {
                Console.Write("\nIngrese Matrícula: ");
                identificador = Console.ReadLine();
            }
            else if (typeof(T) == typeof(Aeropuerto))
            {
                Console.Write("\nIngrese Código IATA: ");
                identificador = Console.ReadLine();
            }
            else if (typeof(T) == typeof(Boleto))
            {
                Console.Write("\nIngrese Código de Boleto: ");
                identificador = Console.ReadLine();
            }
            else
            {
                Console.Write("\nIngrese ID o código: ");
                identificador = Console.ReadLine();
            }

            var entidades = new T().LoadFromJson().Cast<object>().ToList();
            var entidad = entidades.FirstOrDefault(e =>
                (e is Persona p && p.NumeroIdentificacion == identificador) ||
                (e is Vuelo v && v.CodigoVuelo == identificador) ||
                (e is Avion a && a.Matricula == identificador) ||
                (e is Aeropuerto ap && ap.CodigoIATA == identificador) ||
                (e is Boleto b && b.CodigoBoleto == identificador));

            if (entidad == null)
            {
                ConsoleUtils.MostrarError("Registro no encontrado");
                return;
            }

            entidades.Remove(entidad);
            File.WriteAllText($"{typeof(T).Name}.json", JsonConvert.SerializeObject(entidades.Take(100)));
            ConsoleUtils.MostrarExito("Registro eliminado exitosamente!");
        }
        catch (Exception ex)
        {
            ConsoleUtils.MostrarError($"Error al eliminar: {ex.Message}");
        }
    }

    public void Listar()
    {
        try
        {
            var entidades = new T().LoadFromJson();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"══════ LISTADO DE {typeof(T).Name.ToUpper()}S ══════");
            Console.ResetColor();
            Console.WriteLine();

            if (!entidades.Any())
            {
                Console.WriteLine("No hay registros disponibles.");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            foreach (var entidad in entidades)
            {
                MostrarRegistro(entidad);
                Console.WriteLine(new string('─', 50));
            }
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            ConsoleUtils.MostrarError($"Error al listar: {ex.Message}");
        }
    }

    private void AsignarValores(object entidad)
    {
        var propiedades = entidad.GetType().GetProperties();
        foreach (var prop in propiedades)
        {
            if (!prop.CanWrite || prop.Name == "Id" || prop.Name == "HorasVueloTotales")
                continue;

            while (true)
            {
                try
                {
                    Console.Write($"{prop.Name.Replace("_", " ")}: ");
                    string valor = Console.ReadLine();

                    // Validación 1: Campo requerido
                    if (string.IsNullOrWhiteSpace(valor))
                    {
                        ConsoleUtils.MostrarError("¡Este campo es obligatorio!");
                        continue;
                    }

                    // Validación 2: Tipos de datos
                    if (prop.PropertyType == typeof(int) && !int.TryParse(valor, out _))
                    {
                        ConsoleUtils.MostrarError("¡Debe ser un número entero!");
                        continue;
                    }

                    if (prop.PropertyType == typeof(DateTime) &&
                       !DateTime.TryParseExact(valor, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    {
                        ConsoleUtils.MostrarError("¡Formato inválido! Use dd/MM/yyyy");
                        continue;
                    }

                    if (prop.PropertyType == typeof(decimal) && !decimal.TryParse(valor, out _))
                    {
                        ConsoleUtils.MostrarError("¡Debe ser un valor decimal!");
                        continue;
                    }

                    // Validación 3: Reglas específicas por campo
                    switch (prop.Name)
                    {
                        case "Email":
                            if (!Regex.IsMatch(valor, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                            {
                                ConsoleUtils.MostrarError("¡Email inválido!");
                                continue;
                            }
                            break;

                        case "NumeroIdentificacion":
                            if (valor.Length < 5)
                            {
                                ConsoleUtils.MostrarError("¡Mínimo 5 caracteres!");
                                continue;
                            }
                            break;

                        case "Genero":
                            var opcionesGenero = new[] { "Masculino", "Femenino", "No Binario" };
                            if (!opcionesGenero.Contains(valor))
                            {
                                ConsoleUtils.MostrarError($"Opciones válidas: {string.Join(", ", opcionesGenero)}");
                                continue;
                            }
                            break;

                        case "CodigoVuelo":
                            if (!Regex.IsMatch(valor, @"^[A-Z]{2}-\d{3}$"))
                            {
                                ConsoleUtils.MostrarError("Formato inválido! Ej: AV-101");
                                continue;
                            }
                            break;

                        case "Matricula":
                            if (!Regex.IsMatch(valor, @"^[A-Z]{2}-[A-Z]\d{3}$"))
                            {
                                ConsoleUtils.MostrarError("Formato inválido! Ej: HR-A350");
                                continue;
                            }
                            break;

                        case "FechaNacimiento":
                            DateTime fechaNac = DateTime.ParseExact(valor, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            int edad = DateTime.Now.Year - fechaNac.Year;
                            if (edad < 18)
                            {
                                ConsoleUtils.MostrarError("¡Debe ser mayor de edad!");
                                continue;
                            }
                            break;
                    }

                    // Asignación segura
                    if (prop.PropertyType == typeof(bool))
                        valor = valor.ToLower() == "si" ? "true" : "false";

                    prop.SetValue(entidad, Convert.ChangeType(valor, prop.PropertyType));
                    break;
                }
                catch (Exception ex)
                {
                    ConsoleUtils.MostrarError($"Error: {ex.Message}");
                }
            }
        }
    }

    private void MostrarRegistro(object entidad)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        foreach (var prop in entidad.GetType().GetProperties())
        {
            if (prop.PropertyType == typeof(List<>) || prop.PropertyType.IsArray)
                continue;

            var valor = prop.GetValue(entidad);

            if (valor is DateTime)
                valor = ((DateTime)valor).ToString("dd/MM/yyyy HH:mm");

            Console.WriteLine($"{prop.Name.Replace("_", " "),-25}: {valor}");
        }
        Console.ResetColor();
    }
}