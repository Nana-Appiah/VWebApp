﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using VerificationWebApp.DbModels;

namespace VerificationWebApp.DbData
{
    public partial class IDVerificationTestContext : DbContext
    {
        public IDVerificationTestContext()
        {
        }

        public IDVerificationTestContext(DbContextOptions<IDVerificationTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Linkage> Linkages { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Response> Responses { get; set; }
        public virtual DbSet<Verified> Verifieds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Verified>(entity =>
            {
                entity.Property(e => e.AcctNo).HasComment("account number");

                entity.Property(e => e.NationalId).HasComment("Ghana Card or TIN Number");

                entity.Property(e => e.ShortCode).HasComment("The short code from the NIA verification");

                entity.Property(e => e.Telephone).HasComment("The telephone number ");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"data source = 10.150.0.19; initial catalog = IDVerificationTest; user id = sa; password =$Passw0rd; MultipleActiveResultSets = True; ");
            }
        }

    }
}