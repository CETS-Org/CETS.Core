using System;

namespace DTOs.CORE.LookUp.Responses
{
    public class LookUpResponse
    {
        public Guid LookUpId { get; set; }
        public Guid LookUpTypeId { get; set; }
        public string LookUpTypeCode { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
