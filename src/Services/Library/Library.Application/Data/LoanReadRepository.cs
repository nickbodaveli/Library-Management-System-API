using BuildingBlocks.Pagination;
using Library.Application.Common.Dtos;
using Library.Application.Library.Loans.Queries.GetOverDueLoans;
using MongoDB.Driver;

namespace Library.Application.Data
{
    public class LoanReadRepository : ILoanReadRepository
    {
        private readonly IMongoCollection<LoanReadDto> _loans;

        public LoanReadRepository(IMongoDatabase database)
        {
            _loans = database.GetCollection<LoanReadDto>("LoansRead");
        }

        public async Task<List<LoanReadDto>> GetActiveLoansByMemberIdAsync(Guid memberId, CancellationToken cancellationToken)
        {
            var filter = Builders<LoanReadDto>.Filter.And(
                 Builders<LoanReadDto>.Filter.Eq(l => l.MemberId, memberId),
                 Builders<LoanReadDto>.Filter.Eq(l => l.ReturnDate, null)
             );
            var loans = await _loans.Find(filter).ToListAsync(cancellationToken);
            return loans;
        }

        public async Task<LoanReadDto?> GetLoanDtoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var filter = Builders<LoanReadDto>.Filter.Eq(x => x.Id, id);
            return await _loans.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<GetPaginatedOverDueLoansQueryResult> GetOverdueLoansAsync(
             int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var filter = Builders<LoanReadDto>.Filter.And(
                 Builders<LoanReadDto>.Filter.Eq(l => l.ReturnDate, null),
                 Builders<LoanReadDto>.Filter.Lt(l => l.DueDate, DateTime.UtcNow.Date)
             );
            var totalCount = await _loans.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            var dtos = await _loans.Find(filter)
                .Skip(pageIndex * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            return new GetPaginatedOverDueLoansQueryResult(
                new PaginatedResult<LoanReadDto>(pageIndex, pageSize, (int)totalCount, dtos));
        }


        public async Task AddAsync(LoanReadDto readModel, CancellationToken cancellationToken)
        {
            await _loans.InsertOneAsync(readModel, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(LoanReadDto readModel, CancellationToken cancellationToken)
        {
            var filter = Builders<LoanReadDto>.Filter.Eq(x => x.Id, readModel.Id);
            await _loans.ReplaceOneAsync(filter, readModel, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }

        public Task InvalidateActiveLoansCacheAsync(Guid memberId, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task InvalidatePaginatedOverdueListCachesAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
