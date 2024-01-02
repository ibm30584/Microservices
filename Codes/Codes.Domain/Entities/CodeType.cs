using Core.Domain.Entities;

namespace Codes.Domain.Entities
{
    public class CodeType : ITrackCreatedEntity, ITrackUpdatedEntity
    {
        public int CodeTypeId { get; set; }
        public string Value { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string? Text2 { get; set; }

        public string CreatedByUserId { get; set; } = null!;
        public string? UpdatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


        public List<Code> Codes { get; set; } = [];
    }
}
