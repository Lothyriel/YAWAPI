namespace WebAPI.Domain
{
    public class Area
    {
        public double TotalSquareMeters { get; }

        public double TotalAcres => TotalSquareMeters / MetersInAcres;
        public double TotalHectares => TotalSquareMeters / MetersInHectare;

        public const double MetersInAcres = 4046.86;
        public const double MetersInHectare = 10000;

        public Area(double totalSquareMeters)
        {
            TotalSquareMeters = totalSquareMeters;
        }

        public Dictionary<object, object> Result()
        {
            return new Dictionary<object, object>
            {
                ["SquareMeters"] = TotalSquareMeters,
                ["Hectares"] = TotalHectares,
                ["Acres"] = TotalAcres
            };
        }

        public static Area FromAcres(double acres)
        {
            return new Area(acres * MetersInAcres);
        }

        public static Area FromHectares(double hectares)
        {
            return new Area(hectares * MetersInHectare);
        }

        public static Area FromMeters(double meters)
        {
            return new Area(meters);
        }
    }
}