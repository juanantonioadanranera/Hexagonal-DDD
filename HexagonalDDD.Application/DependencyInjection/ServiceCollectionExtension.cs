using Microsoft.Extensions.DependencyInjection;
using HexagonalDDD.Applicaion.UseCases.Create_Sample;

namespace HexagonalDDD.Applicaion.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<CreateSampleHandler>();
            return services;
        }
    }
}