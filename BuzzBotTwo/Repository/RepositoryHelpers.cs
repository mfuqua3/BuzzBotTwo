using Microsoft.Extensions.DependencyInjection;

namespace BuzzBotTwo.Repository
{
    public static class RepositoryHelpers
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped<IBotRoleRepository, BotRoleRepository>()
                .AddScoped<IItemRepository, ItemRepository>()
                .AddScoped<IRaidRepository, RaidRepository>()
                .AddScoped<IRaidItemRepository, RaidItemRepository>()
                .AddScoped<IRaidLockoutRepository, RaidLockoutRepository>()
                .AddScoped<IRaidParticipantRepository, RaidParticipantRepository>()
                .AddScoped<IServerRepository, ServerRepository>()
                .AddScoped<IServerBotRoleRepository, ServerBotRoleRepository>()
                .AddScoped<IServerUserRepository, ServerUserRepository>()
                .AddScoped<ISoftResEventRepository, SoftResEventRepository>()
                .AddScoped<ISoftResUserRepository, SoftResUserRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IRecurringRaidTemplateRepository, RecurringRaidTemplateRepository>()
                .AddScoped<ISoftResRaidTemplateRepository, SoftResRaidRaidTemplateRepository>();
            return services;
        }
    }
}