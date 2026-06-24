using FluentAssertions;
using MedManage.Core.Entities;
using MedManage.Infrastructure.Persistence;
using MedManage.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MedManage.Infrastructure.Tests.Repositories;

/// <summary>
/// Tests for the soft delete pattern implementation in the Repository base class.
/// Validates: REQ-1.4 (Implement proper soft delete)
/// </summary>
public class SoftDeleteRepositoryTests : IDisposable
{
    private readonly MedManageDbContext _context;
    private readonly Repository<Member> _repository;

    public SoftDeleteRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<MedManageDbContext>()
            .UseInMemoryDatabase(databaseName: $"SoftDeleteTests_{Guid.NewGuid()}")
            .Options;

        _context = new MedManageDbContext(options);
        _repository = new Repository<Member>(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task DeleteAsync_SetsDateDeleted_InsteadOfRemoving()
    {
        // Arrange
        var member = new Member
        {
            MemberNumber = "MEM001",
            Surname = "Smith",
            Name = "John",
            DateInserted = DateTime.UtcNow
        };
        await _context.Members.AddAsync(member);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(member);
        await _context.SaveChangesAsync();

        // Assert - entity still exists in database with DateDeleted set
        var deletedMember = await _context.Members
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.MemberId == member.MemberId);

        deletedMember.Should().NotBeNull();
        deletedMember!.DateDeleted.Should().NotBeNull();
        deletedMember.DateDeleted.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task GetAllAsync_ExcludesDeletedRecords_ByDefault()
    {
        // Arrange
        var activeMember = new Member
        {
            MemberNumber = "MEM001",
            Surname = "Active",
            Name = "Member",
            DateInserted = DateTime.UtcNow
        };
        var deletedMember = new Member
        {
            MemberNumber = "MEM002",
            Surname = "Deleted",
            Name = "Member",
            DateInserted = DateTime.UtcNow,
            DateDeleted = DateTime.UtcNow.AddDays(-1)
        };

        await _context.Members.AddRangeAsync(activeMember, deletedMember);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.GetAllAsync();

        // Assert - only active member returned
        results.Should().HaveCount(1);
        results.First().MemberNumber.Should().Be("MEM001");
    }

    [Fact]
    public async Task GetAllAsync_IncludesDeletedRecords_WhenIncludeDeletedIsTrue()
    {
        // Arrange
        var activeMember = new Member
        {
            MemberNumber = "MEM001",
            Surname = "Active",
            Name = "Member",
            DateInserted = DateTime.UtcNow
        };
        var deletedMember = new Member
        {
            MemberNumber = "MEM002",
            Surname = "Deleted",
            Name = "Member",
            DateInserted = DateTime.UtcNow,
            DateDeleted = DateTime.UtcNow.AddDays(-1)
        };

        await _context.Members.AddRangeAsync(activeMember, deletedMember);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.GetAllAsync(includeDeleted: true);

        // Assert - both members returned
        results.Should().HaveCount(2);
    }

    [Fact]
    public async Task FindAsync_ExcludesDeletedRecords_ByDefault()
    {
        // Arrange
        var activeMember = new Member
        {
            MemberNumber = "MEM001",
            Surname = "Smith",
            Name = "John",
            DateInserted = DateTime.UtcNow
        };
        var deletedMember = new Member
        {
            MemberNumber = "MEM002",
            Surname = "Smith",
            Name = "Jane",
            DateInserted = DateTime.UtcNow,
            DateDeleted = DateTime.UtcNow.AddDays(-1)
        };

        await _context.Members.AddRangeAsync(activeMember, deletedMember);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.FindAsync(m => m.Surname == "Smith");

        // Assert - only active member returned
        results.Should().HaveCount(1);
        results.First().MemberNumber.Should().Be("MEM001");
    }

    [Fact]
    public async Task FindAsync_IncludesDeletedRecords_WhenIncludeDeletedIsTrue()
    {
        // Arrange
        var activeMember = new Member
        {
            MemberNumber = "MEM001",
            Surname = "Smith",
            Name = "John",
            DateInserted = DateTime.UtcNow
        };
        var deletedMember = new Member
        {
            MemberNumber = "MEM002",
            Surname = "Smith",
            Name = "Jane",
            DateInserted = DateTime.UtcNow,
            DateDeleted = DateTime.UtcNow.AddDays(-1)
        };

        await _context.Members.AddRangeAsync(activeMember, deletedMember);
        await _context.SaveChangesAsync();

        // Act
        var results = await _repository.FindAsync(m => m.Surname == "Smith", includeDeleted: true);

        // Assert - both members returned
        results.Should().HaveCount(2);
    }

    [Fact]
    public async Task RestoreAsync_ClearsDateDeleted_MakingRecordQueryableAgain()
    {
        // Arrange
        var member = new Member
        {
            MemberNumber = "MEM001",
            Surname = "Smith",
            Name = "John",
            DateInserted = DateTime.UtcNow,
            DateDeleted = DateTime.UtcNow.AddDays(-1) // Soft-deleted
        };
        await _context.Members.AddAsync(member);
        await _context.SaveChangesAsync();

        // Verify it's excluded from normal queries
        var beforeRestore = await _repository.GetAllAsync();
        beforeRestore.Should().BeEmpty();

        // Act
        await _repository.RestoreAsync(member);
        await _context.SaveChangesAsync();

        // Assert - member is now returned in normal queries
        var afterRestore = await _repository.GetAllAsync();
        afterRestore.Should().HaveCount(1);
        afterRestore.First().DateDeleted.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_DoesNotRemoveEntityFromDatabase()
    {
        // Arrange
        var member = new Member
        {
            MemberNumber = "MEM001",
            Surname = "Smith",
            Name = "John",
            DateInserted = DateTime.UtcNow
        };
        await _context.Members.AddAsync(member);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(member);
        await _context.SaveChangesAsync();

        // Assert - entity still in database (count with IgnoreQueryFilters)
        var totalCount = await _context.Members.IgnoreQueryFilters().CountAsync();
        totalCount.Should().Be(1);

        // But normal query returns 0
        var normalCount = await _context.Members.CountAsync();
        normalCount.Should().Be(0);
    }
}
