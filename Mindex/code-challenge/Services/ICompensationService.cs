using challenge.Models;

namespace challenge.Services
{
    public interface ICompensationService
    {
        Compensation GetByEmployeeId(string employeeId);
        Compensation Create(Compensation compensation);
    }
}
