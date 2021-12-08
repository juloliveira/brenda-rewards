using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sara.Api.Exceptions
{
    public interface IApiException
    {
        int Status { get; }
        string Message { get; }
    }
}
