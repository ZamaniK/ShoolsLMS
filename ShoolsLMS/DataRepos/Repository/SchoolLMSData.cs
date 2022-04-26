using ShoolsLMS.Models;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShoolsLMS.DataRepos.Reposisoty
{
    public class SchoolLMSData
    {
        private readonly DbContext context;
        private readonly IDictionary<Type, object> repositories = new Dictionary<Type, object>();

        public SchoolLMSData()
            : this(new ApplicationDbContext())
        {

        }

        public SchoolLMSData(DbContext context)
        {
            this.context = context;
        }

        public IRepository<Lesson> Teachers
        {
            get
            {
                return this.GetRepository<Lesson>();
            }
        }

        public IRepository<ApplicationUser> Users
        {
            get
            {
                return this.GetRepository<ApplicationUser>();
            }
        }

        public IRepository<Grade> Grades
        {
            get
            {
                return this.GetRepository<Grade>();
            }
        }

        public IRepository<Subject> Subjects
        {
            get
            {
                return this.GetRepository<Subject>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                }
            }
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfModel = typeof(T);

            if (!this.repositories.ContainsKey(typeOfModel))
            {
                var typeOfRepository = typeof(GenericRepository<T>);

                this.repositories.Add(typeOfModel, Activator.CreateInstance(typeOfRepository, this.context));
            }

            return (IRepository<T>)this.repositories[typeOfModel];
        }
    }
}