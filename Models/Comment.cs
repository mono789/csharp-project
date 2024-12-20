namespace PruebaTecnica2.Models
{
    public class Comment
    {
        public int postId { get; set; }
        public int userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string body { get; set; }
    }
}
