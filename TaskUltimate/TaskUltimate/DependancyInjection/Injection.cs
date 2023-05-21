using System.Drawing;
using System.Windows.Input;
using TaskUltimate.Interfaces;
using TaskUltimate.Services;

namespace TaskUltimate.DependancyInjection
{
    public static class Injection
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<IReservationService, ReservationService>();
            services.AddTransient<ISendMessageService, SendMessageService>();
            services.AddTransient<IUserService, UserService>();
            return services;
        }
    }
}
