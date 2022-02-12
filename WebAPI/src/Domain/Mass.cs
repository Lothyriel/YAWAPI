namespace WebAPI.Domain
{
    public class Mass
    {
        public double TotalKilograms { get; }

        public double TotalPounds => TotalKilograms / KilogramsInPound;
        public double TotalTons => TotalKilograms / KilogramsInTon;

        public const double KilogramsInTon = 1000;
        public const double KilogramsInPound = 0.453592;

        public Mass(double totalKilograms)
        {
            TotalKilograms = totalKilograms;
        }

        public Dictionary<object, object> Result()
        {
            return new Dictionary<object, object>
            {
                ["Kilograms"] = TotalKilograms,
                ["Tons"] = TotalTons,
                ["Pounds"] = TotalPounds
            };
        }

        public static Mass FromTons(double tons)
        {
            return new Mass(tons * KilogramsInTon);
        }

        public static Mass FromPounds(double pounds)
        {
            return new Mass(pounds * KilogramsInPound);
        }

        public static Mass FromKilograms(double kilograms)
        {
            return new Mass(kilograms);
        }
    }
}