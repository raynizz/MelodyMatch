using System;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Chats;
using MelodyMatch.Matches.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;

namespace MelodyMatch.Matches.Handlers;

public class MutualLikeCreatedHandler :
    ILocalEventHandler<MutualLikeCreatedEvent>,
    ITransientDependency
{
    private readonly IRepository<Chat, Guid> _chatRepository;
    private readonly IRepository<Message, Guid> _messageRepository;

    public MutualLikeCreatedHandler(
        IRepository<Chat, Guid> chatRepository,
        IRepository<Message, Guid> messageRepository)
    {
        _chatRepository = chatRepository;
        _messageRepository = messageRepository;
    }

    public async Task HandleEventAsync(MutualLikeCreatedEvent eventData)
    {
        var userAId = eventData.UserAId;
        var userBId = eventData.UserBId;

        var chatExists = await _chatRepository.AnyAsync(c =>
            c.Participants.Any(p => p.UserId == userAId) &&
            c.Participants.Any(p => p.UserId == userBId));

        if (chatExists)
        {
            return;
        }

        var chat = new Chat(new() { userAId, userBId });

        await _chatRepository.InsertAsync(chat, autoSave: true);

        if (!string.IsNullOrWhiteSpace(eventData.FirstMessage) &&
            eventData.FirstMessageSenderId.HasValue)
        {
            var senderId = eventData.FirstMessageSenderId.Value;

            var message = new Message(
                senderId: senderId,
                content: eventData.FirstMessage!,
                isRead: false,
                chatId: chat.Id
            );

            await _messageRepository.InsertAsync(message, autoSave: true);
        }
    }
}