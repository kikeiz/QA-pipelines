namespace QA.Application.DTOs
{
    public record CreateQAProcessDTO(Guid ShootingId, List<MediaDto> Media);


    public record MediaDto(Guid Id, string OriginalUrl, string OptimizedUrl, string Type, int Position);
}