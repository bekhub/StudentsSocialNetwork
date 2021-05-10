﻿using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Post : BaseEntity
    {
        public string Body { get; set; }

        public bool IsActive { get; set; }

        public bool IsDraft { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        public int LikesCount => PostLikes.Count;

        public int CommentsCount => PostComments.Count;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<PostPicture> Pictures { get; set; } = new();

        public List<Comment> Comments { get; set; } = new();
        public List<PostComment> PostComments { get; set; } = new();

        public List<PostLike> PostLikes { get; set; } = new();

        public List<Tag> Tags { get; set; } = new();
        public List<PostTag> PostTags { get; set; } = new();
    }
}