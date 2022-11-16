namespace CashMasters.Entities
{
    public class Change : Money
    {
        public Change(double denomination, int quantity) : base(denomination, quantity)
        {
        }
    }
}
