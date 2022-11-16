using CashMasters.Configuration;
using CashMasters.Entities;
using CashMasters.Exceptions;
using CashMasters.Extensions;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

namespace CashMasters.Services
{   
    public interface IPointOfSale
    {
        IEnumerable<Change> ProcessPayment(double totalPrice, IEnumerable<Payment> payment);
    }
    /// <summary>
    /// Service in charge of process payment and returns the smallest number of bills and coins for change
    /// </summary>
    public class PointOfSale : IPointOfSale
    {
        private readonly CurrencySettings _currency;

        public PointOfSale(IOptions<CurrencySettings> currencyOptions)
        {
            _currency = currencyOptions.Value;
        }

        public IEnumerable<Change> ProcessPayment(double totalPrice, IEnumerable<Payment> payment)
        {
            var paymentDenominations = payment.Select(x => x.Denomination);
            if (paymentDenominations.Any(item => !_currency.Denominations.Contains(item)))
                throw new InvalidDenominationException();

            var totalPaymentProvided = payment.Sum(x => x.Denomination * x.Quantity);
            if (totalPaymentProvided < totalPrice)
                throw new PaymentNotEnoughToCoverTotalPrice();

            //using extension method to round up total price
            var change = totalPaymentProvided - totalPrice.RoundUpValue();

            var currency = _currency.Denominations
                .Where(x => x <= change)
                .OrderByDescending(x => x)
                .ToList();

            var customerChange = new List<Change>();

            //loop from the highest to the lowest denomination
            while (change > 0)
            {
                var _currency = currency.Where(w => w <= change);
                var firstCurrency = _currency.First();

                if (_currency.Count() == 0)
                    throw new InvalidChangeDenominationException();

                change = Math.Round(change - firstCurrency, 2);

                customerChange.Add(new Change(firstCurrency, 1));
            }

            return customerChange
                .GroupBy(g => g.Denomination)
                .Select(x => new Change (x.Key, x.Sum(s => s.Quantity)));
        }
    }
}
