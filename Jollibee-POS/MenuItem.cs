using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jollibee_POS
{
    internal class MenuItem
    {
        public string Name { get; set; }
        public decimal Price { get ; set; }
        public int Quantity { get; set; }

        public decimal Total => Price * Quantity;
    }
}
