namespace BackendTask6.Model
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string Password { get; set; }
        public int idRole { get; set; }
        public Role? role { get; set; }
        public ICollection<Post>? posts { get; set; }
    }
}
