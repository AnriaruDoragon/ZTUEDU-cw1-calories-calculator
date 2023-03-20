namespace CCLibrary.Products
{
    public class Drink : Consumable
    {
        protected Drink()
        {
        }

        public Drink(string name, double caloriesPerServing, DrinkTypes drinkType = DrinkTypes.Tap)
            : base(name, caloriesPerServing)
        {
            Type = drinkType;
        }

        protected DrinkTypes _drinkType;
        public bool IsCarbonated { get; set; }

        public DrinkTypes Type
        {
            get => _drinkType;
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

        public override Product Copy()
        {
            return new Drink
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                NetMassInGrams = this.NetMassInGrams,
                CaloriesPerServing = this.CaloriesPerServing,
                ServingSizeInGrams = this.ServingSizeInGrams,
                Type = this.Type,
                IsCarbonated = this.IsCarbonated
            };
        }
    }
}
