namespace Core.Domain.Entities
{
    public interface ITrackUpdatedEntity
    {
        public string? UpdatedUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
