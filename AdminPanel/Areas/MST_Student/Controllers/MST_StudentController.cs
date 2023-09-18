using AdminPanel.Areas.LOC_City.Models;
using AdminPanel.Areas.MST_Branch.Models;
using AdminPanel.Areas.MST_Student.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace AdminPanel.Areas.MST_Student.Controllers
{
	[Area("MST_Student")]
	[Route("{conntroller}/{action}")]
	public class MST_StudentController : Controller
	{
		private readonly IConfiguration Configuration;
		public MST_StudentController(IConfiguration _configuration)
		{
			Configuration = _configuration;
		}

		#region Get_MST_Student
		public IActionResult MST_StudentList()
		{
			string connectionstr = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection conn = new SqlConnection(connectionstr);
			conn.Open();
			SqlCommand objCmd = conn.CreateCommand();
			objCmd.CommandType = CommandType.StoredProcedure;
			objCmd.CommandText = "PR_Student_SelectAll";
			SqlDataReader objSDR = objCmd.ExecuteReader();
			DataTable dt = new DataTable();
			dt.Load(objSDR);
			conn.Close();
			return View("MST_StudentList", dt);
		}
		#endregion

		#region Delete_MST_Student
		public IActionResult MST_StudentDelete(int StudentID)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "PR_Student_DeleteByPK";
			command.Parameters.AddWithValue("@StudentID", StudentID);
			command.ExecuteNonQuery();
			connection.Close();
			return RedirectToAction("MST_StudentList");
		}
		#endregion

		#region Save_MST_Student
		public IActionResult MST_StudentSave(MST_StudentModel MST_StudentModel, int StudentID = 0)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			if (StudentID == 0)
			{
				command.CommandText = "PR_Student_Insert";
			}
			else
			{
				command.CommandText = "PR_Student_UpdateByPK";
				command.Parameters.AddWithValue("@StudentID", MST_StudentModel.StudentID);
			}
			command.Parameters.AddWithValue("@BranchID", MST_StudentModel.BranchID);
			command.Parameters.AddWithValue("@CityID", MST_StudentModel.CityID);
			command.Parameters.AddWithValue("@StudentName", MST_StudentModel.StudentName);
			command.Parameters.AddWithValue("@MobileNoStudent", MST_StudentModel.MobileNoStudent);
			command.Parameters.AddWithValue("@Email", MST_StudentModel.Email);
			command.Parameters.AddWithValue("@MobileNoFather", MST_StudentModel.MobileNoFather);
			command.Parameters.AddWithValue("@Address", MST_StudentModel.Address);
			command.Parameters.AddWithValue("@BirthDate", MST_StudentModel.BirthDate);
			command.Parameters.AddWithValue("@Age", MST_StudentModel.Age);
			command.Parameters.AddWithValue("@IsActive", MST_StudentModel.IsActive);
			command.Parameters.AddWithValue("@Gender", MST_StudentModel.Gender);
			command.Parameters.AddWithValue("@Password", MST_StudentModel.Password);
			command.ExecuteNonQuery();
			connection.Close();
			return RedirectToAction("MST_StudentList");
		}
		#endregion

		#region AddEdit_MST_Student
		public IActionResult MST_StudentAddEdit(int StudentID = 0)
		{
			#region State ComboBox
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection1 = new SqlConnection(connectionString);
			connection1.Open();
			SqlCommand command1 = connection1.CreateCommand();
			command1.CommandType = CommandType.StoredProcedure;
			command1.CommandText = "PR_City_IDName";
			SqlDataReader reader1 = command1.ExecuteReader();
			DataTable table1 = new DataTable();
			table1.Load(reader1);
			connection1.Close();

			List<LOC_CityDropDownModel> list = new List<LOC_CityDropDownModel>();
			foreach (DataRow row in table1.Rows)
			{
				LOC_CityDropDownModel lOC_CityDropDownModel = new LOC_CityDropDownModel();
				lOC_CityDropDownModel.CityID = Convert.ToInt32(row["CityID"]);
				lOC_CityDropDownModel.CityName = row["CityName"].ToString();
				list.Add(lOC_CityDropDownModel);
			}
			ViewBag.CityList = list;
			#endregion
			#region Branch ComboBox
			SqlConnection connection2 = new SqlConnection(connectionString);
			connection2.Open();
			SqlCommand command2 = connection2.CreateCommand();
			command2.CommandType = CommandType.StoredProcedure;
			command2.CommandText = "PR_Branch_IDName";
			SqlDataReader reader2 = command2.ExecuteReader();
			DataTable table2 = new DataTable();
			table2.Load(reader2);
			connection2.Close();

			List<MST_BranchDropDownModel> list2 = new List<MST_BranchDropDownModel>();
			foreach (DataRow row in table2.Rows)
			{
				MST_BranchDropDownModel MST_BranchDropDownModel = new MST_BranchDropDownModel();
				MST_BranchDropDownModel.BranchID = Convert.ToInt32(row["BranchID"]);
				MST_BranchDropDownModel.BranchName = row["BranchName"].ToString();
				list2.Add(MST_BranchDropDownModel);
			}
			ViewBag.BranchList = list2;
			#endregion
			#region Add
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "PR_Student_SelectByPK";
			command.Parameters.AddWithValue("@StudentID", StudentID);
			SqlDataReader reader = command.ExecuteReader();
			DataTable table = new DataTable();
			table.Load(reader);
			MST_StudentModel MST_StudentModel = new MST_StudentModel();
			foreach (DataRow dataRow in table.Rows)
			{
				MST_StudentModel.StudentID = Convert.ToInt32(dataRow["StudentID"]);
				MST_StudentModel.CityID = Convert.ToInt32(dataRow["CityID"]);
				MST_StudentModel.BranchID = Convert.ToInt32(dataRow["BranchID"]);
				MST_StudentModel.StudentName = dataRow["StudentName"].ToString();
				MST_StudentModel.MobileNoStudent = dataRow["MobileNoStudent"].ToString();
				MST_StudentModel.Email = dataRow["Email"].ToString();
				MST_StudentModel.MobileNoFather = dataRow["MobileNoFather"].ToString();
				MST_StudentModel.Address = dataRow["Address"].ToString();
				MST_StudentModel.BirthDate = Convert.ToDateTime(dataRow["BirthDate"]);
				MST_StudentModel.Age = Convert.ToInt32(dataRow["Age"]);
				MST_StudentModel.IsActive = Convert.ToBoolean(dataRow["IsActive"]);
				MST_StudentModel.Gender = dataRow["Gender"].ToString();
				MST_StudentModel.Password = dataRow["Password"].ToString();
			}
			return View("MST_StudentAddEdit", MST_StudentModel);
			#endregion
		}
		#endregion
	}
}
