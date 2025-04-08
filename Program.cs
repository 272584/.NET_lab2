using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WeatherApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Podaj nazwę miasta:");
            string? city = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(city))
            {
                Console.WriteLine("Miasto nie może być puste!");
                return;
            }

            try
            {
                using var dbContext = new WeatherDbContext();

                var existingWeather = dbContext.WeatherInfos
                    .FirstOrDefault(w => w.Name.ToLower() == city.ToLower());

                if (existingWeather != null)
                {
                    Console.WriteLine("\nDane pogodowe z bazy:");
                    Console.WriteLine(existingWeather);
                }
                else
                {
                    Console.WriteLine($"\nPobieranie danych pogodowych dla {city}...");
                    await GetWeatherDataFromApi(city, dbContext);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nWystąpił błąd: {ex.Message}");
            }
        }

        static async Task GetWeatherDataFromApi(string city, WeatherDbContext dbContext)
        {
            string apiKey = "3dcaf9e62c63c9bd4c5ae22896e21494";
            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=pl";

            try
            {
                using HttpClient client = new();
                string response = await client.GetStringAsync(apiUrl);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(response, options);

                if (apiResponse?.name != null && apiResponse.main != null && 
                    apiResponse.weather != null && apiResponse.weather.Length > 0)
                {
                    var weatherInfo = new WeatherInfo
                    {
                        Name = apiResponse.name,
                        Temp = apiResponse.main.temp,
                        Description = apiResponse.weather?[0]?.description ?? "Brak dostępnego opisu",
                    };

                    dbContext.WeatherInfos.Add(weatherInfo);
                    await dbContext.SaveChangesAsync();

                    Console.WriteLine("\nDane pobrane z API i zapisane do bazy:");
                    Console.WriteLine(weatherInfo);
                }
                else
                {
                    Console.WriteLine("\nNie udało się pobrać pełnych danych pogodowych.");
                    if (apiResponse?.name == null)
                        Console.WriteLine("Brak nazwy miasta w odpowiedzi.");
                    if (apiResponse?.main == null)
                        Console.WriteLine("Brak danych głównych (temperatura itp.).");
                    if (apiResponse?.weather == null || apiResponse.weather.Length == 0)
                        Console.WriteLine("Brak opisu warunków pogodowych.");
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"\nBłąd podczas łączenia z API: {httpEx.Message}");
                if (httpEx.StatusCode.HasValue)
                {
                    Console.WriteLine($"Kod statusu: {httpEx.StatusCode}");
                    if (httpEx.StatusCode == System.Net.HttpStatusCode.NotFound)
                        Console.WriteLine("Miasto nie zostało znalezione.");
                }
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"\nBłąd podczas przetwarzania danych: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nNieoczekiwany błąd: {ex.Message}");
            }
        }
    }

    public class WeatherDbContext : DbContext
    {
        public DbSet<WeatherInfo> WeatherInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=weather.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherInfo>().HasKey(w => w.Id);
        }
    }
}