using System.Linq;
using System.Collections.Generic;

namespace CCLibrary.Products
{
    public class Dish : Product
    {
        private List<Consumable> _ingredients;

        protected Dish()
        {
            _ingredients = new List<Consumable>();
        }

        protected Dish(IEnumerable<Consumable> ingredients)
        {
            _ingredients = new List<Consumable>(ingredients);
        }

        public Dish(string name, IEnumerable<Consumable>? ingredients = null) : base(name)
        {
            if (ingredients is null)
                _ingredients = new List<Consumable>();
            else
                _ingredients = ingredients.ToList();
        }

        public override Product Copy()
        {
            return new Dish(this._ingredients)
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                NetMassInGrams = this.NetMassInGrams,
            };
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
        public void UpdateIngredients(IEnumerable<Consumable> ingredients)
        {
            _ingredients = new List<Consumable>(ingredients);
        }

        public override double GetCalories()
        {
            return _ingredients.Select(product => product.GetCalories()).Sum();
        }
    }
}
