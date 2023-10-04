using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Areas.MST_Branch.Models
{
	public class MST_BranchModel
	{
		public int BranchID { get; set; }

		[Required]
		public string? BranchName { get; set; }
		[Required]
		public string? BranchCode { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
	}

	public class MST_BranchDropDownModel
	{
		public int? BranchID { get; set; }

		[Required]
		public string? BranchName { get; set; }
	}
	public class MST_BranchFilterModel
	{
		public int? BranchID { get; set; }
		public string? BranchName { get; set; }
		public string? BranchCode { get; set; }
	}
}
