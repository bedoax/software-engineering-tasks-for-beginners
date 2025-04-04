namespace BackendTask6.Model
{
    public class Role
    {
        public int id { get; set; }
        public string name { get; set; }
        public ICollection<User>? users { get; set; }
    }
}
