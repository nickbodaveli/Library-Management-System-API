using BuildingBlocks.Behaviors;
using Library.API.Data;
using Library.Application.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Library.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices
            (this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            //services.AddScoped<IBookRepository, BookRepository>();
            //services.AddScoped<IBookReadRepository, BookReadRepository>();
            //services.AddScoped<IBookReadRepository, CachedBookReadRepository>();
            //services.AddScoped<ILoanRepository, LoanRepository>();
            //services.AddScoped<ILoanReadRepository, LoanReadRepository>();
            //services.AddScoped<ILoanReadRepository, CachedLoanReadRepository>();
            //services.AddScoped<IMemberRepository, MemberRepository>();
            //services.AddScoped<IMemberReadRepository, MemberReadRepository>();
            //services.AddScoped<IMemberReadRepository, CachedMemberReadRepository>();

            return services;
        }
    }
}
