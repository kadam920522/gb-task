using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Entites
{
    public sealed class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }
}
