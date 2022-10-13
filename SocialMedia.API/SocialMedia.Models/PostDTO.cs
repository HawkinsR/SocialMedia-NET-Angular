namespace SocialMedia.Models
{
    public class PostDTO
    {
        public int id { get; set; }
        public int? postReference { get; set; }
        public string text { get; set; }
        public string? imageUrl { get; set; }
        public User author { get; set; }

        public PostDTO() { }
        public PostDTO(int id, int? postReference, string text, string? imageUrl, User author)
        {
            this.id = id;
            this.postReference = postReference;
            this.text = text;
            this.imageUrl = imageUrl;
            this.author = author;
        }
    }
}
