using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sell_Console_System.Contracts
{
    internal interface IIdentifier
    {
        public string Table { get; }
        public string Id { get; }
    }
}
