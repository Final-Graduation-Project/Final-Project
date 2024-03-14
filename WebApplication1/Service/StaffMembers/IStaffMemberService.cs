namespace WebApplication1.Service.StaffMembers;

public interface IStaffMemberService
{
    Task<Table.Teacher> AddStaffMember(Model.RigModl m);
    Task<Table.Teacher> GetStaffMember(int id);
    void setsessionvalue(Table.Teacher teacher);
    
    
}