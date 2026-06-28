namespace MedManage.Core.DTOs.Episode;

public class EpisodeDto
{
    public int EpisodeId { get; set; }
    public string? EpisodeDescription { get; set; }
    public int? MemberId { get; set; }
    public DateOnly? DateCreated { get; set; }
    public DateTime DateInserted { get; set; }
    public DateTime? DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public class CreateEpisodeDto
{
    public string? EpisodeDescription { get; set; }
    public int? MemberId { get; set; }
    public DateOnly? DateCreated { get; set; }
}

public class UpdateEpisodeDto
{
    public int EpisodeId { get; set; }
    public string? EpisodeDescription { get; set; }
    public int? MemberId { get; set; }
    public DateOnly? DateCreated { get; set; }
}

public class EpisodeSearchFilters
{
    public string? EpisodeName { get; set; }
    public int? MemberId { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public bool IncludeDeleted { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
