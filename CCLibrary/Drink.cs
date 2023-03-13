namespace CCLibrary
{
    internal class Drink : Consumable
    {
        public bool IsCarbonated { get; set; } = false;

        public Drink(ulong id, double caloriesPerServing) : base(id, caloriesPerServing)
        {
        }

        public Drink(ulong id, string name, double caloriesPerServing) : base(id, name, caloriesPerServing)
        {
        }
    }
}
