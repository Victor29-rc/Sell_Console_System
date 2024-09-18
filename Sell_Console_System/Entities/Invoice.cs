using Sell_Console_System.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sell_Console_System.Entities
{
    internal class Invoice : IIdentifier
    {
        public string Table { get; }
        public string Id { get; }
        public Invoice()
        {
            Table = "invoice";
            Id = "id";
        }
    }
}
