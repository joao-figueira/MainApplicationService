using MainApplicationService.Entities;

namespace MainApplicationService.Providers
{
    public class CurrentUserProvider
    {
        public User? CurrentUser { get; set; }

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }
    }
}
