using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AdminPanel.Areas.LOC_State.Models;
using AdminPanel.Areas.LOC_Country.Models;

namespace AdminPanel.Areas.LOC_State.Controllers
{
	[Area("LOC_State")]
	[Route("{conntroller}/{action}")]
	public class LOC_StateController : Controller
	{
		private readonly IConfiguration Configuration;
		public LOC_StateController(IConfiguration _configuration)
		{
			Configuration = _configuration;
		}

		#region Get_LOC_StateList
		public IActionResult LOC_StateList()
		{
			string connectionstr = this.Configuration.GetConnectionString("myConnectionString");

			SqlConnection connection1 = new(connectionstr);
			connection1.Open();
			SqlCommand command1 = connection1.CreateCommand();
			command1.CommandType = CommandType.StoredProcedure;
			command1.CommandText = "PR_Country_IDName";
			SqlDataReader reader1 = command1.ExecuteReader();
			DataTable table1 = new();
			table1.Load(reader1);
			connection1.Close();

			List<LOC_CountryDropDownModel> countryList = new();
			foreach (DataRow row in table1.Rows)
			{
                LOC_CountryDropDownModel lOC_CountryDropDownModel = new()
                {
                    CountryID = Convert.ToInt32(row["CountryID"]),
                    CountryName = row["CountryName"].ToString()
                };
                countryList.Add(lOC_CountryDropDownModel);
			}
			ViewBag.CountryList = countryList;


			DataTable dt = new();
			SqlConnection conn = new(connectionstr);
			conn.Open();
			SqlCommand objCmd = conn.CreateCommand();
			objCmd.CommandType = CommandType.StoredProcedure;
			objCmd.CommandText = "PR_State_SelectAll";
			SqlDataReader objSDR = objCmd.ExecuteReader();
			dt.Load(objSDR);
			conn.Close();
			return View("LOC_StateList", dt);
		}
		#endregion

		#region AddEdit_State
		public IActionResult LOC_StateAddEdit(int StateID = 0)
		{
			#region ComboBox
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection1 = new(connectionString);
			connection1.Open();
			SqlCommand command1 = connection1.CreateCommand();
			command1.CommandType = CommandType.StoredProcedure;
			command1.CommandText = "PR_Country_IDName";
			SqlDataReader reader1 = command1.ExecuteReader();
			DataTable table1 = new();
			table1.Load(reader1);
			connection1.Close();

			List<LOC_CountryDropDownModel> countryList = new();
			foreach (DataRow row in table1.Rows)
			{
                LOC_CountryDropDownModel loc_CountryDropDownModel = new()
                {
                    CountryID = Convert.ToInt32(row["CountryID"]),
                    CountryName = row["CountryName"].ToString()
                };
                countryList.Add(loc_CountryDropDownModel);
			}
			ViewBag.CountryList = countryList;
			#endregion
			#region Add
			SqlConnection connection = new(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "PR_State_SelectByPK";
			command.Parameters.AddWithValue("@StateID", StateID);
			SqlDataReader reader = command.ExecuteReader();
			DataTable table = new();
			table.Load(reader);
			LOC_StateModel loc_StateModel = new();
			foreach (DataRow dataRow in table.Rows)
			{
                loc_StateModel.StateID = Convert.ToInt32(dataRow["StateID"]);
                loc_StateModel.StateName = dataRow["StateName"].ToString();
                loc_StateModel.StateCode = dataRow["StateCode"].ToString();
				loc_StateModel.CountryID = Convert.ToInt32(dataRow["CountryID"]);
			}
			return View("LOC_StateAddEdit", loc_StateModel);
			#endregion
		}
		#endregion

		#region Save_LOC_State
		[HttpPost]
		public IActionResult LOC_StateSave(LOC_StateModel lOC_StateModel, int StateID = 0)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection = new(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			if (StateID == 0)
			{
				command.CommandText = "PR_State_Insert";
			}
			else
			{
				command.CommandText = "PR_State_UpdateByPK";
				command.Parameters.AddWithValue("@StateID", StateID);
			}
			command.Parameters.AddWithValue("@StateName", lOC_StateModel.StateName);
			command.Parameters.AddWithValue("@StateCode", lOC_StateModel.StateCode);
			command.Parameters.AddWithValue("@CountryID", lOC_StateModel.CountryID);
			command.ExecuteNonQuery();
			connection.Close();
			return RedirectToAction("LOC_StateList");
		}
		#endregion

		#region Delete_LOC_State
		public IActionResult LOC_StateDelete(int StateID)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection = new(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "PR_State_DeleteByPK";
			command.Parameters.AddWithValue("@StateID", StateID);
			command.ExecuteNonQuery();
			connection.Close();
			return RedirectToAction("LOC_StateList");
		}
		#endregion

		#region Filter_LOC_State
		public IActionResult LOC_StateFilter(LOC_StateFilterModel loc_StateFilterModel)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection = new(connectionString);
			DataTable table = new();
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "PR_StateFilter";
			command.Parameters.AddWithValue("@CountryID", loc_StateFilterModel.CountryID);
			command.Parameters.AddWithValue("@StateName", loc_StateFilterModel.StateName);
			command.Parameters.AddWithValue("@StateCode", loc_StateFilterModel.StateCode);
			SqlDataReader reader = command.ExecuteReader();
			table.Load(reader);
			ModelState.Clear();

			#region Country ComboBox
			SqlConnection connection1 = new(connectionString);
			connection1.Open();
			SqlCommand command1 = connection1.CreateCommand();
			command1.CommandType = CommandType.StoredProcedure;
			command1.CommandText = "PR_Country_IDName";
			SqlDataReader reader1 = command1.ExecuteReader();
			DataTable table1 = new();
			table1.Load(reader1);
			connection1.Close();

			List<LOC_CountryDropDownModel> countryDropDownList = new();
			foreach (DataRow row in table1.Rows)
			{
                LOC_CountryDropDownModel loc_CountryDropDownModel = new()
                {
                    CountryID = Convert.ToInt32(row["CountryID"]),
                    CountryName = row["CountryName"].ToString()
                };
                countryDropDownList.Add(loc_CountryDropDownModel);
			}
			ViewBag.CountryList = countryDropDownList;
			#endregion


			return View("LOC_StateList", table);
		}
		#endregion
	}
}
