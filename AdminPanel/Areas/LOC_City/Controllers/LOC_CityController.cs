﻿using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AdminPanel.Areas.LOC_City.Models;
using AdminPanel.Areas.LOC_Country.Models;
using AdminPanel.Areas.LOC_State.Models;

namespace AdminPanel.Areas.LOC_City.Controllers
{
	[Area("LOC_City")]
	[Route("{conntroller}/{action}")]
	public class LOC_CityController : Controller
	{
		private readonly IConfiguration Configuration;
		public LOC_CityController(IConfiguration _configuration)
		{
			Configuration = _configuration;
		}

		#region Get_LOC_CityList
		public IActionResult LOC_CityList()
		{
			string connectionstr = this.Configuration.GetConnectionString("myConnectionString");
			//Prepare a connection
			DataTable dt = new DataTable();
			SqlConnection conn = new SqlConnection(connectionstr);
			conn.Open();
			SqlCommand objCmd = conn.CreateCommand();
			objCmd.CommandType = CommandType.StoredProcedure;
			objCmd.CommandText = "PR_City_SelectAll";
			SqlDataReader objSDR = objCmd.ExecuteReader();
			dt.Load(objSDR);
			conn.Close();
			return View("LOC_CityList", dt);
		}
		#endregion

		#region Delete_LOC_City
		public IActionResult LOC_CityDelete(int CityID)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "PR_City_DeleteByPK";
			command.Parameters.AddWithValue("@CityID", CityID);
			command.ExecuteNonQuery();
			connection.Close();
			return RedirectToAction("LOC_CityList");
		}
		#endregion

		#region Save_LOC_City
		public IActionResult LOC_CitySave(LOC_CityModel lOC_CityModel, int CityID = 0)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			if (CityID == 0)
			{
				command.CommandText = "PR_City_Insert";
			}
			else
			{
				command.CommandText = "PR_City_UpdateByPK";
				command.Parameters.AddWithValue("@CityID", lOC_CityModel.CityID);
			}
			command.Parameters.AddWithValue("@CityName", lOC_CityModel.CityName);
			command.Parameters.AddWithValue("@CityCode", lOC_CityModel.CityCode);
			command.Parameters.AddWithValue("@StateID", lOC_CityModel.StateID);
			command.Parameters.AddWithValue("@CountryID", lOC_CityModel.CountryID);
			command.ExecuteNonQuery();
			connection.Close();
			return RedirectToAction("LOC_CityList");
		}
		#endregion

		#region AddEdit_LOC_City
		public IActionResult LOC_CityAddEdit(int CityID = 0)
		{
			#region State ComboBox
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection1 = new SqlConnection(connectionString);
			connection1.Open();
			SqlCommand command1 = connection1.CreateCommand();
			command1.CommandType = CommandType.StoredProcedure;
			command1.CommandText = "PR_State_IDName";
			SqlDataReader reader1 = command1.ExecuteReader();
			DataTable table1 = new DataTable();
			table1.Load(reader1);
			connection1.Close();

			List<LOC_StateDropDownModel> list = new List<LOC_StateDropDownModel>();
			foreach (DataRow row in table1.Rows)
			{
				LOC_StateDropDownModel lOC_StateDropDownModel = new LOC_StateDropDownModel();
				lOC_StateDropDownModel.StateID = Convert.ToInt32(row["StateID"]);
				lOC_StateDropDownModel.StateName = row["StateName"].ToString();
				list.Add(lOC_StateDropDownModel);
			}
			ViewBag.StateList = list;
			#endregion
			#region Country ComboBox
			SqlConnection connection2 = new SqlConnection(connectionString);
			connection2.Open();
			SqlCommand command2 = connection2.CreateCommand();
			command2.CommandType = CommandType.StoredProcedure;
			command2.CommandText = "PR_Country_IDName";
			SqlDataReader reader2 = command2.ExecuteReader();
			DataTable table2 = new DataTable();
			table2.Load(reader2);
			connection2.Close();

			List<LOC_CountryDropDownModel> list2 = new List<LOC_CountryDropDownModel>();
			foreach (DataRow row in table2.Rows)
			{
				LOC_CountryDropDownModel lOC_CountryDropDownModel = new LOC_CountryDropDownModel();
				lOC_CountryDropDownModel.CountryID = Convert.ToInt32(row["CountryID"]);
				lOC_CountryDropDownModel.CountryName = row["CountryName"].ToString();
				list2.Add(lOC_CountryDropDownModel);
			}
			ViewBag.CountryList = list2;
			#endregion
			#region Add
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "PR_City_SelectByPK";
			command.Parameters.AddWithValue("@CityID", CityID);
			SqlDataReader reader = command.ExecuteReader();
			DataTable table = new DataTable();
			table.Load(reader);
			LOC_CityModel lOC_CityModel = new LOC_CityModel();
			foreach (DataRow dataRow in table.Rows)
			{
				lOC_CityModel.CityID = Convert.ToInt32(dataRow["CityID"]);
				lOC_CityModel.CityName = dataRow["CityName"].ToString();
				lOC_CityModel.CityCode = dataRow["CityCode"].ToString();
				lOC_CityModel.StateID = Convert.ToInt32(dataRow["StateID"]);
				lOC_CityModel.CountryID = Convert.ToInt32(dataRow["CountryID"]);
			}
			return View("LOC_CityAddEdit", lOC_CityModel);
			#endregion
		}
		#endregion
	}
}