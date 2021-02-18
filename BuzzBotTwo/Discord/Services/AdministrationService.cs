﻿using System.Collections.Generic;
using System.Linq;
using Discord;
using Microsoft.Extensions.Configuration;

namespace BuzzBotTwo.Discord.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly IConfiguration _configuration;
        private readonly HashSet<ulong> _adminUsers;

        public AdministrationService(IConfiguration configuration)
        {
            _configuration = configuration;
            _adminUsers = configuration.GetSection("adminUsers").AsEnumerable()
                .Where(c => ulong.TryParse((string?) c.Value, out _)).Select(c => ulong.Parse(c.Value)).ToHashSet();
        }

        public bool IsUserAdmin(IUser user)
            => IsUserAdmin(user.Id);

        public void Authorize(IUser user)
        {
            _adminUsers.Add(user.Id);
        }

        public bool IsUserAdmin(ulong requestingUser)
        {
            return _adminUsers.Contains(requestingUser);
        }
    }
}