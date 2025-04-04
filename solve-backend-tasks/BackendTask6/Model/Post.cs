﻿namespace BackendTask6.Model
{
    public class Post
    {
        public int id { get; set; }
        public string title { get; set; }
        public string? body { get; set; }
        public bool approved { get; set; }
        public int userId { get; set; } 
        public User? user { get; set; }
    }
}
