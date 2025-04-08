namespace WeatherApp
{
    public class WeatherInfo
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public float Temp { get; set; }
        public required string Description { get; set; }

        public override string ToString()
        {
            return $"Miasto: {Name}\nTemperatura: {Temp}°C\nWarunki: {Description}";
        }
    }

    public class ApiResponse
{
    public string name { get; set; } = string.Empty;
    public Main main { get; set; } = new Main();
    public Weather[] weather { get; set; } = Array.Empty<Weather>();
}

    public class Weather
    {
        public string? description { get; set; }
    }

    public class Main
    {
        public float temp { get; set; }
    }
}