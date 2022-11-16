using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMasters.Entities
{
    public abstract class Money
    {
        public Money(double denomination, int quantity)
        {
            Denomination = denomination;
            Quantity = quantity;
        }

        public double Denomination { get; set; }
        public int Quantity { get; set; }
    }
}
