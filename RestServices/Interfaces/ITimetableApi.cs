using System.Threading.Tasks;
using Core.TimetableApiModels;
using Refit;

namespace RestServices.Interfaces
{
    public interface ITimetableApi
    {
        [Get("/departments")]
        Task<Departments.Response> Departments();

        [Get("/faculties")]
        Task<Faculties.Response> Faculties();

        [Get("/timetable/{lessonCode}")]
        Task<Timetable.Response> Timetable(string lessonCode);
    }
}
