using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;
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
                .AddScoped<ISoftResRaidTemplateRepository, SoftResRaidRaidTemplateRepository>()
                .AddScoped<IPaginatedMessageRepository, PaginatedMessageRepository>()
                .AddScoped<IMessageChannelRepository, MessageChannelRepository>()
                .AddScoped<ISoftResRaidMonitorRepository, SoftResRaidMonitorRepository>();
            return services;
        }
    }

    public interface IMessageChannelRepository:IRepository<MessageChannel, ulong> { }

    public class MessageChannelRepository : Repository<MessageChannel, ulong>, IMessageChannelRepository
    {
        public MessageChannelRepository(BotContext db) : base(db)
        {
        }
    }
}