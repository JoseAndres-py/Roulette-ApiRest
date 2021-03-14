using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_ApiRest.Exceptions
{
    public class ForbiddenAccessException:Exception
    {
        public ForbiddenAccessException() { }

        public ForbiddenAccessException(string message)
            : base(message) { }

        public ForbiddenAccessException(string message, Exception inner)
            : base(message, inner) { }
    }
}
