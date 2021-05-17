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

        public int CommentsCount => Replies.Count;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        public int? TargetId { get; set; }
        public Comment Target { get; set; }

        public int? PostId { get; set; }
        public Post Post { get; set; }

        public List<CommentLike> CommentLikes { get; set; } = new();
        
        public List<Comment> Replies { get; set; } = new();
    }
}
