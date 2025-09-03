using System;

namespace DTOs.CORE.LookUp.Requests
{
    public class UpdateLookUpRequest
    {
        public Guid LookUpTypeId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
