using Sepid.Robot.Domain.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Data.Access.Exceptions
{
    public class DeleteDataException : BaseException
    {
        public DeleteDataException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
