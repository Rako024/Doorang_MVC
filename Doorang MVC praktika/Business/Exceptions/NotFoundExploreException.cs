using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Exceptions
{
    public class NotFoundExploreException : Exception
    {
        public string PropertyName { get; set; }
        public NotFoundExploreException(string  porpName,string? message) : base(message)
        {
            PropertyName = porpName;
        }
    }
}
