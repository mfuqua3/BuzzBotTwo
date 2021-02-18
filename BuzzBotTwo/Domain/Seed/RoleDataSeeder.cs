using System;
using System.Collections.Generic;

namespace BuzzBotTwo.Domain.Seed
{
    public class RoleDataSeeder : IDataSeeder<BotRole>
    {
        public IEnumerable<BotRole> Data => GetRoles();

        private IEnumerable<BotRole> GetRoles()
        {
            var roleEnumFields = Enum.GetValues(typeof(BotRoleLevel));
            foreach (BotRoleLevel enumField in roleEnumFields)
            {
                yield return new BotRole
                {
                    Id = (int)enumField,
                    Title = enumField.ToString()
                };
            }
        }
    }
}