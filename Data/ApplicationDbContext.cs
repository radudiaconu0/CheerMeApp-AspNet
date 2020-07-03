using CheerMeApp.Contracts.V1.Responses;
using CheerMeApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CheerMeApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follower> Followers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>()
                .HasMany(post => post.Comments)
                .WithOne(comment => comment.Post)
                .HasForeignKey(comment => comment.PostId)
                .HasPrincipalKey(post => post.Id);
            builder.Entity<Post>()
                .HasMany(post => post.Likes)
                .WithOne(like => like.Post)
                .HasForeignKey(like => like.PostId)
                .HasPrincipalKey(post => post.Id);
            builder.Entity<Post>()
                .HasOne(post => post.User)
                .WithMany(user => user.Posts)
                .HasForeignKey(post => post.UserId)
                .HasPrincipalKey(user => user.Id);
            builder.Entity<Like>()
                .HasOne(like => like.Liker)
                .WithMany(user => user.Likes)
                .HasForeignKey(like => like.UserId)
                .HasPrincipalKey(user => user.Id);
            builder.Entity<Comment>()
                .HasMany(comment => comment.Replies)
                .WithOne(comment => comment.Parent)
                .HasForeignKey(comment => comment.ParentId)
                .HasPrincipalKey(comment => comment.Id);
            builder.Entity<Comment>()
                .HasOne(comment => comment.User)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.UserId)
                .HasPrincipalKey(user => user.Id);
            builder.Entity<Comment>()
                .HasMany(comment => comment.Likes)
                .WithOne(like => like.Comment)
                .HasForeignKey(like => like.CommentId)
                .HasPrincipalKey(comment => comment.Id);
            builder.Entity<User>()
                .HasMany(user => user.Followers)
                .WithOne(follower => follower.FollowerUser)
                .HasForeignKey(follower => follower.FollowableId)
                .HasPrincipalKey(user => user.Id);
            builder.Entity<User>()
                .HasMany(user => user.Followings)
                .WithOne(follower => follower.FollowingUser)
                .HasForeignKey(follower => follower.FollowableId)
                .HasPrincipalKey(user => user.Id);
            base.OnModelCreating(builder);
        }
    }
}