using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN231_Kazilet_API.Models.Entities
{
    public partial class PRN231_KaziletContext : DbContext
    {
        public PRN231_KaziletContext()
        {
        }

        public PRN231_KaziletContext(DbContextOptions<PRN231_KaziletContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Folder> Folders { get; set; } = null!;
        public virtual DbSet<FolderCourse> FolderCourses { get; set; } = null!;
        public virtual DbSet<Gameplay> Gameplays { get; set; } = null!;
        public virtual DbSet<GameplaySetting> GameplaySettings { get; set; } = null!;
        public virtual DbSet<LearningHistory> LearningHistories { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<QuestionStatus> QuestionStatuses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=(local);database=PRN231_Kazilet;uid=sa;pwd=123;TrustServerCertificate=True;");
            }
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
                    .HasConstraintName("FK__Answers__questio__4D94879B");
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

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Courses__created__4E88ABD4");
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
                    .HasConstraintName("FK__Folders__created__5165187F");
            });

            modelBuilder.Entity<FolderCourse>(entity =>
            {
                entity.ToTable("FolderCourse");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.FolderId).HasColumnName("folder_id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.FolderCourses)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__FolderCou__cours__4F7CD00D");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.FolderCourses)
                    .HasForeignKey(d => d.FolderId)
                    .HasConstraintName("FK__FolderCou__folde__5070F446");
            });

            modelBuilder.Entity<Gameplay>(entity =>
            {
                entity.ToTable("Gameplay");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("code")
                    .IsFixedLength();

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.IsCorrect).HasColumnName("is_correct");

                entity.Property(e => e.PlayerAnswer)
                    .HasMaxLength(200)
                    .HasColumnName("player_answer");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.Score).HasColumnName("score");

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
                    .HasConstraintName("FK__Gameplay__code__52593CB8");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Gameplays)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__Gameplay__questi__534D60F1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Gameplays)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Gameplay__user_i__5441852A");
            });

            modelBuilder.Entity<GameplaySetting>(entity =>
            {
                entity.ToTable("GameplaySetting");

                entity.HasIndex(e => e.Code, "UQ__Gameplay__357D4CF99BA0C3FE")
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
                    .HasConstraintName("FK__GameplayS__cours__5535A963");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.GameplaySettings)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__GameplayS__creat__5629CD9C");
            });

            modelBuilder.Entity<LearningHistory>(entity =>
            {
                entity.ToTable("LearningHistory");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.LearningDate)
                    .HasColumnType("datetime")
                    .HasColumnName("learning_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.LearningHistories)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__LearningH__cours__571DF1D5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LearningHistories)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__LearningH__user___5812160E");
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
                    .HasConstraintName("FK__Notificat__user___59063A47");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasMaxLength(200)
                    .HasColumnName("content");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.IsMarked).HasColumnName("is_marked");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__Questions__cours__59FA5E80");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK__Questions__statu__5AEE82B9");
            });

            modelBuilder.Entity<QuestionStatus>(entity =>
            {
                entity.ToTable("QuestionStatus");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<User>(entity =>
            {
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
                    .HasConstraintName("FK__Users__role__5BE2A6F2");
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
