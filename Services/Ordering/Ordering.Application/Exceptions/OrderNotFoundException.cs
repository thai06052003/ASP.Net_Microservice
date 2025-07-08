using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
    public class OrderNotFoundException : ApplicationException
    {
        public OrderNotFoundException(string name, Object key) : base($"Entity {name} - {key} is not found.")
        {
            
        }
    }
}
