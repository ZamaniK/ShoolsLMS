using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShoolsLMS.Models.Data;

namespace ShoolsLMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string HomeAddress { get; set; }

        public bool ConfirmedEmail { get; set; }
        public ICollection<StudentAssignment> Assignments { get; set; }
        public virtual ICollection<StudentGrade> Subjects { get; set; }
        public ICollection<StudentQuiz> Quiz { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AnswerChoice> Answerchoices { get; set; }
        public DbSet<Quiz> Quiz { get; set; }
        public DbSet<Assignment> Assignment { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Paper> Papers { get; set; }

        public DbSet<StudentGrade> StudentGrades { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }
        public DbSet<StudentQuiz> StudentQuiz { get; set; }
        public DbSet<ADLog> ADLogs { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<SubjectPictures> SubjectPictures { get; set; }
        public DbSet<BookPDF> BookPDFs { get; set; }
        public DbSet<BooksBookPDF> BooksBookPDFs { get; set; }
        public DbSet<TeacherPictures> teacherPictures { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StudentGrade>()
                .HasKey(t => new { t.StudentId, t.GradeId });


            modelBuilder.Entity<StudentGrade>()
                .HasRequired(pt => pt.Student)
                .WithMany(p => p.Subjects)
                .HasForeignKey(pt => pt.StudentId);

            modelBuilder.Entity<StudentGrade>()
                .HasRequired(pt => pt.Grade)
                .WithMany(t => t.Enrollments)
                .HasForeignKey(pt => pt.GradeId);

            modelBuilder.Entity<StudentSubject>()
                .HasKey(t => new { t.StudentId, t.SubjectId });


            //modelBuilder.Entity<StudentSubject>()
            //    .HasRequired(pt => pt.Student)
            //    .WithMany(p => p.Subjects)
            //    .HasForeignKey(pt => pt.StudentId);

            modelBuilder.Entity<StudentSubject>()
                .HasRequired(pt => pt.Subject)
                .WithMany(t => t.Enrollments)
                .HasForeignKey(pt => pt.SubjectId);


            modelBuilder.Entity<StudentQuiz>()
                .HasKey(t => new { t.StudentId, t.QuizId });


            modelBuilder.Entity<StudentQuiz>()
                .HasRequired(pt => pt.Student)
                .WithMany(p => p.Quiz)
                .HasForeignKey(pt => pt.StudentId);

            modelBuilder.Entity<StudentQuiz>()
                .HasRequired(pt => pt.Quiz)
                .WithMany(t => t.Students)
                .HasForeignKey(pt => pt.QuizId);


            modelBuilder.Entity<StudentAssignment>()
                .HasKey(t => new { t.StudentId, t.AssignmentId });


            modelBuilder.Entity<StudentAssignment>()
                .HasRequired(pt => pt.Student)
                .WithMany(p => p.Assignments)
                .HasForeignKey(pt => pt.StudentId);

            modelBuilder.Entity<StudentAssignment>()
                .HasRequired(pt => pt.Assignment)
                .WithMany(d => d.Assignments)
                .HasForeignKey(pt => pt.AssignmentId);

        }
    }
}