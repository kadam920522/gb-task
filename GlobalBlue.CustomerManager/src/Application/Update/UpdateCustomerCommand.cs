using MediatR;

namespace GlobalBlue.CustomerManager.Application.Update
{
    public sealed class UpdateCustomerCommand : IRequest
    {
        public UpdateCustomerCommand(string eTag, int customerId, string newFirstName, string newSurname, string newEmailAddress, string newPassword)
        {
            CustomerId = customerId;
            NewFirstName = newFirstName;
            NewSurname = newSurname;
            NewEmailAddress = newEmailAddress;
            NewPassword = newPassword;
            ETag = eTag;
        }

        public int CustomerId { get; }

        public string NewFirstName { get; }

        public string NewSurname { get; }

        public string NewEmailAddress { get; }

        public string NewPassword { get; }

        public string ETag { get; }

        public uint ETagAsXmin => uint.Parse(ETag);
    }
}
