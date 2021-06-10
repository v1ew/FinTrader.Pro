namespace FinTrader.Pro.Bonds.Selector
{
    public class DaysRange
    {
        public int MinValue { get; set; }
        
        public int MaxValue { get; set; }
        
        public BondSetType SetType { get; set; }
	
        public bool ThisRange(int value) {
            return MinValue <= value && MaxValue >= value ? true : false;
        }

    }
}