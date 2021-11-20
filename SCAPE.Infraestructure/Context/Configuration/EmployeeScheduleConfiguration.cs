using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SCAPE.Domain.Entities;
using System;

namespace SCAPE.Infraestructure.Context.Configuration
{
    public class EmployeeScheduleConfiguration : IEntityTypeConfiguration<EmployeeSchedule>
    {
        public void Configure(EntityTypeBuilder<EmployeeSchedule> entity)
        {
            entity.HasKey(e => new { e.Id, e.IdEmployee, e.IdWorkPlace });

            entity.ToTable("Employee_Schedule");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.IdEmployee).HasColumnName("idEmployee");

            entity.Property(e => e.IdWorkPlace).HasColumnName("idWorkPlace");

            entity.Property(e => e.DayOfWeek).HasColumnName("dayOfWeek");

            entity.Property(e => e.EndMinute).HasColumnName("endMinute");

            entity.Property(e => e.StartMinute).HasColumnName("startMinute");

            entity.HasOne(d => d.IdEmployeeNavigation)
                .WithMany(p => p.EmployeeSchedule)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("FK_Employee_WorkPlace_Schedule_2");

            entity.HasOne(d => d.IdWorkPlaceNavigation)
                .WithMany(p => p.EmployeeSchedule)
                .HasForeignKey(d => d.IdWorkPlace)
                .HasConstraintName("FK_Employee_WorkPlace_Schedule");

        }
    }
}
