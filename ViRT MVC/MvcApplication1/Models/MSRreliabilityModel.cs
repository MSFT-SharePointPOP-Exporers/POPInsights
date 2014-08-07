using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MvcApplication1.Models
{
	public class MSRreliability
    {
		//Connection string for DB queries
		private SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["ViRTConnection"].ConnectionString);

		/// <summary>
		/// Constructor for a MSR_Reliability Object
		/// </summary>
		public MSRreliability()
		{

		}

		/// <summary>
		/// Calculates the reliability percentage for a given month
		/// </summary>
		/// <param name="monthYear">The month to calculate the percent reliability</param>
		/// <returns>Percent reliability</returns>
		public decimal MonthReliabilityPercent(DateTime monthYear)
		{
			dbConnect.Open();
			DateTime start = monthYear;
			DateTime end = (monthYear.AddMonths(1)).AddDays(-1);

			String queryTags = "SELECT ((SUM(STS_SuccessfulTokenIssuances) + SUM(STS_UserErrors)) / (SUM(STS_SuccessfulTokenIssuances) + SUM(STS_FailedTokenIssuances) + 0.0000000001)) * 100 " +
				"FROM Prod_MSR_Reliability WHERE Date BETWEEN '" + start.ToString() + "' AND '" + end.ToString() + "'";
			SqlCommand queryCommand = new SqlCommand(queryTags, dbConnect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();
			DataTable tagTable = new DataTable();
			tagTable.Load(queryCommandReader);

			dbConnect.Close();


			if (tagTable.Rows[0][0] == null) return 0;

			return (Decimal)tagTable.Rows[0][0];
		}


		/// <summary>
		/// Creates a data table with daily data for a specified month
		/// If no data is available for a time period, there are no entires
		/// in the table
		/// </summary>
		/// <param name="monthYear">Month and Year which the data will represent</param>
		/// <returns>DataTable with columns Date and Percent for the month</returns>
		public DataTable ReliaiblityDailyTable(DateTime monthYear)
		{
			dbConnect.Open();
			DateTime start = monthYear;
			DateTime end = (monthYear.AddMonths(1)).AddDays(-1);

			String queryTags = "SELECT Date, ((SUM(STS_SuccessfulTokenIssuances) + SUM(STS_UserErrors)) / (SUM(STS_SuccessfulTokenIssuances) + SUM(STS_FailedTokenIssuances) + 0.0000000001)) *100 AS [Percent] " +
				"FROM Prod_MSR_Reliability WHERE Date BETWEEN '" + start.ToString() + "' AND '" + end.ToString() + "' GROUP BY Date";
			SqlCommand queryCommand = new SqlCommand(queryTags, dbConnect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();
			DataTable tagTable = new DataTable();
			tagTable.Load(queryCommandReader);

			dbConnect.Close();

			return tagTable;
		}
	}
}
