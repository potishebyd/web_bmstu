using Microsoft.Extensions.DependencyInjection;
using web_bmstu.ModelsConverters;

namespace web_bmstu.Utils
{
    public static class ProvideExtension
    {
        public static IServiceCollection AddDtoConverters(this IServiceCollection services)
        {
            services.AddTransient<SongConverters>();
            services.AddTransient<PlaylistConverters>();
            services.AddTransient<ArtistConverters>();
            services.AddTransient<RecordingStudioConverters>();
            services.AddTransient<UserConverters>();

            return services;
        }
    }
}