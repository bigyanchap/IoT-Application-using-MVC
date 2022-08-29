namespace ioxdemo.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class IoXDBContext : DbContext
    {
        public IoXDBContext()
            : base("name=IoXDBContext")
        {
        }

        public virtual DbSet<Pin> Pins { get; set; }
        public virtual DbSet<Thing> Things { get; set; }
        public virtual DbSet<PinType> PinTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Pin>()
            //    .Property(e => e.LastModified)
            //    .IsFixedLength();

            modelBuilder.Entity<Thing>()
                .HasMany(e => e.Pins)
                .WithRequired(e => e.Thing)
                .WillCascadeOnDelete(false);
        }
    }
}
