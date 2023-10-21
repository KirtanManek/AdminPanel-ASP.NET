using AdminPanel.Areas.LOC_Country.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace AdminPanel.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("{conntroller}/{action}")]
    public class LOC_CountryController : Controller
    {
        private readonly IConfiguration Configuration;
        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region Get_LOC_CountryList
        public IActionResult LOC_CountryList()
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new();
            SqlConnection conn = new(connectionstr);
            conn.Open();
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Country_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_CountryList", dt);
        }
        #endregion

        #region AddEdit_LOC_Country
        public IActionResult LOC_CountryAddEdit(int CountryID = 0)
        {
            string connectionString = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Country_SelectByPK";
            command.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new();
            table.Load(reader);
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
            SqlConnection connection = new(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            if (CountryID == 0)
            {
                command.CommandText = "PR_Country_Insert";
            }
            else
            {
                command.CommandText = "PR_Country_UpdateByPK";
                command.Parameters.AddWithValue("@CountryID", CountryID);
            }
            command.Parameters.AddWithValue("@CountryName", loc_CountryModel.CountryName);
            command.Parameters.AddWithValue("@CountryCode", loc_CountryModel.CountryCode);
			command.Parameters.AddWithValue("@Modified", DateTime.Now);
			command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("LOC_CountryList");
        }
        #endregion

        #region Delete_LOC_Country
        public IActionResult LOC_CountryDelete(int CountryID)
        {
            string connectionString = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Country_DeleteByPK";
            command.Parameters.AddWithValue("@CountryID", CountryID);
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("LOC_CountryList");
        }
		#endregion

		#region Filter_LOC_Country
		public IActionResult LOC_CountryFilter(string CountryData = "")
		{
			string connectionString = this.Configuration.GetConnectionString("myConnectionString");
			SqlConnection connection = new(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "PR_CountryFilter";
			if (CountryData != "") command.Parameters.AddWithValue("@CountryData", CountryData);
			SqlDataReader reader = command.ExecuteReader();
			DataTable LOC_CountryFilter_Table = new();
			LOC_CountryFilter_Table.Load(reader);
			//ModelState.Clear();
			return View("LOC_CountryList", LOC_CountryFilter_Table);
		}
		#endregion
	}
}
