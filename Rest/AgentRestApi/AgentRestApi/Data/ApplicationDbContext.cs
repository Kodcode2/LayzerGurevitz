﻿using AgentRestApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AgentRestApi.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IConfiguration configuration) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<MissionModel> Missions { get; set; }
        public DbSet<AgentModel> Agents { get; set; }
        public DbSet<TargetModel> Targets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Saves us Status as a string

            modelBuilder.Entity<AgentModel>()
                .Property(x => x.Agentstatus)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<TargetModel>()
               .Property(x => x.TargetStatus)
               .HasConversion<string>()
               .IsRequired();

            modelBuilder.Entity<MissionModel>()
               .Property(x => x.MissionStatus)
               .HasConversion<string>()
               .IsRequired();

            modelBuilder.Entity<MissionModel>()
                .HasOne(m => m.Agent)
                .WithMany(m => m.Missions)               
                .HasForeignKey(m => m.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MissionModel>()
                .HasOne(m => m.Targets)
                .WithMany()
                .HasForeignKey(m => m.TargetId)
                .OnDelete(DeleteBehavior.Restrict);
            

            base.OnModelCreating(modelBuilder);
        }

    
    }
}
