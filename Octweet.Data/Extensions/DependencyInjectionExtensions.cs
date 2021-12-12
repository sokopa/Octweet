using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Octweet.Data.Abstractions.Repositories;
using Octweet.Data.Repositories;

namespace Octweet.Data.Extensions
{
    public static class DependencyInjectionExtensions 
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, string connString, string assemblyName)
        {
            services.AddDbContext<OctweetDbContext>(
                        options => options.UseMySql(connString, ServerVersion.AutoDetect(connString), sql => sql.MigrationsAssembly(assemblyName)),
                        ServiceLifetime.Transient);

            services.AddScoped<ITweetRepository, TweetRepository>();
            services.AddScoped<IAnnotationRepository, AnnotationRepository>();
            services.AddScoped<IQueryLogRepository, QueryLogRepository>();

            return services;
        }
    }
}
