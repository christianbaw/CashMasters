using CashMasters.Configuration;
using CashMasters.Entities;
using CashMasters.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CashMasters
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var host = BuildHost();
            using IServiceScope serviceScope = host.Services.CreateScope();
            var totalPrice = 500.37;
            var payment = new List<Payment> { new Payment(100, 7), new Payment(20.00, 3), new Payment(0.20, 3) };
            var pos = serviceScope.ServiceProvider.GetRequiredService<IPointOfSale>();
            
            //Execution of process payment starts from Main method.
            var change = pos.ProcessPayment(totalPrice, payment); 
            
        }

        /// <summary>
        /// Retrieves the denomination configuration, to configure default denomination refer to the file appsettings.json
        /// </summary>
        static IHost BuildHost()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddScoped<IPointOfSale, PointOfSale>();
                    services.Configure<CurrencySettings>(options => configuration.GetSection(nameof(CurrencySettings)).Bind(options, c => c.BindNonPublicProperties = true));
                })
                .Build();

            return host;
        }
    }
}