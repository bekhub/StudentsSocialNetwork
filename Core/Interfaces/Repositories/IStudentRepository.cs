using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student> GetByStudentNumberAsync(string studentNumber);
    }
}
