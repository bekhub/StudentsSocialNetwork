﻿using System.Threading.Tasks;
using Core.Interfaces.Services;
using Core.ObisApiModels;

namespace ObisRestClient
{
    public class RestApiService : IRestApiService
    {
        private readonly HttpService _httpService;
        
        private const string AUTHENTICATE = "authenticate";
        private const string MAIN_INFO = "main-info";
        private const string STUDENT_INFO = "student-info";
        private const string STUDENT_TAKEN_LESSONS = "student-taken-lessons";
        private const string STUDENT_TRANSCRIPT = "student-transcript";
        private const string STUDENT_SEMESTER_NOTES = "student-semester-notes";
        private const string CHANGE_PASSWORD = "change-password";

        public RestApiService(HttpService httpService)
        {
            _httpService = httpService;
        }

        public Task<Authenticate.Response> AuthenticateAsync(Authenticate.Request request)
        {
            return _httpService.PostAsync<Authenticate.Response>(AUTHENTICATE, request);
        }

        public Task<MainInfo.Response> MainInfoAsync()
        {
            return _httpService.GetAsync<MainInfo.Response>(MAIN_INFO);
        }

        public Task<StudentInfo.Response> StudentInfoAsync()
        {
            return _httpService.GetAsync<StudentInfo.Response>(STUDENT_INFO);
        }

        public Task<StudentTakenLessons.Response> StudentTakenLessonsAsync()
        {
            return _httpService.GetAsync<StudentTakenLessons.Response>(STUDENT_TAKEN_LESSONS);
        }

        public Task<StudentTranscript.Response> StudentTranscriptAsync()
        {
            return _httpService.GetAsync<StudentTranscript.Response>(STUDENT_TRANSCRIPT);
        }

        public Task<StudentSemesterNotes.Response> StudentSemesterNotesAsync()
        {
            return _httpService.GetAsync<StudentSemesterNotes.Response>(STUDENT_SEMESTER_NOTES);
        }

        public Task<ChangePassword.Response> ChangePasswordAsync(ChangePassword.Request request)
        {
            return _httpService.PutAsync<ChangePassword.Response>(CHANGE_PASSWORD, request);
        }

        public void SetAuthKey(string authKey)
        {
            _httpService.SetAuthKey(authKey);
        }
    }
}
