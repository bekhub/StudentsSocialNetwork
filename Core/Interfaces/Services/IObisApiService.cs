using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ObisApiModels;

namespace Core.Interfaces.Services
{
    public interface IObisApiService
    {
        Task<Authenticate.Response> AuthenticateAsync(Authenticate.Request request);
        
        Task<Authenticate.Response> AuthenticateAsync(string studentNumber, string studentPassword);
        
        Task<MainInfo.Response> MainInfoAsync();
        
        Task<StudentInfo.Response> StudentInfoAsync();
        
        Task<List<StudentTakenLessons.Response>> StudentTakenLessonsAsync();
        
        Task<StudentTranscript.Response> StudentTranscriptAsync();
        
        Task<StudentSemesterNotes.Response> StudentSemesterNotesAsync();
        
        Task<ChangePassword.Response> ChangePasswordAsync(ChangePassword.Request request);

        Task<WakeUp.Response> WakeUpAsync();

        void SetAuthKey(string authKey);
    }
}
