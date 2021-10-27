using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SCAPE.Domain.Entities;
using System;

namespace SCAPE.Infraestructure.Context.Configuration
{
    public class EmployerConfiguration : IEntityTypeConfiguration<Employer>
    {
        public void Configure(EntityTypeBuilder<Employer> entity)
        {
            entity.HasIndex(e => e.DocumentId)
                    .HasName("UQ__Employer__EFAAAD8430F86768")
                    .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.DocumentId)
                .IsRequired()
                .HasColumnName("documentId")
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasColumnName("firstName")
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasColumnName("lastName")
                .HasMaxLength(500)
                .IsUnicode(false);
        }
    }
}
