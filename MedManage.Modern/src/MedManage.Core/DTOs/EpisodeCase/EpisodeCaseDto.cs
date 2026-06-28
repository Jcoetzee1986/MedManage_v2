using System;

namespace MedManage.Core.DTOs.EpisodeCase;

public class CreateEpisodeCaseDto
{
    public int EpisodeId { get; set; }
    public int CaseId { get; set; }
    public DateOnly? DateCreated { get; set; }
}

public class UpdateEpisodeCaseDto
{
    public DateOnly? DateCreated { get; set; }
}

public class EpisodeCaseDto
{
    public int EpisodeId { get; set; }
    public int CaseId { get; set; }
    public DateOnly? DateCreated { get; set; }
    public DateTime? DateInserted { get; set; }
    public DateTime? DateUpdated { get; set; }
}
