using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class StudentRepository : EfRepository<Student>, IStudentRepository
    {
        public StudentRepository(SsnDbContext ssnDbContext) : base(ssnDbContext) { }

        public Task<Student> GetByStudentNumberAsync(string studentNumber)
        {
            return SsnDbContext.Students
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.StudentNumber == studentNumber);
        }
    }
}
