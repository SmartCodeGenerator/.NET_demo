using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using System.Collections.Generic;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class UserProfileRepository : IRepository<UserProfile, string>
    {
        private readonly ApplicationContext context;

        public UserProfileRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public IEnumerable<UserProfile> GetAll() => context.Users;

        public UserProfile Get(string id) => context.Users.Find(id);

        public void Create(UserProfile userProfile) => context.Users.Add(userProfile);

        public void Update(UserProfile userProfile) => context.Users.Update(userProfile);

        public void Delete(string id)
        {
            var userProfileToRemove = Get(id);
            if (userProfileToRemove != null)
            {
                context.Users.Remove(userProfileToRemove);
            }
        }
    }
}
