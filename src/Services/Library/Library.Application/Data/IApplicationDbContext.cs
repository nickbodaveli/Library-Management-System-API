using Library.Domain.Models.Book;
using Library.Domain.Models.Loan;
using Library.Domain.Models.Member;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Book> Books { get; }
        DbSet<Member> Members { get; }
        DbSet<Loan> Loans { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
