using System;

namespace GlobalBlue.CustomerManager.Application.Exceptions
{
    public sealed class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(int id) : base($"Customer with ID:{id} doesn't exist")
        {

        }
    }
}
