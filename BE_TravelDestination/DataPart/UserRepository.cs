namespace BE_TravelDestination.DataPart
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public UserRepository()
        {
            
            _users.Add(new User { Username = "admin", HashedPassword = BCrypt.Net.BCrypt.HashPassword("adminPassword") });
            _users.Add(new User { Username = "user", HashedPassword = BCrypt.Net.BCrypt.HashPassword("userPassword") });
        }

        public User FindByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }
    }

}
