using System;

namespace Application.Exceptions
{
    public sealed class CustomerConflictException : Exception
    {
        public CustomerConflictException(string message) : base(message)
        {

        }
    }
}
