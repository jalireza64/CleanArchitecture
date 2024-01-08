using Sepid.Robot.Domain.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Data.Access.Exceptions
{
    public class FetchDataException : BaseException
    {
        public FetchDataException(string message) : base(message)
        {
            SetStatusCode(System.Net.HttpStatusCode.NotFound);
        }

        public FetchDataException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
