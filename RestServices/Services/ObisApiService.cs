using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Interfaces.Services;
using Core.ObisApiModels;
using Microsoft.Extensions.Options;
using RestServices.Settings;

namespace RestServices.Services
{
    public class ObisApiService : BaseHttpService, IObisApiService
    {
        private const string AUTHENTICATE = "authenticate";
        private const string MAIN_INFO = "main-info";
        private const string STUDENT_INFO = "student-info";
        private const string STUDENT_TAKEN_LESSONS = "student-taken-lessons";
        private const string STUDENT_TRANSCRIPT = "student-transcript";
        private const string STUDENT_SEMESTER_NOTES = "student-semester-notes";
        private const string CHANGE_PASSWORD = "change-password";
        private const string WAKE_UP = "wake-up";
        
        public ObisApiService(HttpClient httpClient, IOptions<ObisApiSettings> apiSettingsOptions) : base(httpClient)
        {
            var apiSettings = apiSettingsOptions.Value;
            HttpClient.BaseAddress = new Uri(apiSettings.BaseUrl);
            HttpClient.DefaultRequestHeaders.Add("appKey", apiSettings.AppKey);
        }

        public async Task<Authenticate.Response> AuthenticateAsync(Authenticate.Request request)
        {
            var response = await PostAsync<Authenticate.Response>(AUTHENTICATE, request);
            if (response != null && !string.IsNullOrEmpty(response.AuthKey))
                SetAuthKey(response.AuthKey);
            return response;
        }

        public Task<Authenticate.Response> AuthenticateAsync(string studentNumber, string studentPassword)
        {
            return AuthenticateAsync(new Authenticate.Request
            {
                Number = studentNumber,
                Password = studentPassword,
            });
        }

        public Task<MainInfo.Response> MainInfoAsync()
        {
            return GetAsync<MainInfo.Response>(MAIN_INFO);
        }

        public Task<StudentInfo.Response> StudentInfoAsync()
        {
            return GetAsync<StudentInfo.Response>(STUDENT_INFO);
        }

        public Task<List<StudentTakenLessons.Response>> StudentTakenLessonsAsync()
        {
            return GetAsync<List<StudentTakenLessons.Response>>(STUDENT_TAKEN_LESSONS);
        }

        public Task<StudentTranscript.Response> StudentTranscriptAsync()
        {
            return GetAsync<StudentTranscript.Response>(STUDENT_TRANSCRIPT);
        }

        public Task<StudentSemesterNotes.Response> StudentSemesterNotesAsync()
        {
            return GetAsync<StudentSemesterNotes.Response>(STUDENT_SEMESTER_NOTES);
        }

        public Task<ChangePassword.Response> ChangePasswordAsync(ChangePassword.Request request)
        {
            return PutAsync<ChangePassword.Response>(CHANGE_PASSWORD, request);
        }

        public Task<WakeUp.Response> WakeUpAsync()
        {
            return GetAsync<WakeUp.Response>(WAKE_UP);
        }

        public void SetAuthKey(string authKey)
        {
            HttpClient.DefaultRequestHeaders.Add("Authorization", authKey);
        }
    }
}
