using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

public class Clima
{
    private static readonly HttpClient client = new HttpClient();
    private static readonly string apiKey = "b16b6bb75382da49ce358a5c029db59a"; // Reemplaza con tu clave de API

    public static async Task<ServerResult> ObtenerClimaAsync(string ciudad)
    {
        if (string.IsNullOrWhiteSpace(ciudad))
        {
            return new ServerResult(false, "La ciudad es obligatoria");
        }

        try
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?q={ciudad}&appid={apiKey}&units=metric";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new ServerResult(false, "No se pudo obtener el clima. Verifica el nombre de la ciudad.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var climaData = JsonSerializer.Deserialize<ClimaResponse>(jsonResponse);

            var mensaje = $"En {ciudad}, la temperatura actual es de {climaData.Main.Temp}°C con {climaData.Weather[0].Description}.";
            return new ServerResult(true, mensaje);
        }
        catch (Exception ex)
        {
            return new ServerResult(false, $"Error al obtener el clima: {ex.Message}");
        }
    }
}

public class ClimaResponse
{
    public MainData Main { get; set; }
    public WeatherData[] Weather { get; set; }
}

public class MainData
{
    public float Temp { get; set; }
}

public class WeatherData
{
    public string Description { get; set; }
}

// Clase ServerResult
public class ServerResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }

    public ServerResult(bool success, string message)
    {
        Success = success;
        Message = message;
        Errors = new List<string>();
    }

    public ServerResult(bool success, string message, List<string> errors)
    {
        Success = success;
        Message = message;
        Errors = errors;
    }
}
