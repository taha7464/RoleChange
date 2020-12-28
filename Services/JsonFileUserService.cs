using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace RoleChange.Services
{
    public class JsonFileUserService
    {
        public string rootFolder { get; set; }
        public JsonFileUserService(IWebHostEnvironment env)
        {
            rootFolder = env.ContentRootPath;
        }

        private string JsonFileName
        {
            get { return rootFolder + "/data/users.json"; }
        }

        public async Task< IEnumerable<User>> GetUsers()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {

                return await Task.Run (()=>System.Text.Json.JsonSerializer.Deserialize<User[]>( jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }));
            }
        }

        public async Task<IEnumerable<User>> EditUser(string id,string role)
        {
            var users = await GetUsers();
            var query = users.First(x => x.id == id);
            query.role = role;
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(JsonFileName, json);
            return await GetUsers();
           
        }
    }
}
