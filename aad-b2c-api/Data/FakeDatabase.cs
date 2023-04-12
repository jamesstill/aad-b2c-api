using aad_b2c_api.Models;

namespace aad_b2c_api.Data
{
    public static class FakeDatabase
    {
        static Guid forecastReaderId = new("91AF91D8-3531-44E0-854B-A2C93897CE37");
        static Guid forecastWriterId = new("5B5A42EC-4AAA-47FE-8044-71957D80AE38");

        public static IList<ClaimDTO> Claims { get; }

        static FakeDatabase()
        {
            Claims = new List<ClaimDTO>()
            {
                new ClaimDTO { Id = forecastReaderId, Name = "WeatherForecastReader" },
                new ClaimDTO { Id = forecastWriterId, Name = "WeatherForecastWriter" }
            };
        }
    }
}
