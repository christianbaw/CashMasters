namespace CashMasters.Entities
{
    public class Payment : Money
    {
        public Payment(double denomination, int quantity) : base(denomination, quantity)
        {
        }
    }
}
