namespace Core.Domain.Entities
{
    public interface ITrackCreatedEntity
    {

        public DateTime CreatedDate { get; set; }
        public string CreatedByUserId { get; set; }
    }
}
