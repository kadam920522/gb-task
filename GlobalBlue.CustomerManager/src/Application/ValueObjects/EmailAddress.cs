using System;
using System.Text.RegularExpressions;

namespace GlobalBlue.CustomerManager.Application.ValueObjects
{
    // Get from // Copied from https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects#value-object-implementation-in-c
    public class EmailAddress
    {
        private static readonly Regex _emailRegex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$");

        private readonly string _emailAddress;

        private EmailAddress(string emailAddress) => _emailAddress = emailAddress;

        protected static bool EqualOperator(EmailAddress left, EmailAddress right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (EmailAddress)obj;

            return this.ToString() == other.ToString();
        }

        public override int GetHashCode() => _emailAddress.GetHashCode();

        public override string ToString() => _emailAddress;

        public static bool operator ==(EmailAddress one, EmailAddress two)
        {
            return one?.Equals(two) ?? false;
        }

        public static bool operator !=(EmailAddress one, EmailAddress two)
        {
            return !(one?.Equals(two) ?? false);
        }

        public static implicit operator EmailAddress(string emailAddress)
        {
            Validate(emailAddress);
            return new EmailAddress(emailAddress);
        }

        private static void Validate(string emailAddress)
        {
            if (emailAddress is null) throw new ArgumentNullException(nameof(emailAddress));
            if (emailAddress == string.Empty || !IsValid(emailAddress)) throw new ArgumentException($"Invalid email address: {emailAddress}");
        }

        public static bool IsValid(string emailAddress) => _emailRegex.IsMatch(emailAddress);
    }
}
