using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SCAPE.Domain.Entities;

namespace SCAPE.Infraestructure.Context.Configuration
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Date)
                .HasColumnName("date")
                .HasColumnType("datetime");

            entity.Property(e => e.IdEmployee).HasColumnName("idEmployee");

            entity.Property(e => e.IdWorkPlace).HasDefaultValueSql("((68))");

            entity.Property(e => e.Type)
                .IsRequired()
                .HasColumnName("type")
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.IdEmployeeNavigation)
                .WithMany(p => p.Attendance)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("FK_Employee");

            entity.HasOne(d => d.IdWorkPlaceNavigation)
                .WithMany(p => p.Attendance)
                .HasForeignKey(d => d.IdWorkPlace)
                .HasConstraintName("FK_WorkPlaceAttendance");
        }
    }
}
