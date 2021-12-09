using GlobalBlue.CustomerManager.Application.Exceptions;

namespace GlobalBlue.CustomerManager.WebApi.ProblemDetails
{
    internal sealed class CustomerConflicProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public CustomerConflicProblemDetails(Microsoft.AspNetCore.Mvc.ProblemDetails details, CustomerConflictException exception)
        {
            Message = exception.Message;
            Status = details.Status;
            Instance = details.Instance;
            Title = details.Title;
            Detail = details.Detail;
        }

        public string Message { get; }
    }
}
