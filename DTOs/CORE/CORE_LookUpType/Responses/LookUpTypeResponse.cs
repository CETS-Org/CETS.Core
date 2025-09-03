using System;

namespace DTOs.CORE.LookUpType.Responses
{
    public class LookUpTypeResponse
    {
        public Guid LookUpTypeId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
