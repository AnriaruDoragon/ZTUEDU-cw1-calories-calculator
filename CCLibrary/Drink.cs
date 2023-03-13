namespace CCLibrary
{
    public class Drink : Consumable
    {
        protected DrinkTypes _drinkType;
        public bool IsCarbonated { get; set; } = false;

        public DrinkTypes Type
        {
            get { return _drinkType; }
            protected set
            {
                _drinkType = value;
                IsCarbonated = value switch
                {
                    DrinkTypes.Soda or DrinkTypes.Energy => true,
                    _ => false,
                };
            }
        }

        public Drink(ulong id, double caloriesPerServing, DrinkTypes drinkType = DrinkTypes.Tap)
            : base(id, caloriesPerServing)
        {
            Type = drinkType;
        }

        public Drink(ulong id, string name, double caloriesPerServing, DrinkTypes drinkType = DrinkTypes.Tap)
            : base(id, name, caloriesPerServing)
        {
            Type = drinkType;
        }
    }
}
