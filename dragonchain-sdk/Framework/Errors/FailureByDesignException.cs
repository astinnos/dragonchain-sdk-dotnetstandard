using System;

namespace dragonchain_sdk.Framework.Errors
{
    public class FailureByDesignException : Exception
    {
        public string Code { get; set; }

        public FailureByDesignException(string code = "FAILURE_BY_DESIGN", string message = "Failure By Design")
            :base(message)
        {
            Code = code;
        }
    }
}