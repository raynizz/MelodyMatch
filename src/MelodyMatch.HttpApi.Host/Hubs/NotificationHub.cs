using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMatch.Contexts.MelodyMatchUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.DependencyInjection;

namespace MelodyMatch.Hubs;

[Authorize]
public class NotificationHub : Hub, ITransientDependency
{
    private readonly ICurrentMelodyMatchUser _currentMelodyMatchUser;
    
    private static readonly Dictionary<Guid, List<string>> UserConnections = new();
    private static readonly object Lock = new();

    public NotificationHub(ICurrentMelodyMatchUser currentMelodyMatchUser)
    {
        _currentMelodyMatchUser = currentMelodyMatchUser;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = await _currentMelodyMatchUser.GetIdAsync();
        
        lock (Lock)
        {
            if (!UserConnections.ContainsKey(userId))
            {
                UserConnections[userId] = new List<string>();
            }
            UserConnections[userId].Add(Context.ConnectionId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = await _currentMelodyMatchUser.GetIdAsync();
        
        lock (Lock)
        {
            if (UserConnections.ContainsKey(userId))
            {
                UserConnections[userId].Remove(Context.ConnectionId);
                if (UserConnections[userId].Count == 0)
                {
                    UserConnections.Remove(userId);
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
    
    public static List<string> GetConnectionIds(Guid userId)
    {
        lock (Lock)
        {
            return UserConnections.ContainsKey(userId) 
                ? new List<string>(UserConnections[userId]) 
                : new List<string>();
        }
    }
}

