using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sell_Console_System.Contracts;

namespace Sell_Console_System.Entities
{
    internal class Product : IIdentifier
    {
        public string Table { get; }
        public string Id { get; }
        public Product()
        {
            Table = "product";
            Id = "id";
        }
    }
}
