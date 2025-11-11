using BuildingBlocks.Pagination;
using Library.Application.Common.Dtos;
using Library.Application.Library.Members.Queries.GetPaginatedMembers;
using MongoDB.Driver;

namespace Library.Application.Data
{
    public class MemberReadRepository : IMemberReadRepository
    {
        private readonly IMongoCollection<MemberReadDto> _members;

        public MemberReadRepository(IMongoDatabase database)
        {
            _members = database.GetCollection<MemberReadDto>("MembersRead");
        }

        public async Task<MemberReadDto?> GetMemberDtoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var filter = Builders<MemberReadDto>.Filter.Eq(x => x.Id, id);
            return await _members.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<GetPaginatedMemberQueryResult> GetPaginatedMembersAsync(
            int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var filter = Builders<MemberReadDto>.Filter.Empty;

            var totalCount = await _members.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            var dtos = await _members.Find(filter)
                .Skip(pageIndex * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            return new GetPaginatedMemberQueryResult(
                new PaginatedResult<MemberReadDto>(
                    pageIndex,
                    pageSize,
                    (int)totalCount,
                    dtos));
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var filter = Builders<MemberReadDto>.Filter.Eq(x => x.Email, email);
            var count = await _members.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            return count > 0;
        }

        public async Task AddAsync(MemberReadDto readModel, CancellationToken cancellationToken)
        {
            await _members.InsertOneAsync(readModel, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(MemberReadDto readModel, CancellationToken cancellationToken)
        {
            var filter = Builders<MemberReadDto>.Filter.Eq(x => x.Id, readModel.Id);
            await _members.ReplaceOneAsync(filter, readModel, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }

        public Task InvalidatePaginatedListCachesAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
