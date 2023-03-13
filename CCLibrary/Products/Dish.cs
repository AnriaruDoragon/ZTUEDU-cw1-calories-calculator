using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCLibrary.Products
{
    internal class Dish : Product
    {


        public Dish(string name) : base(name)
        {

        }

        public override double GetCalories()
        {
            throw new NotImplementedException();
        }
    }
}
