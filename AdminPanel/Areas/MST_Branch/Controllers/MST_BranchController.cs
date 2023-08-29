using AdminPanel.Areas.MST_Branch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using System.Data;
using System.Data.SqlClient;

namespace AdminPanel.Areas.MST_Branch.Controllers
{
	[Area("MST_Branch")]
	[Route("{conntroller}/{action}")]
	public class MST_BranchController : Controller
	{
		private readonly IConfiguration Configuration;
		public MST_BranchController(IConfiguration _configuration)
		{
			Configuration = _configuration;
		}

		#region Get_MST_BranchList
		public IActionResult MST_BranchList()
		{
			string connectionstr = this.Configuration.GetConnectionString("myConnectionString");
			//Prepare a connection
			DataTable dt = new DataTable();
			SqlConnection conn = new SqlConnection(connectionstr);
			conn.Open();
			SqlCommand objCmd = conn.CreateCommand();
			objCmd.CommandType = CommandType.StoredProcedure;
			objCmd.CommandText = "PR_Branch_SelectAll";
			SqlDataReader objSDR = objCmd.ExecuteReader();
			dt.Load(objSDR);
			return View("MST_BranchList", dt);
		}
		#endregion

		#region AddEdit_MST_Branch
		public IActionResult MST_BranchAddEdit(int BranchID = 0)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "PR_Branch_SelectByPK";
			command.Parameters.AddWithValue("@BranchID", BranchID);
			SqlDataReader reader = command.ExecuteReader();
			DataTable table = new DataTable();
			table.Load(reader);
			MST_BranchModel mst_BranchModel = new MST_BranchModel();
			foreach (DataRow dataRow in table.Rows)
			{
				mst_BranchModel.BranchID = Convert.ToInt32(dataRow["BranchID"]);
				mst_BranchModel.BranchName = dataRow["BranchName"].ToString();
				mst_BranchModel.BranchCode = dataRow["BranchCode"].ToString();
			}
			return View("MST_BranchAddEdit", mst_BranchModel);
		}
		#endregion

		#region Save_MST_Branch
		public IActionResult MST_BranchSave(MST_BranchModel mst_BranchModel, int BranchID = 0)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			if (BranchID == 0)
			{
				command.CommandText = "PR_Branch_Insert";
			}
			else
			{
				command.CommandText = "PR_Branch_UpdateByPK";
				command.Parameters.AddWithValue("@BranchID", BranchID);
			}
			command.Parameters.AddWithValue("@BranchName", mst_BranchModel.BranchName);
			command.Parameters.AddWithValue("@BranchCode", mst_BranchModel.BranchCode);
			command.ExecuteNonQuery();
			connection.Close();
			return RedirectToAction("MST_BranchList");
		}
		#endregion

		#region Delete_MST_Branch
		public IActionResult MST_BranchDelete(int BranchID)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "PR_Branch_DeleteByPK";
			command.Parameters.AddWithValue("@BranchID", BranchID);
			command.ExecuteNonQuery();
			connection.Close();
			return RedirectToAction("MST_BranchList");
		}
		#endregion
	}
}
