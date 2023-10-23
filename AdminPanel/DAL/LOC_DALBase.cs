using AdminPanel.Areas.LOC_City.Models;
using AdminPanel.Areas.LOC_Country.Models;
using AdminPanel.Areas.LOC_State.Models;
using System.Data;
using System.Data.SqlClient;

namespace AdminPanel.DAL
{
	public class LOC_DALBase
	{
		#region LOC_Select
		public DataTable LOC_Select(string connectionString, string areaName, int ID = 0)
		{
			DataTable dt = new();
			try
			{
				using SqlConnection connection = new(connectionString);
				connection.Open();
				SqlCommand command = connection.CreateCommand();
				command.CommandType = CommandType.StoredProcedure;
				if (ID == 0)
				{
					command.CommandText = $"PR_{areaName}_SelectAll";
				}
				else
				{
					command.CommandText = $"PR_{areaName}_SelectByPK";
					command.Parameters.AddWithValue($"@{areaName}ID", ID);
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

		#region LOC_DeleteByPK
		public void LOC_DeleteByPK(string connectionString, string areaName, int ID)
		{
			try
			{
				using SqlConnection connection = new(connectionString);
				connection.Open();
				using SqlCommand command = new($"PR_{areaName}_DeleteByPK", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddWithValue($"@{areaName}ID", ID);
				command.ExecuteNonQuery();
			}
			catch (Exception)
			{
				throw;
			}
		}
		#endregion

		#region LOC_InsertUpdate
		public void LOC_InsertUpdate<T>(string connectionString, string areaName, T insertUpdateModel, int ID = 0)
		{
			using SqlConnection connection = new(connectionString);
			connection.Open();
			using SqlCommand command = connection.CreateCommand();
			try
			{
				command.CommandType = CommandType.StoredProcedure;
				if (ID == 0)
				{
					command.CommandText = $"PR_{areaName}_Insert";
				}
				else
				{
					command.CommandText = $"PR_{areaName}_UpdateByPK";
					command.Parameters.AddWithValue($"@{areaName}ID", ID);
					if (insertUpdateModel is LOC_CityModel cityModel)
					{
						command.Parameters.AddWithValue("@CityName", cityModel.CityName);
						command.Parameters.AddWithValue("@CityCode", cityModel.CityCode);
						command.Parameters.AddWithValue("@StateID", cityModel.StateID);
						command.Parameters.AddWithValue("@CountryID", cityModel.CountryID);
					}
					else if (insertUpdateModel is LOC_StateModel stateModel)
					{
						command.Parameters.AddWithValue("@StateName", stateModel.StateName);
						command.Parameters.AddWithValue("@StateCode", stateModel.StateCode);
						command.Parameters.AddWithValue("@CountryID", stateModel.CountryID);
					}
					else if (insertUpdateModel is LOC_CountryModel countryModel)
					{
						command.Parameters.AddWithValue("@CountryName", countryModel.CountryName);
						command.Parameters.AddWithValue("@CountryCode", countryModel.CountryCode);
						command.Parameters.AddWithValue("@Modified", DateTime.Now);
					}
				}
				command.ExecuteNonQuery();
			}
			catch (SqlException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}
		#endregion

		#region GetCountry
		public List<LOC_CountryDropDownModel> GetCountry(string connectionString)
		{
			List<LOC_CountryDropDownModel> CountryDropDownList = new();
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
			catch (Exception)
			{
				return new List<LOC_CountryDropDownModel>();
			}
			return CountryDropDownList;
		}
		#endregion

		#region GetState
		public List<LOC_StateDropDownModel> GetState(string connectionString, int CountryID = 0)
		{
			List<LOC_StateDropDownModel> StateDropDownList = new();

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
