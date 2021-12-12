using System;

namespace GlobalBlue.CustomerManager.Application.Exceptions
{
    public sealed class CustomerChangedExcepion : Exception
    {
        public CustomerChangedExcepion()
        {

        }

        public CustomerChangedExcepion(Exception originalException) : base("Customer propably changed concurrently.", originalException)
        {

        }
    }
}
