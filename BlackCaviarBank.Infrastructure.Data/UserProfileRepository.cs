using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Domain.Interfaces;
using System.Collections.Generic;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class UserProfileRepository : IRepository<UserProfile>
    {
        private readonly ApplicationContext context;

        public UserProfileRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public IEnumerable<UserProfile> GetAll() => context.UserProfiles;

        public UserProfile Get(int id) => context.UserProfiles.Find(id);

        public void Create(UserProfile userProfile) => context.UserProfiles.Add(userProfile);

        public void Update(UserProfile userProfile) => context.UserProfiles.Update(userProfile);

        public void Delete(int id)
        {
            var userProfileToRemove = Get(id);
            if (userProfileToRemove != null)
            {
                context.UserProfiles.Remove(userProfileToRemove);
            }
        }
    }
}
