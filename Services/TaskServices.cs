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
    public class TaskService : ITaskService
    {
        List<MyTask>? tasks { get; }
        private IWebHostEnvironment webHost;
        private string filePath;
        public TaskService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                tasks = JsonSerializer.Deserialize<List<MyTask>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(tasks));
        }

        public List<MyTask> GetAll(string token)
        {
            String userIdFromToken = TokenService.Decode(token);
            return tasks.Where(task => task.UserId == int.Parse(userIdFromToken))?.ToList();
        }

        public MyTask Get(int id) => tasks.FirstOrDefault(t => t.Id == id);

        public void Add(String token, MyTask task)
        {
            MyTask lastElement = tasks.LastOrDefault();
            task.Id = lastElement.Id + 1;
            task.UserId = int.Parse(TokenService.Decode(token));
            tasks.Add(task);
            saveToFile();
        }

        public void Delete(int id)
        {
            var task = Get(id);
            if (task is null)
                return;

            tasks.Remove(task);
            saveToFile();
        }

        public void Update(MyTask task)
        {
            var index = tasks.FindIndex(t => t.Id == task.Id);
            if (index == -1)
                return;
            tasks[index].Name = task.Name;
            tasks[index].IsDone = task.IsDone;
            saveToFile();
        }
        public int Count => tasks.Count();
    }

}
