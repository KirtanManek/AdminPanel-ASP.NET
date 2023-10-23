using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AdminPanel.Areas.LOC_State.Models;
using AdminPanel.Areas.LOC_Country.Models;
using AdminPanel.DAL;

namespace AdminPanel.Areas.LOC_State.Controllers
{
	[Area("LOC_State")]
	[Route("{conntroller}/{action}")]
	public class LOC_StateController : Controller
	{
		private readonly string areaName = "State";
		private readonly IConfiguration Configuration;
		public LOC_StateController(IConfiguration _configuration)
		{
			Configuration = _configuration;
		}

		#region LOC_StateList
		public IActionResult LOC_StateList()
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			LOC_DAL loc_DAL = new();
			#region Country_ComboBox
			ViewBag.CountryList = loc_DAL.GetCountry(connectionString);
			#endregion
			DataTable dt = loc_DAL.LOC_Select(connectionString, areaName);
			return View("LOC_StateList", dt);
		}
		#endregion

		#region LOC_StateAddEdit
		public IActionResult LOC_StateAddEdit(int StateID = 0)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			LOC_DAL loc_DAL = new();
			#region Country_ComboBox
			ViewBag.CountryList = loc_DAL.GetCountry(connectionString);
			#endregion
			#region Add
			if(StateID == 0)
			{
				return View("LOC_StateAddEdit", new LOC_StateModel());
			}
			DataTable table = loc_DAL.LOC_Select(connectionString, areaName, StateID);
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

		#region LOC_StateSave
		[HttpPost]
		public IActionResult LOC_StateSave(LOC_StateModel loc_StateModel, int StateID = 0)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			LOC_DAL loc_DAL = new();
			loc_DAL.LOC_InsertUpdate(connectionString, areaName, loc_StateModel, StateID);
			return RedirectToAction("LOC_StateList");
		}
		#endregion

		#region LOC_StateDelete
		public IActionResult LOC_StateDelete(int StateID)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			LOC_DAL loc_DAL = new();
			loc_DAL.LOC_DeleteByPK(connectionString, areaName, StateID);
			return RedirectToAction("LOC_StateList");
		}
		#endregion

		#region Filter_LOC_State
		public IActionResult LOC_StateFilter(LOC_StateFilterModel loc_StateFilterModel)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			LOC_DAL loc_DAL = new();
			DataTable table = loc_DAL.LOC_Filter(connectionString, areaName, loc_StateFilterModel);
			ModelState.Clear();
			#region Country_ComboBox
			ViewBag.CountryList = loc_DAL.GetCountry(connectionString);
			#endregion
			return View("LOC_StateList", table);
		}
		#endregion
	}
}
