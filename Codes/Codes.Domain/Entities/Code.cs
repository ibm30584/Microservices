using Core.Domain.Entities;

namespace Codes.Domain.Entities
{
    public class Code : ITrackCreatedEntity, ITrackUpdatedEntity
    {
        public int CodeId { get; set; }
        public required string Value { get; set; }
        public required string Text { get; set; }
        public string? Text2 { get; set; }
        public bool Enabled { get; set; } = true;

        public string CreatedByUserId { get; set; } = null!;
        public string? UpdatedUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }


        public List<Metadata>? Metadata { get; set; }
        public int CodeTypeId { get; set; }
        public CodeType CodeType { get; set; } = null!;

    }
}