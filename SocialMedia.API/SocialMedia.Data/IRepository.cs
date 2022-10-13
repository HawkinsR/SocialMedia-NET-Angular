using SocialMedia.Models;

namespace SocialMedia.Data
{
    public interface IRepository
    {
        public Task<int>CreateNewUserAndReturnUserIdAsync(UserDTO newUser);

        public Task<User> GetUserLoginAsync(string password, string email);

        public Task<IEnumerable<Post>> GetAllPostsAsync();

        public Task<Post> GetPostById(int id);

        public Task<Post> CreateNewPostAndReturnPost(PostDTO newPost);
    }
}
