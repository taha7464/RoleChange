using System;
using System.Text.Json;

namespace RoleChange
{
    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string role { get; set; }
        public override string ToString() => JsonSerializer.Serialize<User>(this);
        
    }
}
