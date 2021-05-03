using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CommentReplyConfig : IEntityTypeConfiguration<CommentReply>
    {
        public void Configure(EntityTypeBuilder<CommentReply> builder)
        {
            builder.HasKey(x => new {x.TargetId, x.ReplyId});
            
            builder.HasIndex(x => x.ReplyId)
                .IsUnique();
        }
    }
}
