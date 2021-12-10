namespace GlobalBlue.CustomerManager.Application.Exceptions
{
    public sealed class CustomerEmailAddressConflictException : CustomerConflictException
    {
        public CustomerEmailAddressConflictException(string conflictingEmailAddress) : 
            base($"Customer already exists with the following e-mail address: {conflictingEmailAddress}")
        {
        }
    }
}
