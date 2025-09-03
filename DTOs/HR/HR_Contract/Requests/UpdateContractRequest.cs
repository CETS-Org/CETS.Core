using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.HR.HR_Contract.Requests
{
	public class UpdateContractRequest
	{
		[Required]
		public Guid TeacherID { get; set; }

		[Required]
		[StringLength(50)]
		public string ContractNumber { get; set; } = null!;

		public DateTime? SignedAt { get; set; }
		public DateTime? ExpiredAt { get; set; }

		[Required]
		public Guid ContractStatusID { get; set; }

		[StringLength(2048)]
		public string? ContractUrl { get; set; }

		[Required]
		[StringLength(64)]
		public string FileHash { get; set; } = null!;

		public bool IsDeleted { get; set; }
	}
}



