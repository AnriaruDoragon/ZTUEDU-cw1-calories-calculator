using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCLibrary.Products
{
    public class Food : Consumable
    {
        protected FoodTypes _foodType;

        public string JsonNutrition { get; set; } = string.Empty;

        protected Food()
        {
            Nutrition = new Nutrition();
        }

        public Food(string name, double caloriesPerServing, FoodTypes foodType = FoodTypes.Other, Nutrition nutrition = new())
            : base(name, caloriesPerServing)
        {
            _foodType = foodType;
            Nutrition = nutrition;
        }

        public FoodTypes Type
        {
            get => _foodType;
            set => _foodType = value;
        }

        [NotMapped]
        public Nutrition Nutrition
        {
            get => JsonConvert.DeserializeObject<Nutrition>(JsonNutrition);
            set => JsonNutrition = JsonConvert.SerializeObject(value);
        }

        public override Product Copy()
        {
            return new Food
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                NetMassInGrams = this.NetMassInGrams,
                CaloriesPerServing = this.CaloriesPerServing,
                ServingSizeInGrams = this.ServingSizeInGrams,
                Type = this.Type,
                Nutrition = this.Nutrition
            };
        }
    }
}
