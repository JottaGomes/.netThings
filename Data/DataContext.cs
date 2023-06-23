using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill {Id = 1, Name = "FireBall", Damage = 10}, 
                new Skill {Id = 2, Name = "FrostBall", Damage = 5}, 
                new Skill {Id = 3, Name = "Khamehame", Damage = 99}
            ); 
        }

        public DbSet<Character> Characters => Set<Character>(); 

        public DbSet<User> Users => Set<User>(); 
        
        public DbSet<Weapon> Weapons => Set<Weapon>(); 

         public DbSet<Skill> Skills => Set<Skill>(); 


    } 
}