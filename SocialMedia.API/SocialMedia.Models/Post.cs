using System.Security.Cryptography.X509Certificates;

namespace SocialMedia.Models
{
    public class Post
    {
        public int id { get; set; }
        public int? postReference { get; set; }
        public string text { get; set; }
        public string imageUrl { get; set; }
        public User author { get; set; }
        public Post[] comments { get; set; }

        public Post() { }
        public Post(int id, int? postReference,  string text, string? imageUrl, User author, Post[] comments)
        {
            this.id = id;
            this.postReference = postReference;
            this.text = text;
            this.imageUrl = imageUrl;
            this.author = author;
            this.comments = comments;
        }
    }
}