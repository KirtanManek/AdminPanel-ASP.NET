using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AdminPanel.Areas.LOC_City.Models;
using AdminPanel.Areas.LOC_Country.Models;
using AdminPanel.Areas.LOC_State.Models;

namespace AdminPanel.Areas.LOC_City.Controllers
{
	[Area("LOC_City")]
	[Route("{controller}/{action}")]
	public class LOC_CityController : Controller
	{
		private readonly IConfiguration Configuration;
		public LOC_CityController(IConfiguration _configuration)
		{
			Configuration = _configuration;
		}

		#region LOC_CityList
		public IActionResult LOC_CityList()
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");

			#region Country_ComboBox
			ViewBag.CountryList = GetCountry();
			#endregion

			#region State_Combo
			ViewBag.StateList = GetState();
			#endregion

			SqlConnection LOC_CityList_Connection = new(connectionString);
			LOC_CityList_Connection.Open();
			SqlCommand LOC_CityList_Command = LOC_CityList_Connection.CreateCommand();
			LOC_CityList_Command.CommandType = CommandType.StoredProcedure;
			LOC_CityList_Command.CommandText = "PR_City_SelectAll";
			SqlDataReader reader = LOC_CityList_Command.ExecuteReader();
			DataTable LOC_CityList_Table = new();
			LOC_CityList_Table.Load(reader);
			LOC_CityList_Connection.Close();
			return View(LOC_CityList_Table);
		}
		#endregion

		#region LOC_CityDelete
		public IActionResult LOC_CityDelete(int CityID)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection LOC_CityDelete_Connection = new(connectionString);
			LOC_CityDelete_Connection.Open();
			SqlCommand LOC_CityDelete_Command = LOC_CityDelete_Connection.CreateCommand();
			LOC_CityDelete_Command.CommandType = CommandType.StoredProcedure;
			LOC_CityDelete_Command.CommandText = "PR_City_DeleteByPK";
			LOC_CityDelete_Command.Parameters.AddWithValue("@CityID", CityID);
			LOC_CityDelete_Command.ExecuteNonQuery();
			LOC_CityDelete_Connection.Close();
			return RedirectToAction("LOC_CityList");
		}
		#endregion

		#region LOC_CitySave
		[HttpPost]
		public IActionResult LOC_CitySave(LOC_CityModel loc_CityModel, int CityID = 0)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection LOC_CitySave_Connection = new(connectionString);
			LOC_CitySave_Connection.Open();
			SqlCommand LOC_CitySave_Command = LOC_CitySave_Connection.CreateCommand();
			LOC_CitySave_Command.CommandType = CommandType.StoredProcedure;
			if (CityID == 0)
			{
				LOC_CitySave_Command.CommandText = "PR_City_Insert";
			}
			else
			{
				LOC_CitySave_Command.CommandText = "PR_City_UpdateByPK";
				LOC_CitySave_Command.Parameters.AddWithValue("@CityID", loc_CityModel.CityID);
			}
			LOC_CitySave_Command.Parameters.AddWithValue("@CityName", loc_CityModel.CityName);
			LOC_CitySave_Command.Parameters.AddWithValue("@CityCode", loc_CityModel.CityCode);
			LOC_CitySave_Command.Parameters.AddWithValue("@StateID", loc_CityModel.StateID);
			LOC_CitySave_Command.Parameters.AddWithValue("@CountryID", loc_CityModel.CountryID);
			LOC_CitySave_Command.ExecuteNonQuery();
			LOC_CitySave_Connection.Close();
			return RedirectToAction("LOC_CityList");
		}
		#endregion

		#region LOC_CityAddEdit
		public IActionResult LOC_CityAddEdit(int CityID = 0)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");

			#region Country_ComboBox
			ViewBag.CountryList = GetCountry();
			#endregion

			#region Add
			SqlConnection connection = new(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "PR_City_SelectByPK";
			command.Parameters.AddWithValue("@CityID", CityID);
			SqlDataReader reader = command.ExecuteReader();
			DataTable table = new();
			table.Load(reader);
			LOC_CityModel loc_CityModel = new();
			foreach (DataRow dataRow in table.Rows)
			{
				loc_CityModel.CityID = Convert.ToInt32(dataRow["CityID"]);
				loc_CityModel.CityName = dataRow["CityName"].ToString();
				loc_CityModel.CityCode = dataRow["CityCode"].ToString();
				loc_CityModel.StateID = Convert.ToInt32(dataRow["StateID"]);
				loc_CityModel.CountryID = Convert.ToInt32(dataRow["CountryID"]);
			}

			#region State_ComboBox
			ViewBag.StateList = GetState(loc_CityModel.CountryID);
			#endregion

			return View("LOC_CityAddEdit", loc_CityModel);
			#endregion
		}
		#endregion

		#region DropDownByCountry
		public IActionResult DropDownByCountry(int CountryID)
		{
			Console.WriteLine(CountryID);
			var vModel = GetState(CountryID);
			return Json(vModel);
		}
		#endregion

		#region Filter
		public IActionResult LOC_CityFilter(LOC_CityFilterModel loc_CityFilterModel)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");

			#region Country_ComboBox
			ViewBag.CountryList = GetCountry();
			#endregion

			#region State_ComboBox
			ViewBag.StateList = GetState();
			#endregion

			DataTable LOC_CityFilter_Table = new();
			SqlConnection LOC_CityFilter_Connection = new(connectionString);
			LOC_CityFilter_Connection.Open();
			SqlCommand LOC_CityFilter_Command = LOC_CityFilter_Connection.CreateCommand();
			LOC_CityFilter_Command.CommandType = CommandType.StoredProcedure;
			LOC_CityFilter_Command.CommandText = "PR_CityFilter";
			LOC_CityFilter_Command.Parameters.AddWithValue("@CountryID", loc_CityFilterModel.CountryID);
			LOC_CityFilter_Command.Parameters.AddWithValue("@StateID", loc_CityFilterModel.StateID);
			LOC_CityFilter_Command.Parameters.AddWithValue("@CityName", loc_CityFilterModel.CityName);
			LOC_CityFilter_Command.Parameters.AddWithValue("@CityCode", loc_CityFilterModel.CityCode);
			SqlDataReader City_Filter_Reader = LOC_CityFilter_Command.ExecuteReader();
			LOC_CityFilter_Table.Load(City_Filter_Reader);

			ModelState.Clear();
			return View("LOC_CityList", LOC_CityFilter_Table);
		}
		#endregion

		#region GetCountry
		public List<LOC_CountryDropDownModel> GetCountry()
		{
			List<LOC_CountryDropDownModel> CountryDropDownList = new();
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			try
			{
				using SqlConnection GetCountry_Connection = new(connectionString);
				GetCountry_Connection.Open();

				using SqlCommand GetCountry_Command = GetCountry_Connection.CreateCommand();
				GetCountry_Command.CommandType = CommandType.StoredProcedure;

				GetCountry_Command.CommandText = "PR_Country_IDName";

				using SqlDataReader reader = GetCountry_Command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						LOC_CountryDropDownModel loc_CountryDropDownModel = new()
						{
							CountryID = Convert.ToInt32(reader["CountryID"]),
							CountryName = reader["CountryName"].ToString()
						};
						CountryDropDownList.Add(loc_CountryDropDownModel);
					}
				}
			}
			catch(Exception)
			{
				return new List<LOC_CountryDropDownModel>();
			}
			return CountryDropDownList;
		}
		#endregion

		#region GetState
		public List<LOC_StateDropDownModel> GetState(int CountryID = 0)
		{
			List<LOC_StateDropDownModel> StateDropDownList = new();

			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			try
			{
				using SqlConnection GetState_Connection = new(connectionString);
				GetState_Connection.Open();

				using SqlCommand GetState_Command = GetState_Connection.CreateCommand();
				GetState_Command.CommandType = CommandType.StoredProcedure;
				if (CountryID == 0)
				{
					GetState_Command.CommandText = "PR_State_IDName";
				}
				else
				{
					GetState_Command.CommandText = "PR_State_SelectComboBoxByCountry";
					GetState_Command.Parameters.Add(new SqlParameter("@CountryID", CountryID));
				}
				using SqlDataReader reader = GetState_Command.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						LOC_StateDropDownModel loc_StateDropDownModel = new()
						{
							StateID = Convert.ToInt32(reader["StateID"]),
							StateName = reader["StateName"].ToString()
						};
						StateDropDownList.Add(loc_StateDropDownModel);
					}
				}
			}
			catch (Exception)
			{
				return new List<LOC_StateDropDownModel>();
			}
			return StateDropDownList;
		}
		#endregion
	}
}
