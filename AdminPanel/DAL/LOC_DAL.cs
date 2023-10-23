using AdminPanel.Areas.LOC_City.Models;
using AdminPanel.Areas.LOC_Country.Models;
using AdminPanel.Areas.LOC_State.Models;
using System.Data;
using System.Data.SqlClient;

namespace AdminPanel.DAL
{
	public class LOC_DAL : LOC_DALBase
	{
		#region LOC_Filter
		public DataTable LOC_Filter<T>(string connectionString, string areaName, T filterModel)
		{
			DataTable dt = new();
			try
			{
				using SqlConnection connection = new(connectionString);
				connection.Open();
				using SqlCommand command = connection.CreateCommand();
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = $"PR_{areaName}Filter";
				if (filterModel is LOC_CityFilterModel cityModel)
				{
					command.Parameters.AddWithValue("@CountryID", cityModel.CountryID);
					command.Parameters.AddWithValue("@StateID", cityModel.StateID);
					command.Parameters.AddWithValue("@CityName", cityModel.CityName);
					command.Parameters.AddWithValue("@CityCode", cityModel.CityCode);
				}
				else if (filterModel is LOC_StateFilterModel stateModel)
				{
					command.Parameters.AddWithValue("@CountryID", stateModel.CountryID);
					command.Parameters.AddWithValue("@StateName", stateModel.StateName);
					command.Parameters.AddWithValue("@StateCode", stateModel.StateCode);
				}
				else if (filterModel is LOC_CountryFilterModel countryModel)
				{
					command.Parameters.AddWithValue("@CountryName", countryModel.CountryName);
					command.Parameters.AddWithValue("@CountryCode", countryModel.CountryCode);
				}
				using SqlDataReader reader = command.ExecuteReader();
				dt.Load(reader);
			}
			catch (Exception)
			{
				throw;
			}

			return dt;
		}
		#endregion
	}
}
