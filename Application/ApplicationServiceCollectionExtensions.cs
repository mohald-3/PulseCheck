using Application.Mapping;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddValidatorsFromAssemblyContaining<UserCreateDtoValidator>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFriendshipService, FriendshipService>();
            services.AddScoped<ICheckInService, CheckInService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


            return services;
        }
    }
}
