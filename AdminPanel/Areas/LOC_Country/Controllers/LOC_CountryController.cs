using AdminPanel.Areas.LOC_Country.Models;
using AdminPanel.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace AdminPanel.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("{conntroller}/{action}")]
    public class LOC_CountryController : Controller
    {
        private readonly string areaName = "Country";
        private readonly IConfiguration Configuration;
        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region LOC_CountryList
        public IActionResult LOC_CountryList()
        {
            string connectionString = this.Configuration.GetConnectionString("myConnectionString");
            LOC_DAL loc_DAL = new();
            DataTable dt = loc_DAL.LOC_Select(connectionString, areaName);
            return View("LOC_CountryList", dt);
        }
		#endregion

		#region LOC_CountryAddEdit
		public IActionResult LOC_CountryAddEdit(int CountryID = 0)
        {
            string connectionString = this.Configuration.GetConnectionString("myConnectionString");
            LOC_DAL loc_DAL = new();
            if(CountryID == 0)
            {
				return View("LOC_CountryAddEdit", new LOC_CountryModel());
			}
            DataTable table = loc_DAL.LOC_Select(connectionString, areaName, CountryID);
            LOC_CountryModel loc_CountryModel = new();
            foreach (DataRow dataRow in table.Rows)
            {
				loc_CountryModel.CountryID = Convert.ToInt32(dataRow["CountryID"]);
				loc_CountryModel.CountryName = dataRow["CountryName"].ToString();
				loc_CountryModel.CountryCode = dataRow["CountryCode"].ToString();
            }
            return View("LOC_CountryAddEdit", loc_CountryModel);
        }
        #endregion

        #region Save_LOC_Country
        public IActionResult LOC_CountrySave(LOC_CountryModel loc_CountryModel, int CountryID = 0)
        {
            string connectionString = this.Configuration.GetConnectionString("myConnectionString");
            LOC_DAL loc_DAL = new();
            loc_DAL.LOC_InsertUpdate(connectionString, areaName, loc_CountryModel, CountryID);
            return RedirectToAction("LOC_CountryList");
        }
        #endregion

        #region Delete_LOC_Country
        public IActionResult LOC_CountryDelete(int CountryID)
        {
            string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			LOC_DAL loc_DAL = new();
            loc_DAL.LOC_DeleteByPK(connectionString, areaName, CountryID);
            return RedirectToAction("LOC_CountryList");
        }
		#endregion

		#region Filter_LOC_Country
		public IActionResult LOC_CountryFilter(LOC_CountryFilterModel loc_CountryFilterModel)
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			LOC_DAL loc_DAL = new();
			DataTable dt = loc_DAL.LOC_Filter(connectionString, areaName, loc_CountryFilterModel);
            ModelState.Clear();
            return View("LOC_CountryList", dt);
		}
		#endregion
	}
}
