using System;

namespace GlobalBlue.CustomerManager.Application.Exceptions
{
    public class CustomerConflictException : Exception
    {
        public CustomerConflictException(string message) : base(message)
        {

        }
    }
}
