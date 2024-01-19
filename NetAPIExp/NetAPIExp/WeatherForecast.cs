namespace NetAPIExp
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public float CurrentDay
        {
            get
            {
                return Date.Day;
            }
            set
            {
                CurrentDay = value;
                Date.AddDays((int)value);
            }
        }
        public string? Summary { get; set; }
    }
}
