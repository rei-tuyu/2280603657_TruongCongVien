using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace _2280603657_TruongCongVien.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=ModelQLSV")
        {
        }

        public virtual DbSet<FACULTY> FACULTies { get; set; }
        public virtual DbSet<STUDENT> STUDENTS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FACULTY>()
                .HasMany(e => e.STUDENTS)
                .WithRequired(e => e.FACULTY)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<STUDENT>()
                .Property(e => e.StudentID)
                .IsUnicode(false);
        }
    }
}
