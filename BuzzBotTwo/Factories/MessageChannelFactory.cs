using System.Collections.Generic;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Factories
{
    public interface IMessageChannelFactory
    {
        MessageChannel CreateNewDM(ulong channelId, ulong userId);
        MessageChannel CreateNewServerMessage(ulong channelId, ulong serverId);
    }

    public class MessageChannelFactory : IMessageChannelFactory
    {
        public MessageChannel CreateNewDM(ulong channelId, ulong userId)
        {
            return new MessageChannel
            {
                Id = channelId,
                UserId = userId,
                PaginatedMessages = new List<PaginatedMessage>()
            };
            
        }

        public MessageChannel CreateNewServerMessage(ulong channelId, ulong serverId)
        {
            return new MessageChannel
            {
                Id = channelId,
                ServerId = serverId,
                PaginatedMessages = new List<PaginatedMessage>()
            };

        }
    }
}