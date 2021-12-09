using FluentValidation;
using GlobalBlue.CustomerManager.Application.Exceptions;
using GlobalBlue.CustomerManager.WebApi.ProblemDetails;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace GlobalBlue.CustomerManager.WebApi.Extensions
{
    internal static class ProblemDetailsExtensions
    {
        public static IServiceCollection AddHellangProblemDetails(this IServiceCollection services, IWebHostEnvironment environment)
        {
            return services.AddProblemDetails(options => ConfigureProblemDetails(options, environment));
        }

        private static void ConfigureProblemDetails(ProblemDetailsOptions options, IWebHostEnvironment environment)
        {
            // Only include exception details in a development environment. There's really no nee
            // to set this as it's the default behavior. It's just included here for completeness :)
            options.IncludeExceptionDetails = (ctx, ex) => environment.IsDevelopment();

            // Custom mapping function for FluentValidation's ValidationException.
            options.MapFluentValidationException();

            options.MapCustomerConflictException();

            options.MapToStatusCode<CustomerNotFoundException>(StatusCodes.Status404NotFound);

            // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
            // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        }

        private static void MapCustomerConflictException(this ProblemDetailsOptions options) =>
            options.Map<CustomerConflictException>((ctx, ex) =>
            {
                var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                var details = factory.CreateProblemDetails(ctx, StatusCodes.Status409Conflict);
                return new CustomerConflicProblemDetails(details, ex);
            });

        private static void MapFluentValidationException(this ProblemDetailsOptions options) =>
            options.Map<ValidationException>((ctx, ex) =>
            {
                var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                var errors = ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(x => x.ErrorMessage).ToArray());

                return factory.CreateValidationProblemDetails(ctx, errors);
                
            });
    }
}
