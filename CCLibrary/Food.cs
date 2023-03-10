using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCLibrary
{
    internal class Food : Consumable
    {
        public Food(ulong id, double caloriesPerServing) : base(id, caloriesPerServing)
        {
        }

        public Food(ulong id, string name, double caloriesPerServing) : base(id, name, caloriesPerServing)
        {
        }
    }
}
