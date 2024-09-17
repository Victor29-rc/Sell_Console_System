using Sell_Console_System.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sell_Console_System.Models
{
    internal class Response : IResponse
    {
        private readonly string _errorMessage = String.Empty;
        private readonly object _result = new {};
        private readonly List<object> _results = new List<object>();

        public bool HasError { get; }
        public string ErrorMessage { get => _errorMessage; }
        public object Result { get => _result; }
        public List<object> Results { get => _results; }

        public Response(bool hasError) 
        {
            HasError = hasError;
        }

        public Response(bool hasError, string errorMessage)
        {
            HasError = hasError;
            _errorMessage = errorMessage;
        }
        public Response(bool hasError, List<object> results)
        {
            HasError = hasError;
            _results = results;
        }
    }
}
