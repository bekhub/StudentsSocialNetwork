﻿namespace Core.Entities
{
    public class CommentLike
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
