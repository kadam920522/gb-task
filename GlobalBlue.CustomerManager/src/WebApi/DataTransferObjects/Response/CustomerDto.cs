using GlobalBlue.CustomerManager.Application.Entities;

namespace GlobalBlue.CustomerManager.WebApi.DataTransferObjects.Response
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public static CustomerDto MapFrom(Customer customer) => new CustomerDto
        {
            Id = customer.Id,
            EmailAddress = customer.EmailAddress.ToString(),
            FirstName = customer.FirstName,
            Surname = customer.Surname,
            Password = customer.Password
        };
    }
}
