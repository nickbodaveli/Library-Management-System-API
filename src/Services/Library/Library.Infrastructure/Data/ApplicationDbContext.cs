using Library.Application.Data;
using Library.Domain.Models.Book;
using Library.Domain.Models.Loan;
using Library.Domain.Models.Member;
using Library.Domain.ValueObjects.Book;
using Library.Domain.ValueObjects.Loan;
using Library.Domain.ValueObjects.Member;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Loan> Loans => Set<Loan>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(b =>
            {
                b.HasKey(b => b.Id);
                b.Property(b => b.Version)
                    .IsConcurrencyToken();
                b.Property(o => o.Id)
                    .HasConversion(
                        id => id.Value,
                        value => BookId.Of(value))
                    .HasColumnType("uuid")
                    .ValueGeneratedNever();

                b.HasIndex(b => b.ISBN)
                    .IsUnique()
                    .HasDatabaseName("ix_book_isbn");

                b.Ignore(a => a.DomainEvents);
            });

            modelBuilder.Entity<Member>(m =>
            {
                m.HasKey(m => m.Id);

                m.Property(o => o.Id)
                    .HasConversion(
                        id => id.Value,
                        value => MemberId.Of(value))
                    .HasColumnType("uuid")
                    .ValueGeneratedNever();

                m.HasIndex(m => m.Email)
                    .IsUnique()
                    .HasDatabaseName("ix_member_email");

                m.Ignore(a => a.DomainEvents);
            });

            modelBuilder.Entity<Loan>(l =>
            {
                l.HasKey(l => l.Id);

                l.Property(o => o.Id)
                    .HasConversion(
                        id => id.Value,
                        value => LoanId.Of(value))
                    .HasColumnType("uuid")
                    .ValueGeneratedNever();

                l.Property(o => o.BookId)
                    .HasConversion(
                        id => id.Value,
                        value => BookId.Of(value))
                    .HasColumnType("uuid");

                l.Property(o => o.MemberId)
                    .HasConversion(
                        id => id.Value,
                        value => MemberId.Of(value))
                    .HasColumnType("uuid");

                l.HasIndex(l => l.BookId).HasDatabaseName("ix_loan_book_id");
                l.HasIndex(l => l.MemberId).HasDatabaseName("ix_loan_member_id");
                l.HasIndex(l => new { l.MemberId, l.BookId }).HasDatabaseName("ix_loan_member_book");

                l.Ignore(a => a.DomainEvents);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
