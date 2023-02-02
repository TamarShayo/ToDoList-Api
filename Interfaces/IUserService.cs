using ToDoList.Models;
namespace ToDoList.Interfaces
{
    public interface IUserService
    {
        List<MyUser>? GetAll();
        MyUser Get(int id);
        void Add(MyUser user);
        void Delete(int id);
        int Count { get; }

    }

}