using System.Net.Http;
using System.Threading.Tasks;
using Core.Interfaces.Services;
using RestServices.Services;
using Xunit;

namespace UnitTests.RestServices.Services
{
    public class TimetableApiServiceTest
    {
        private readonly ITimetableApiService _timetableApiService;

        public TimetableApiServiceTest()
        {
            _timetableApiService = new TimetableApiService(new HttpClient());
        }

        [Fact]
        public async Task ReturnsDepartments()
        {
            var response = await _timetableApiService.DepartmentsAsync();
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.NotNull(response[0].Name);
            Assert.NotEmpty(response[0].Name);
        }

        [Fact]
        public async Task ReturnsFaculties()
        {
            var response = await _timetableApiService.FacultiesAsync();
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.NotNull(response[0].Name);
            Assert.NotEmpty(response[0].Name);
        }

        [Fact]
        public async Task ReturnsTimetable()
        {
            const string code = "bil-407";
            var response = await _timetableApiService.TimetableAsync(code);
            Assert.NotNull(response);
            Assert.NotNull(response.Faculty);
            Assert.NotEmpty(response.Faculty.Name);
            Assert.NotNull(response.Department);
            Assert.NotEmpty(response.Department.Name);
            Assert.NotNull(response.Timetable);
            Assert.NotEmpty(response.Timetable);
            Assert.NotEmpty(response.Timetable[0].TimeDay);
            Assert.NotEmpty(response.Timetable[0].TimeHour);
            Assert.NotEmpty(response.Timetable[0].Classroom);
            Assert.NotEmpty(response.Type);
            Assert.NotEmpty(response.Teacher);
        }
    }
}
