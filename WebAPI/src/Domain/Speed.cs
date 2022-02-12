namespace WebAPI.Domain
{
    public class Speed
    {
        public const double KphInMph = 1.60934;

        public Speed(double kph)
        {
            TotalKph = kph;
        }

        public double TotalKph { get; }

        public double TotalMph => TotalKph / KphInMph;

        public Dictionary<object, object> Result()
        {
            return new Dictionary<object, object>
            {
                ["Mph"] = TotalMph,
                ["Kph"] = TotalKph
            };
        }

        public static Speed FromKph(double kph)
        {
            return new Speed(kph);
        }

        public static Speed FromMph(double mph)
        {
            return new Speed(mph * KphInMph);
        }
    }
}