using WebApplication1.Model;

namespace WebApplication1.Service.Student;

public interface IStudentService
{
    Task<Table.Student> AddStudent(Model.RigModl m);
    Task<Table.Student> GetStudent(int id);
    void setsessionvalue(Table.Student student);
    public int? GetCurrentLoggedIn();
    public void logout();
}