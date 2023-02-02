
using ToDoList.Models;
namespace ToDoList.Interfaces
{
    public interface ITaskService
    {
        List<MyTask>? GetAll(String token);
        MyTask Get(int id);
        void Add(String token, MyTask task);
        void Delete(int id);
        void Update(MyTask task);
        int Count { get; }
    }
}