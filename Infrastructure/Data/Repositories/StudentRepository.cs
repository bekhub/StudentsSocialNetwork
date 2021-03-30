using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class StudentRepository : EfRepository<Student>, IStudentRepository
    {
        public StudentRepository(SsnDbContext ssnDbContext) : base(ssnDbContext) { }

        public Task<Student> GetByStudentNumberAsync(string studentNumber)
        {
            return SsnDbContext.Set<Student>()
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.StudentNumber == studentNumber);
        }
    }
}
