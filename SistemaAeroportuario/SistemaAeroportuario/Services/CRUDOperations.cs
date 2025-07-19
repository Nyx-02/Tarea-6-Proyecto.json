using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

public class CRUDOperations<T> where T : class, IJsonStorage, new()
{
    public void MenuCRUD()
    {
        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"════════ GESTIÓN DE {typeof(T).Name.ToUpper()}S ════════");
            Console.ResetColor();

            Console.WriteLine("1. Agregar");
            Console.WriteLine("2. Actualizar");
            Console.WriteLine("3. Eliminar");
            Console.WriteLine("4. Listar Todos");
            Console.WriteLine("5. Regresar");
            Console.Write("\nSeleccione: ");

            if (!int.TryParse(Console.ReadLine(), out int opcion))
            {
                ConsoleUtils.MostrarError("¡Debe ingresar un número!");
                continue;
            }

            switch (opcion)
            {
                case 1: Agregar(); break;
                case 2: Actualizar(); break;
                case 3: Eliminar(); break;
                case 4: Listar(); break;
                case 5: return;
                default: ConsoleUtils.MostrarError("Opción inválida."); break;
            }
        }
    }

    public void Agregar()
    {
        try
        {
            var entidad = new T();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"══════ AGREGAR {typeof(T).Name.ToUpper()} ══════");
            Console.ResetColor();

            // Lógica especial para Pasajero (vinculación con boleto)
            if (typeof(T) == typeof(Pasajero))
            {
                Boleto.MostrarBoletosDisponibles();
                var boletos = Boleto.ListarBoletosDisponibles();
                if (!boletos.Any()) return;

                Console.Write("\nIngrese código de boleto: ");
                string codigoBoleto = Console.ReadLine();
                var boleto = boletos.FirstOrDefault(b => b.CodigoBoleto == codigoBoleto);

                if (boleto == null)
                {
                    ConsoleUtils.MostrarError("¡Boleto no encontrado!");
                    return;
                }
                ((Pasajero)(object)entidad).NumeroBoleto = codigoBoleto;
            }

            AsignarValores(entidad);
            entidad.SaveToJson();
            ConsoleUtils.MostrarExito("¡Registro agregado exitosamente!");
        }
        catch (Exception ex)
        {
            ConsoleUtils.MostrarError($"Error: {ex.Message}");
        }
    }

    public void Actualizar()
    {
        try
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"══════ ACTUALIZAR {typeof(T).Name.ToUpper()} ══════");
            Console.ResetColor();

            // Mostrar lista previa para selección
            Listar(mostrarSoloResumen: true);

            Console.Write("\nIngrese ID/código a actualizar: ");
            string id = Console.ReadLine();

            var entidad = BuscarPorId(id);
            if (entidad == null)
            {
                ConsoleUtils.MostrarError("¡Registro no encontrado!");
                return;
            }

            Console.WriteLine("\nDATOS ACTUALES:");
            MostrarRegistro(entidad);
            Console.WriteLine("\nINGRESE NUEVOS DATOS:");
            AsignarValores(entidad);

            // Guardar cambios
            var todos = new T().LoadFromJson();
            var index = todos.FindIndex(e =>
                e is Vuelo v && v.CodigoVuelo == id ||
                e is Pasajero p && p.NumeroBoleto == id ||
                e is Avion a && a.Matricula == id
            );
            todos[index] = entidad;
            File.WriteAllText($"{typeof(T).Name}.json", JsonConvert.SerializeObject(todos));

            ConsoleUtils.MostrarExito("¡Registro actualizado!");
        }
        catch (Exception ex)
        {
            ConsoleUtils.MostrarError($"Error: {ex.Message}");
        }
    }

    public void Eliminar()
    {
        try
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"══════ ELIMINAR {typeof(T).Name.ToUpper()} ══════");
            Console.ResetColor();

            // Mostrar lista previa para selección
            Listar(mostrarSoloResumen: true);

            Console.Write("\nIngrese ID/código a eliminar: ");
            string id = Console.ReadLine();

            var todos = new T().LoadFromJson();
            var entidad = todos.FirstOrDefault(e =>
                e is Vuelo v && v.CodigoVuelo == id ||
                e is Pasajero p && p.NumeroBoleto == id ||
                e is Avion a && a.Matricula == id
            );

            if (entidad == null)
            {
                ConsoleUtils.MostrarError("¡Registro no encontrado!");
                return;
            }

            todos.Remove(entidad);
            File.WriteAllText($"{typeof(T).Name}.json", JsonConvert.SerializeObject(todos));
            ConsoleUtils.MostrarExito("¡Registro eliminado!");
        }
        catch (Exception ex)
        {
            ConsoleUtils.MostrarError($"Error: {ex.Message}");
        }
    }

    public void Listar(bool mostrarSoloResumen = false)
    {
        try
        {
            var registros = new T().LoadFromJson();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"══════ LISTADO DE {typeof(T).Name.ToUpper()}S ══════");
            Console.ResetColor();

            if (!registros.Any())
            {
                Console.WriteLine("\nNo hay registros.");
                ConsoleUtils.PresioneParaContinuar();
                return;
            }

            foreach (var registro in registros)
            {
                if (mostrarSoloResumen)
                {
                    // Mostrar solo información clave para selección
                    Console.WriteLine($"- {ObtenerId(registro)}: {ObtenerNombreResumen(registro)}");
                }
                else
                {
                    MostrarRegistro(registro);
                    Console.WriteLine(new string('─', 40));
                }
            }

            if (!mostrarSoloResumen)
                ConsoleUtils.PresioneParaContinuar();
        }
        catch (Exception ex)
        {
            ConsoleUtils.MostrarError($"Error: {ex.Message}");
        }
    }

    // --- Métodos auxiliares ---
    private void AsignarValores(object entidad)
    {
        var propiedades = entidad.GetType().GetProperties()
            .Where(p => p.CanWrite && p.Name != "Id" && !p.PropertyType.IsGenericType);

        foreach (var prop in propiedades)
        {
            while (true)
            {
                try
                {
                    Console.Write($"{prop.Name}: ");
                    string valor = Console.ReadLine();

                    // Validaciones básicas
                    if (string.IsNullOrWhiteSpace(valor))
                    {
                        ConsoleUtils.MostrarError("¡Campo requerido!");
                        continue;
                    }

                    // Validar tipo de dato
                    if (prop.PropertyType == typeof(int) && !int.TryParse(valor, out _))
                    {
                        ConsoleUtils.MostrarError("¡Debe ser un número entero!");
                        continue;
                    }

                    // Asignar valor
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

    private object BuscarPorId(string id)
    {
        return new T().LoadFromJson().FirstOrDefault(e =>
            e is Vuelo v && v.CodigoVuelo == id ||
            e is Pasajero p && p.NumeroBoleto == id ||
            e is Avion a && a.Matricula == id
        );
    }

    private string ObtenerId(object entidad)
    {
        return entidad switch
        {
            Vuelo v => v.CodigoVuelo,
            Pasajero p => p.NumeroBoleto,
            Avion a => a.Matricula,
            _ => "ID"
        };
    }

    private string ObtenerNombreResumen(object entidad)
    {
        return entidad switch
        {
            Vuelo v => $"{v.Origen} → {v.Destino}",
            Pasajero p => $"{p.Nombre} {p.Apellido}",
            Avion a => a.Modelo,
            _ => entidad.ToString()
        };
    }

    private void MostrarRegistro(object entidad)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        foreach (var prop in entidad.GetType().GetProperties())
        {
            if (prop.PropertyType.IsGenericType) continue;
            var valor = prop.GetValue(entidad)?.ToString() ?? "N/A";
            Console.WriteLine($"{prop.Name.PadRight(15)}: {valor}");
        }
        Console.ResetColor();
    }
}