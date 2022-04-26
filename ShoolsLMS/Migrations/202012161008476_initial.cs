namespace ShoolsLMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ADLogs",
                c => new
                    {
                        Email = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        RetypePassword = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Email);
            
            CreateTable(
                "dbo.AnswerChoices",
                c => new
                    {
                        AnswerChoiceId = c.Int(nullable: false, identity: true),
                        Choices = c.String(nullable: false),
                        IsCorrect = c.Boolean(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerChoiceId)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(nullable: false),
                        QuizId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: true)
                .Index(t => t.QuizId);
            
            CreateTable(
                "dbo.Quizs",
                c => new
                    {
                        QuizId = c.Int(nullable: false, identity: true),
                        QuizName = c.String(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        Duration = c.Time(nullable: false, precision: 7),
                        EndTime = c.DateTime(nullable: false),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuizId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.StudentQuizs",
                c => new
                    {
                        StudentId = c.String(nullable: false, maxLength: 128),
                        QuizId = c.Int(nullable: false),
                        Marks = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentId, t.QuizId })
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.QuizId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ConfirmedEmail = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.StudentAssignments",
                c => new
                    {
                        StudentId = c.String(nullable: false, maxLength: 128),
                        AssignmentId = c.Int(nullable: false),
                        Subject_SubjectId = c.Int(),
                    })
                .PrimaryKey(t => new { t.StudentId, t.AssignmentId })
                .ForeignKey("dbo.Subjects", t => t.Subject_SubjectId)
                .ForeignKey("dbo.Assignments", t => t.AssignmentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.AssignmentId)
                .Index(t => t.Subject_SubjectId);
            
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        AssignmentId = c.Int(nullable: false, identity: true),
                        AssignmentName = c.String(nullable: false),
                        LastDate = c.DateTime(nullable: false),
                        FileName = c.String(maxLength: 255),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AssignmentId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        SubjectCode = c.String(nullable: false, maxLength: 20),
                        SubjectName = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        GradeId = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        Rating = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SubjectId)
                .ForeignKey("dbo.Grades", t => t.GradeId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.GradeId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        GradeId = c.Int(nullable: false, identity: true),
                        GradeName = c.String(nullable: false),
                        ParentCategory_GradeId = c.Int(),
                    })
                .PrimaryKey(t => t.GradeId)
                .ForeignKey("dbo.Grades", t => t.ParentCategory_GradeId)
                .Index(t => t.ParentCategory_GradeId);
            
            CreateTable(
                "dbo.StudentGrades",
                c => new
                    {
                        StudentId = c.String(nullable: false, maxLength: 128),
                        GradeId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentId, t.GradeId })
                .ForeignKey("dbo.Grades", t => t.GradeId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.GradeId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        TeacherId = c.Int(nullable: false, identity: true),
                        TeacherName = c.String(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        FileName = c.String(maxLength: 255),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TeacherId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.SubjectId)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Certificates",
                c => new
                    {
                        CertificateId = c.Int(nullable: false, identity: true),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CertificateId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID, cascadeDelete: true)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactEmail = c.String(nullable: false, maxLength: 128),
                        ContactName = c.String(nullable: false),
                        ContactMessage = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ContactEmail);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        RatingId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        stars = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RatingId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.SubjectId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Ratings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ratings", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Certificates", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Certificates", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Quizs", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.StudentQuizs", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentAssignments", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentAssignments", "AssignmentId", "dbo.Assignments");
            DropForeignKey("dbo.Assignments", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Teachers", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Teachers", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "GradeId", "dbo.Grades");
            DropForeignKey("dbo.Grades", "ParentCategory_GradeId", "dbo.Grades");
            DropForeignKey("dbo.StudentGrades", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentGrades", "GradeId", "dbo.Grades");
            DropForeignKey("dbo.StudentAssignments", "Subject_SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.StudentQuizs", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.Questions", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.AnswerChoices", "QuestionId", "dbo.Questions");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Ratings", new[] { "UserId" });
            DropIndex("dbo.Ratings", new[] { "SubjectId" });
            DropIndex("dbo.Certificates", new[] { "SubjectId" });
            DropIndex("dbo.Certificates", new[] { "ApplicationUserID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Teachers", new[] { "ApplicationUserID" });
            DropIndex("dbo.Teachers", new[] { "SubjectId" });
            DropIndex("dbo.StudentGrades", new[] { "GradeId" });
            DropIndex("dbo.StudentGrades", new[] { "StudentId" });
            DropIndex("dbo.Grades", new[] { "ParentCategory_GradeId" });
            DropIndex("dbo.Subjects", new[] { "User_Id" });
            DropIndex("dbo.Subjects", new[] { "GradeId" });
            DropIndex("dbo.Assignments", new[] { "SubjectId" });
            DropIndex("dbo.StudentAssignments", new[] { "Subject_SubjectId" });
            DropIndex("dbo.StudentAssignments", new[] { "AssignmentId" });
            DropIndex("dbo.StudentAssignments", new[] { "StudentId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.StudentQuizs", new[] { "QuizId" });
            DropIndex("dbo.StudentQuizs", new[] { "StudentId" });
            DropIndex("dbo.Quizs", new[] { "SubjectId" });
            DropIndex("dbo.Questions", new[] { "QuizId" });
            DropIndex("dbo.AnswerChoices", new[] { "QuestionId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Ratings");
            DropTable("dbo.Contacts");
            DropTable("dbo.Certificates");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Teachers");
            DropTable("dbo.StudentGrades");
            DropTable("dbo.Grades");
            DropTable("dbo.Subjects");
            DropTable("dbo.Assignments");
            DropTable("dbo.StudentAssignments");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.StudentQuizs");
            DropTable("dbo.Quizs");
            DropTable("dbo.Questions");
            DropTable("dbo.AnswerChoices");
            DropTable("dbo.ADLogs");
        }
    }
}
