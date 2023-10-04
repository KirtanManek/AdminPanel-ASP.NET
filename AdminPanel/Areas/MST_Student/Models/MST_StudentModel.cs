using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Areas.MST_Student.Models
{
	public class MST_StudentModel
	{
		public int? StudentID { get; set; }
		[Required]
		public int? CityID { get; set; }
		[Required]
		public int? BranchID { get; set; }
		[Required]
		public string? StudentName { get; set; }
		[Required]
		public string? MobileNoStudent { get; set; }
		[Required]
		public string? Email { get; set; }
		public string? MobileNoFather { get; set; }
		public string? Address { get; set; }
		public DateTime BirthDate { get; set; }
		public int? Age { get; set; }
		public bool? IsActive { get; set; }
		public string? Gender { get; set; }
		public string? Password { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
	}
	public class MST_StudentFilterModel
	{
		public int? StudentID { get; set; }
		public string? StudentName { get; set; }
		public int? CityID { get; set; }
		public int? BranchID { get; set; }
	}

	public class LOC_CityDropDownModel
	{
		public int CityID { get; set; }

		public string? CityName { get; set; }
	}

	public class MST_BranchDropDownModel
	{
		public int BranchID { get; set; }

		public string BranchName { get; set; }
	}
}
