using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN231_Kazilet_API.Models.Entities
{
    public partial class PRN231_Kazilet_v2Context : DbContext
    {
        public PRN231_Kazilet_v2Context()
        {
        }

        public PRN231_Kazilet_v2Context(DbContextOptions<PRN231_Kazilet_v2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Folder> Folders { get; set; } = null!;
        public virtual DbSet<Gameplay> Gameplays { get; set; } = null!;
        public virtual DbSet<GameplayAnswer> GameplayAnswers { get; set; } = null!;
        public virtual DbSet<GameplaySetting> GameplaySettings { get; set; } = null!;
        public virtual DbSet<LearningHistory> LearningHistories { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyCnn"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasMaxLength(200)
                    .HasColumnName("content");

                entity.Property(e => e.IsCorrect).HasColumnName("is_correct");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__Answers__questio__4E88ABD4");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CoursePassword)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("course_password");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.IsPublic).HasColumnName("isPublic");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Courses__created__4F7CD00D");

                entity.HasMany(d => d.Folders)
                    .WithMany(p => p.Courses)
                    .UsingEntity<Dictionary<string, object>>(
                        "FolderCourse",
                        l => l.HasOne<Folder>().WithMany().HasForeignKey("FolderId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FolderCou__folde__5165187F"),
                        r => r.HasOne<Course>().WithMany().HasForeignKey("CourseId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__FolderCou__cours__5070F446"),
                        j =>
                        {
                            j.HasKey("CourseId", "FolderId").HasName("PK__FolderCo__2F1AA7DF2567D70B");

                            j.ToTable("FolderCourse");

                            j.IndexerProperty<int>("CourseId").HasColumnName("course_id");

                            j.IndexerProperty<int>("FolderId").HasColumnName("folder_id");
                        });
            });

            modelBuilder.Entity<Folder>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Folders)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Folders__created__52593CB8");
            });

            modelBuilder.Entity<Gameplay>(entity =>
            {
                entity.ToTable("Gameplay");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("avatar");

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("code")
                    .IsFixedLength();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.IsGetResult).HasColumnName("is_get_result");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.Streak).HasColumnName("streak");

                entity.Property(e => e.Turn).HasColumnName("turn");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Username)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.CodeNavigation)
                    .WithMany(p => p.Gameplays)
                    .HasPrincipalKey(p => p.Code)
                    .HasForeignKey(d => d.Code)
                    .HasConstraintName("FK__Gameplay__code__534D60F1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Gameplays)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Gameplay__user_i__5441852A");
            });

            modelBuilder.Entity<GameplayAnswer>(entity =>
            {
                entity.HasKey(e => new { e.GameplayId, e.QuestionId })
                    .HasName("PK__Gameplay__6539452512EC06B8");

                entity.ToTable("GameplayAnswer");

                entity.Property(e => e.GameplayId).HasColumnName("gameplay_id");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.PlayerAnswer).HasColumnName("player_answer");

                entity.HasOne(d => d.Gameplay)
                    .WithMany(p => p.GameplayAnswers)
                    .HasForeignKey(d => d.GameplayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GameplayA__gamep__5535A963");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.GameplayAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GameplayA__quest__5629CD9C");
            });

            modelBuilder.Entity<GameplaySetting>(entity =>
            {
                entity.ToTable("GameplaySetting");

                entity.HasIndex(e => e.Code, "UQ__Gameplay__357D4CF924B120D9")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("code")
                    .IsFixedLength();

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.IsCompleted).HasColumnName("is_completed");

                entity.Property(e => e.IsHostPlay).HasColumnName("is_host_play");

                entity.Property(e => e.IsSkillEnabled).HasColumnName("is_skill_enabled");

                entity.Property(e => e.IsStarted).HasColumnName("is_started");

                entity.Property(e => e.NoQuestion).HasColumnName("no_question");

                entity.Property(e => e.TimeLimit).HasColumnName("time_limit");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.GameplaySettings)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__GameplayS__cours__571DF1D5");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.GameplaySettings)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__GameplayS__creat__5812160E");
            });

            modelBuilder.Entity<LearningHistory>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CourseId })
                    .HasName("PK__Learning__414FD875F8E87E78");

                entity.ToTable("LearningHistory");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.LearningDate)
                    .HasColumnType("date")
                    .HasColumnName("learning_date");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.LearningHistories)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LearningH__cours__59063A47");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LearningHistories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LearningH__user___59FA5E80");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasMaxLength(100)
                    .HasColumnName("content");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Link)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("link");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Notificat__user___5AEE82B9");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasMaxLength(200)
                    .HasColumnName("content");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.IsMarked).HasColumnName("is_marked");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__Questions__cours__5BE2A6F2");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__AB6E61643A2E19B7")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Gid)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("gid");

                entity.Property(e => e.Password)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.Type)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK__Users__role__5CD6CB2B");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
