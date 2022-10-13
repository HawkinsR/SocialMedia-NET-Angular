using Microsoft.Extensions.Logging;
using SocialMedia.Models;
using System.Data.SqlClient;

namespace SocialMedia.Data
{
    public class SqlRepository : IRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<SqlRepository> _logger;

        public SqlRepository(string connectionString, ILogger<SqlRepository> logger)
        {
            this._connectionString = connectionString;
            this._logger = logger;
        }

        public async Task<int> CreateNewUserAndReturnUserIdAsync(UserDTO newUser)
        {
            int UserId = 0;

            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            string cmdText = "INSERT INTO [smd].[Users] ([UserFirstName], [UserLastName], [UserEmail], [UserPassword])" +
                             @"VALUES (@UFN, @ULN, @UEM, @UPW);" +
                             "SELECT [UserId]" +
                             "FROM [smd].[Users]" +
                             @"WHERE [UserEmail] = @UEM;";

            using SqlCommand cmd = new(cmdText, connection);

            cmd.Parameters.AddWithValue("@UFN", newUser.firstName);
            cmd.Parameters.AddWithValue("@ULN", newUser.lastName);
            cmd.Parameters.AddWithValue("@UEM", newUser.email);
            cmd.Parameters.AddWithValue("@UPW", newUser.password);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
            {
                UserId = reader.GetInt32(0);
            }

            await connection.CloseAsync();

            _logger.LogInformation($"Executed CreateNewUserAsync");

            return UserId;
        }

        public async  Task<User> GetUserLoginAsync(string password, string email)
        {
            User user = new User();

            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            string cmdText = @"SELECT [UserId], [UserFirstName], [UserLastName], [UserEmail]" +
                             "FROM [smd].[Users]" +
                             $"WHERE [UserPassword] = @PW AND [UserEmail] = @EM;";

            using SqlCommand cmd = new(cmdText, connection);

            cmd.Parameters.AddWithValue("@PW", password);
            cmd.Parameters.AddWithValue("@EM", email);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                user.id = reader.GetInt32(0);
                user.firstName = reader.GetString(1);
                user.lastName = reader.GetString(2);
                user.email = reader.GetString(3);
            }

            await connection.CloseAsync();

            _logger.LogInformation($"Executed GetUsersAsync");

            return user;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            _logger.LogInformation($"Executing GetAllPostsAsync");

            var posts = new List<Post>();

            using SqlConnection connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            string cmdText = @"SELECT [PostId], [PostText], [PostImageUrl], [PostReference], [UserId], [UserFirstName], [UserLastName], [UserEmail]
                                FROM [smd].[Posts]
                                JOIN [smd].[Users] 
                                    ON [smd].[Posts].[PostAuthorId] = [smd].[Users].[UserId]
                                WHERE [PostReference] IS NULL;";

            using SqlCommand cmd = new SqlCommand(cmdText, connection);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            int? postReference;

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string text = reader.GetString(1);
                string? imageUrl = reader.IsDBNull(2) ? null : reader.GetString(2);
                postReference = reader.IsDBNull(3) ? null : reader.GetInt32(3);
                int authorid = reader.GetInt32(4);
                string authorfirstName = reader.GetString(5);
                string authorlastName = reader.GetString(6);
                string authoremail = reader.GetString(7);

                var tmp = await GetPostsByPostReference(id);

                Post[] comments = tmp.ToArray();

                posts.Add(new Post(id, postReference, text, imageUrl, new User(authorid, authorfirstName, authorlastName, authoremail), comments));
            }

            await connection.CloseAsync();

            _logger.LogInformation("Executed GetAllPostsAsync");

            return posts;
        }

        private async Task<IEnumerable<Post>> GetPostsByPostReference(int reference)
        {
            _logger.LogInformation($"Executing GetPostByPostReference for {reference}");

            var posts = new List<Post>();

            using SqlConnection connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            string cmdText = @"SELECT [PostId], [PostText], [PostImageUrl], [PostReference], [UserId], [UserFirstName], [UserLastName], [UserEmail]
                                FROM [smd].[Posts]
                                JOIN [smd].[Users] 
                                    ON [smd].[Posts].[PostAuthorId] = [smd].[Users].[UserId]
                                WHERE [PostReference] = @REF;";

            using SqlCommand cmd = new SqlCommand(cmdText, connection);

            cmd.Parameters.AddWithValue("@REF", reference);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            int? postReference;

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string text = reader.GetString(1);
                string? imageUrl = reader.IsDBNull(2) ? null : reader.GetString(2);
                postReference = reader.IsDBNull(3) ? null : reader.GetInt32(3);
                int authorid = reader.GetInt32(4);
                string authorfirstName = reader.GetString(5);
                string authorlastName = reader.GetString(6);
                string authoremail = reader.GetString(7);

                var tmp = await GetPostsByPostReference(id);

                Post[] comments = tmp.ToArray();

                posts.Add(new Post(id, postReference, text, imageUrl, new User(authorid, authorfirstName, authorlastName, authoremail), comments));
            }

            await connection.CloseAsync();

            _logger.LogInformation("Executed GetPostByPostReference");

            return posts;
        }

        public async Task<Post> CreateNewPostAndReturnPost(PostDTO newPost)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            string cmdText = @" INSERT INTO [smd].[Posts] ([PostText], [PostImageUrl], [PostAuthorId], [PostReference])
                                VALUES (@PT, @PI, @PA, @PR);
                                
                                SELECT [PostId], [PostText], [PostImageUrl], [PostReference], [UserId], [UserFirstName], [UserLastName], [UserEmail]
                                FROM [smd].[Posts]
                                JOIN [smd].[Users] 
                                    ON [smd].[Posts].[PostAuthorId] = [smd].[Users].[UserId]
                                WHERE [PostText] = @PT AND [PostImageUrl] = @PI AND [PostAuthorId] = @PA;";

            using SqlCommand cmd = new SqlCommand(cmdText, connection);

            cmd.Parameters.AddWithValue("@PT", newPost.text);
            cmd.Parameters.AddWithValue("@PI", newPost.imageUrl);
            cmd.Parameters.AddWithValue("@PA", newPost.author.id);
            cmd.Parameters.AddWithValue("@PR", newPost.postReference);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            Post tmp = new Post();
            int postref = 0;

            while (reader.Read())
            {
                tmp.id = reader.GetInt32(0) ;
                tmp.text = reader.GetString(1);
                tmp.imageUrl = reader.GetString(2);
                postref = reader.GetInt32(3);
                tmp.author.id = reader.GetInt32(4);
                tmp.author.firstName = reader.GetString(5);
                tmp.author.lastName = reader.GetString(6);
                tmp.author.email = reader.GetString(7);
            }

            await connection.CloseAsync();


            _logger.LogInformation("Executed CreateNewPostAndReturnIdAsync");

            return tmp;

        }

        public async Task<Post> GetPostById(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            await connection.OpenAsync();

            string cmdText = @"SELECT [PostText], [PostImageUrl], [PostReference], [UserId], [UserFirstName], [UserLastName], [UserEmail]
                                FROM [smd].[Posts]
                                JOIN [smd].[Users] 
                                    ON [smd].[Posts].[PostAuthorId] = [smd].[Users].[UserId]
                                WHERE [PostId] = @ID;";

            using SqlCommand cmd = new SqlCommand(cmdText, connection);

            cmd.Parameters.AddWithValue("@ID", id);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            Post tmp = new Post();
            int postReference;

            while (reader.Read())
            {
                tmp.id = id;
                tmp.text = reader.GetString(1);
                tmp.imageUrl = reader.GetString(2);
                postReference = reader.GetInt32(3);
                tmp.author.id = reader.GetInt32(4);
                tmp.author.firstName = reader.GetString(5);
                tmp.author.lastName = reader.GetString(6);
                tmp.author.email = reader.GetString(7);
            }

            await connection.CloseAsync();

            _logger.LogInformation("Executed GetProductByIdAsync");

            return tmp;
        }

    }
}