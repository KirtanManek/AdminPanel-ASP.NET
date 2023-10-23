using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AdminPanel.Areas.LOC_City.Models;
using AdminPanel.Areas.LOC_Country.Models;
using AdminPanel.Areas.LOC_State.Models;
using AdminPanel.DAL;

namespace AdminPanel.Areas.LOC_City.Controllers
{
	[Area("LOC_City")]
	[Route("{controller}/{action}")]
	public class LOC_CityController : Controller
	{
		private readonly string areaName = "City";
		private readonly IConfiguration Configuration;
		public LOC_CityController(IConfiguration _configuration)
		{
			Configuration = _configuration;
		}

		#region LOC_CityList
		public IActionResult LOC_CityList()
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			LOC_DAL loc_DAL = new();
			#region Country_ComboBox
			ViewBag.CountryList = loc_DAL.GetCountry(connectionString);
			#endregion
			#region State_Combo
			ViewBag.StateList = loc_DAL.GetState(connectionString);
			#endregion
			DataTable LOC_CityList_Table = loc_DAL.LOC_Select(connectionString, areaName);
			return View(LOC_CityList_Table);
		}
		#endregion

		#region LOC_CityDelete
		public IActionResult LOC_CityDelete(int CityID)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			LOC_DAL loc_DAL = new();
			loc_DAL.LOC_DeleteByPK(connectionString, areaName, CityID);
			return RedirectToAction("LOC_CityList");
		}
		#endregion

		#region LOC_CitySave
		[HttpPost]
		public IActionResult LOC_CitySave(LOC_CityModel loc_CityModel, int CityID = 0)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			LOC_DAL loc_DAL = new();
			loc_DAL.LOC_InsertUpdate(connectionString, areaName, loc_CityModel, CityID);
			return RedirectToAction("LOC_CityList");
		}
		#endregion

		#region LOC_CityAddEdit
		public IActionResult LOC_CityAddEdit(int CityID = 0)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			LOC_DAL loc_DAL = new();
			#region Country_ComboBox
			ViewBag.CountryList = loc_DAL.GetCountry(connectionString);
			#endregion
			#region Add
			if(CityID == 0)
			{
				#region State_ComboBox_WhenCityIDIsZero
				ViewBag.StateList = loc_DAL.GetState(connectionString);
				#endregion
				return View("LOC_CityAddEdit", new LOC_CityModel());
			}
			DataTable table = loc_DAL.LOC_Select(connectionString, areaName, CityID);
			LOC_CityModel loc_CityModel = new();
			foreach (DataRow dataRow in table.Rows)
			{
				loc_CityModel.CityID = Convert.ToInt32(dataRow["CityID"]);
				loc_CityModel.CityName = dataRow["CityName"].ToString();
				loc_CityModel.CityCode = dataRow["CityCode"].ToString();
				loc_CityModel.StateID = Convert.ToInt32(dataRow["StateID"]);
				loc_CityModel.CountryID = Convert.ToInt32(dataRow["CountryID"]);
			}
			#region State_ComboBox_WhenCityIDIsNotZero
			ViewBag.StateList = loc_DAL.GetState(connectionString, loc_CityModel.CountryID);
			#endregion
			return View("LOC_CityAddEdit", loc_CityModel);
			#endregion
		}
		#endregion

		#region DropDownByCountry
		public IActionResult DropDownByCountry(int CountryID)
		{
			LOC_DAL loc_DAL = new();
			var vModel = loc_DAL.GetState(this.Configuration.GetConnectionString("myConnectionString"), CountryID);
			return Json(vModel);
		}
		#endregion

		#region Filter
		public IActionResult LOC_CityFilter(LOC_CityFilterModel loc_CityFilterModel)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			LOC_DAL loc_DAL = new();
			#region Country_ComboBox
			ViewBag.CountryList = loc_DAL.GetCountry(connectionString);
			#endregion
			#region State_ComboBox
			ViewBag.StateList = loc_DAL.GetState(connectionString);
			#endregion
			DataTable LOC_CityFilter_Table = loc_DAL.LOC_Filter(connectionString, areaName, loc_CityFilterModel);
			ModelState.Clear();
			return View("LOC_CityList", LOC_CityFilter_Table);
		}
		#endregion
	}
}
