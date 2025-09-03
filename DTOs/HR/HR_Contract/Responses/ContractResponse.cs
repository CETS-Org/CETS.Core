using System;

namespace DTOs.HR.HR_Contract.Responses
{
	public class ContractResponse
	{
		public Guid Id { get; set; }
		public Guid TeacherID { get; set; }
		public string ContractNumber { get; set; } = null!;
		public DateTime? SignedAt { get; set; }
		public DateTime? ExpiredAt { get; set; }
		public Guid ContractStatusID { get; set; }
		public string? ContractUrl { get; set; }
		public string FileHash { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Guid? UpdatedBy { get; set; }
		public bool IsDeleted { get; set; }
	}
}



