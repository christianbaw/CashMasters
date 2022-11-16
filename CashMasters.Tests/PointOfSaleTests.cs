using CashMasters.Configuration;
using CashMasters.Entities;
using CashMasters.Exceptions;
using CashMasters.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace CashMasters.Tests
{
    public class PointOfSaleTests
    {
        public IOptions<CurrencySettings> CurrencySettingsOptions { get; private set; }

        public PointOfSaleTests()
        {
            var currencySettings = new CurrencySettings("MX", new Dictionary<string, double[]>
            {
                { "US", new double[] { 0.01, 0.05, 0.10, 0.25, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00 } },
                { "MX", new double[] { 0.05, 0.10, 0.20, 0.50, 1.00, 2.00, 5.00, 10.00, 20.00, 50.00, 100.00 } }
            });

            var currencyOptionsMock = new Mock<IOptions<CurrencySettings>>();

            currencyOptionsMock.Setup(ap => ap.Value).Returns(currencySettings);

            CurrencySettingsOptions = currencyOptionsMock.Object;
        }

        [Fact]
        public void ProcessPayment_ShouldReturnException_WhenDenominationIsNotValid()
        {
            //Arrange
            var pos = new PointOfSale(CurrencySettingsOptions);

            var payment = new List<Payment>
            {
                new Payment(300, 4)
            };

            //Act
            var act = () => pos.ProcessPayment(1000, payment);

            //Assert
            Assert.Throws<InvalidDenominationException>(act);
        }

        [Fact]
        public void ProcessPayment_ShouldReturnException_When_Payment_Not_Enough()
        {
            //Arrange
            var pos = new PointOfSale(CurrencySettingsOptions);

            var payment = new List<Payment>
            {
                new Payment(50, 1),
                new Payment(100, 1)
            };

            //Act
            var act = () => pos.ProcessPayment(1000, payment);

            //Assert
            Assert.Throws<PaymentNotEnoughToCoverTotalPrice>(act);
        }
    }
}