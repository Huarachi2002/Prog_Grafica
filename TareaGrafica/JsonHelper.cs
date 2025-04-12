using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TareaGrafica
{
    public static class JsonHelper
    {
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true,
        };

        public static bool SerializeToJson<T>(T obj, string nameFile)
        {
            try
            {
                string carpetaDatos = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                Directory.CreateDirectory(carpetaDatos);
                string rutaCompleta = Path.Combine(carpetaDatos, $"{nameFile}.json");

                string jsonString = JsonSerializer.Serialize(obj, _options);
                File.WriteAllText(rutaCompleta, jsonString);
                Console.WriteLine(rutaCompleta);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al serializar: {ex.Message}");
                return false;
            }
        }

        public static T DeserializeFromJson<T>(string nameFile)
        {
            try
            {
                string carpetaDatos = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                string rutaCompleta = Path.Combine(carpetaDatos, $"{nameFile}.json");

                if (!File.Exists(rutaCompleta))
                {
                    Console.WriteLine($"El archivo {rutaCompleta} no existe");
                    return default;
                }

                string jsonString = File.ReadAllText(rutaCompleta);
                Console.WriteLine($"Contenido JSON leído ({jsonString.Length} bytes)");

                return JsonSerializer.Deserialize<T>(jsonString, _options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al deserializar: {ex.Message}");
                return default;
            }
        }

        public static async Task<bool> SerializeToJsonAsync<T>(T obj, string filePath)
        {
            try
            {
                using FileStream createStream = File.Create(filePath);
                await JsonSerializer.SerializeAsync(createStream, obj, _options);
                Console.WriteLine(filePath);    
                await createStream.DisposeAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al serializar asincrónicamente: {ex.Message}");
                return false;
            }
        }

        public static async Task<T> DeserializeFromJsonAsync<T>(string filePath)
        {
            try
            {
                using FileStream openStream = File.OpenRead(filePath);
                T obj = await JsonSerializer.DeserializeAsync<T>(openStream, _options);
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al deserializar asincrónicamente: {ex.Message}");
                return default;
            }
        }
    }
}
