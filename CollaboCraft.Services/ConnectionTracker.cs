﻿using CollaboCraft.Services.Interfaces;
using System.Collections.Concurrent;

namespace CollaboCraft.Services
{
    public class ConnectionTracker : IConnectionTracker
    {
        private readonly ConcurrentDictionary<string, int> _connectedUsers = new();

        public int ConnectionsCount => _connectedUsers.Count;
        public int ConnectedUsersCount => _connectedUsers.Values.Distinct().Count();

        public void TrackConnection(string connectionId, int userId)
        {
            _connectedUsers.TryAdd(connectionId, userId);
        }

        public void UntrackConnection(string connectionId)
        {
            _connectedUsers.TryRemove(connectionId, out _);
        }

        public bool IsUserConnected(int userId)
        {
            return _connectedUsers.Values.Contains(userId);
        }

        public IEnumerable<int> SelectConnectedUsers(IEnumerable<int> userIds)
        {
            var set = new HashSet<int>(_connectedUsers.Values);

            foreach (var userId in userIds)
            {
                if (set.Remove(userId))
                {
                    yield return userId;
                }
            }
        }

        public IEnumerable<TUser> SelectConnectedUsers<TUser>(IEnumerable<TUser> users, Func<TUser, int> idGetter)
        {
            var set = new HashSet<int>(_connectedUsers.Values);

            foreach (var user in users)
            {
                var userId = idGetter(user);
                if (set.Remove(userId))
                {
                    yield return user;
                }
            }
        }

        public IEnumerable<string> SelectConnectionIds(IEnumerable<int> userIds)
        {
            foreach (var userId in userIds)
            {
                if (_connectedUsers.Values.Contains(userId))
                {
                    yield return _connectedUsers.FirstOrDefault(x => x.Value == userId).Key;
                }
            }
        }
    }
}
