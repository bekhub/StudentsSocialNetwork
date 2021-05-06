using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Comment : BaseEntity
    {
        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        public int LikesCount => CommentLikes.Count;

        public int CommentsCount => CommentReplies.Count;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<CommentLike> CommentLikes { get; set; } = new();

        public List<CommentReply> CommentReplies { get; set; } = new();
    }
}
