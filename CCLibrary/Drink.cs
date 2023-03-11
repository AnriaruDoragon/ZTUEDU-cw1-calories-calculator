namespace CCLibrary
{
    internal class Drink : Consumable
    {
        public Drink(ulong id, double caloriesPerServing) : base(id, caloriesPerServing)
        {
        }

        public Drink(ulong id, string name, double caloriesPerServing) : base(id, name, caloriesPerServing)
        {
        }
    }
}
