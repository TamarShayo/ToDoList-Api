using System.Collections.Generic;
using System.Text.Json;
using ToDoList.Interfaces;
using System.Linq;
using System.IO;
using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using ToDoList.Models;



namespace ToDoList.Services
{
    public class UserService : IUserService
    {
        List<MyUser>? users { get; }
        private IWebHostEnvironment webHost;
        private string filePath;

        public UserService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "User.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                users = JsonSerializer.Deserialize<List<MyUser>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            }
        }


        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(users));
        }

        public List<MyUser> GetAll() => users;

        public MyUser Get(int id) => users.FirstOrDefault(t => t.Id == id);

        public void Add(MyUser user)
        {
            MyUser lastElement = users.LastOrDefault();
            user.Id = lastElement.Id + 1;
            users.Add(user);
            saveToFile();
        }

        public void Delete(int id)
        {
            var user = Get(id);
            if (user is null)
                return;
            users.Remove(user);
            saveToFile();
        }
        public int Count => users.Count();

    }

}
