using System.Linq;
using System.Collections.Generic;

namespace CCLibrary.Products
{
    public class Dish : Product
    {
        private List<Consumable> _ingredients;

        public Dish(string name, List<Consumable>? ingredients = null) : base(name)
        {
            if (ingredients == null)
                _ingredients = new List<Consumable>();
            else
                _ingredients = ingredients;
        }

        /// <summary>
        /// Get copy of the ingredients list.
        /// </summary>
        public List<Consumable> GetIngredients()
        {
            return new List<Consumable>(_ingredients);
        }

        /// <summary>
        /// Add an ingredient to the dish.
        /// </summary>
        public void AddIngredient(Consumable consumable)
        {
            _ingredients.Add(consumable);
        }

        /// <summary>
        /// Update the ingredients list of the dish.
        /// </summary>
        public void UpdateIngredients(List<Consumable> ingredients)
        {
            _ingredients = new List<Consumable>(ingredients);
        }

        public override double GetCalories()
        {
            return _ingredients.Select(product => product.GetCalories()).Sum();
        }
    }
}
