using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BeerTap.Model.Exceptions
{
    public class BeerTapServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public BeerTapServiceException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
