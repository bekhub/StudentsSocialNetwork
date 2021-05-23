using System.Collections.Generic;
using System.Threading.Tasks;
using Core.TimetableApiModels;

namespace Core.Interfaces.Services
{
    public interface ITimetableApiService
    {
        Task<List<Departments.Response>> DepartmentsAsync();

        Task<List<Faculties.Response>> FacultiesAsync();

        Task<Timetable.Response> TimetableAsync(string lessonCode);
    }
}
