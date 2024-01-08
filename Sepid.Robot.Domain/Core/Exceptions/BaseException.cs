using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepid.Robot.Domain.Core.Exceptions
{
    public class BaseException : System.Exception
    {
        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }

        public BaseException(string message)
            : base(message)
        {
        }

        public void SetStatusCode(System.Net.HttpStatusCode statusCode)
        {
            this.StatusCode = (int)statusCode;
        }

        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public string GetMessage()
        {
            return CleanExceptionMessage(Message);
        }

        public string GetStackMessagesString()
        {
            StringBuilder strBuilder = new StringBuilder();
            Exception ex = this;
            while (ex != null)
            {
                string message = CleanExceptionMessage(ex.Message);
                strBuilder.AppendLine($"{ex.Source} : {message}");
                ex = ex.InnerException;
            }
            return strBuilder.ToString();
        }

        private string CleanExceptionMessage(string message)
        {
            message = message.Replace("\r", "");
            message = message.Replace("\r", "");
            message = message.Trim();
            return message;
        }

        public IEnumerable<string> GetStackMessages()
        {
            Exception ex = this;
            while (ex != null)
            {
                yield return CleanExceptionMessage(ex.Message);
                ex = ex.InnerException;
            }
        }
    }
}
