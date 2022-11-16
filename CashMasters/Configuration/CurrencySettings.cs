namespace CashMasters.Configuration
{
    public class CurrencySettings
    {
        public CurrencySettings()
        {
        }

        public CurrencySettings(string country, Dictionary<string, double[]> countries)
        {
            Country = country;
            Countries = countries;
        }

        public string Country { get; private set; }

        private Dictionary<string, double[]> Countries { get; set; }

        public double[] Denominations
        {
            get
            {
                if (Countries == null)
                    return Array.Empty<double>();

                var foundRequestedCurrency = Countries.TryGetValue(Country, out var denominations);

                if (!foundRequestedCurrency)
                    throw new ApplicationException("Country not supported");

                return denominations;
            }
        }
    }
}
