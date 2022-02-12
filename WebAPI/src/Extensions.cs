namespace WebAPI
{
    public static class Extensions
    {
        public static Dictionary<object, object> Result(this TimeSpan span)
        {
            return new Dictionary<object, object>
            {
                ["Hours"] = span.TotalHours,
                ["Minutes"] = span.TotalMinutes,
                ["Days"] = span.TotalDays
            };
        }
    }
}