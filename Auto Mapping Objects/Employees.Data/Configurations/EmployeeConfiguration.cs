﻿using Employees.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder
                .HasOne(e => e.Manager)
                .WithMany(e => e.ManagedEmployees)
                .HasForeignKey(e => e.ManagerId);

            builder.Property(e => e.FirstName)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(e => e.Address)
                .HasMaxLength(250);
        }
    }
}