using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Api.Endpoints.Posts
{
    public class Request
    {
        public class CreatePost
        {
            public string Body { get; set; }
            
            public bool IsActive { get; set; }

            public bool IsDraft { get; set; }

            public string UserID { get; set; }
        }
    }
}