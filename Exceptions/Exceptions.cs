using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Exceptions
{
    public class DuplicateEmailException : Exception
    {
        public DuplicateEmailException() { }

        public DuplicateEmailException(string message)
            : base(message) { }

        public DuplicateEmailException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class DataAccessException : Exception
    {
        public DataAccessException() { }

        public DataAccessException(string message)
            : base(message) { }

        public DataAccessException(string message, Exception inner)
            : base(message, inner) { }
    }
}