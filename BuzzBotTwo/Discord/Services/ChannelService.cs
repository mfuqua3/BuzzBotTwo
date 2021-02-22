using System.Threading.Tasks;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.Repository;
using Discord;
using Discord.Commands;

namespace BuzzBotTwo.Discord.Services
{
    public interface IChannelService
    {
        Task<MessageChannel> AddOrGetChannel(ICommandContext context, bool saveContext = false);
    }

    public class ChannelService : IChannelService
    {
        private readonly IMessageChannelRepository _repository;

        public ChannelService(IMessageChannelRepository repository)
        {
            _repository = repository;
        }
        public async Task<MessageChannel> AddOrGetChannel(ICommandContext context, bool saveContext = false)
        {
            var existing = await _repository.FindAsync(context.Channel.Id);
            if (existing != null) return existing;
            var channel = new MessageChannel()
            {
                Id = context.Channel.Id,
                ServerId = context.Guild.Id
            };
            await _repository.PostAsync(channel);
            if (saveContext)
                await _repository.SaveAllChangesAsync();
            return channel;
        }
    }
}