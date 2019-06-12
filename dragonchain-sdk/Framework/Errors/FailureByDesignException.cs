using System;

namespace dragonchain_sdk.Framework.Errors
{
    public class FailureByDesignException : Exception
    {
        public FailureCode Code { get; set; }

        public FailureByDesignException(FailureCode code, string message = "Failure By Design")
            :base(message)
        {
            Code = code;
        }
    }

    public enum FailureCode
    {
        PARAM_ERROR,
        NOT_FOUND
    }
}