using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MvcApplication1.Models
{
    class MSRqualityModel
    {
		private SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["ViRTConnection"].ConnectionString);

		/// <summary>
		/// Empty QoS object
		/// </summary>
		public MSRqualityModel()
		{

		}

		/// <summary>
		/// Calculates the quality of service for a month
		/// </summary>
		/// <param name="monthYear">Specified month</param>
		/// <returns>Percent of QoS</returns>
		public decimal QualityOfService(DateTime monthYear)
		{
			dbConnect.Open();
			DateTime start = monthYear;
			DateTime end = (monthYear.AddMonths(1)).AddDays(-1);

			Decimal per = CalculateQoS(start, end);

			dbConnect.Close();
			return per;
		}

		/// <summary>
		/// Makes three queries to calculate totals and then calculate a single percentage
		/// Something is wrong with the calculation
		/// It is (S - Pf) / T however Pf is wrong...
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		private decimal CalculateQoS(DateTime start, DateTime end)
		{
			string query = "SELECT CAST(SUM(STS_SuccessfulTokenIssuances) AS decimal(38,0))" +
				" FROM Prod_MSR_Reliability" +
				" WHERE Date BETWEEN '" + start.ToString() + "' AND '" + end.ToString() + "'";
			SqlCommand queryCommand = new SqlCommand(query, dbConnect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();
			DataTable table = new DataTable();
			table.Load(queryCommandReader);

			if (table.Rows[0].IsNull(0)) return -1;
			Console.WriteLine(table.Rows[0][0]);
			decimal succ = (decimal)table.Rows[0][0];

			query = "SELECT CAST(SUM(TotalMonitoredScopesOverThreshold) AS decimal(38,0)) FROM Prod_PerformanceRaw WHERE Date BETWEEN '" + 
				start.ToString() + "' AND '" + end.ToString() + "'";
			queryCommand = new SqlCommand(query, dbConnect);
			queryCommandReader = queryCommand.ExecuteReader();
			table = new DataTable();
			table.Load(queryCommandReader);
			if (table.Rows[0].IsNull(0)) return -1;
			Console.WriteLine(table.Rows[0][0]);
			decimal over = (decimal)table.Rows[0][0];

			query = "SELECT CAST(SUM(STS_SuccessfulTokenIssuances + STS_FailedTokenIssuances + STS_UserErrors) AS decimal(38,0)) FROM Prod_MSR_Reliability WHERE Date BETWEEN '" + 
				start.ToString() + "' AND '" + end.ToString() + "'";
			queryCommand = new SqlCommand(query, dbConnect);
			queryCommandReader = queryCommand.ExecuteReader();
			table = new DataTable();
			table.Load(queryCommandReader);
			if (table.Rows[0].IsNull(0)) return -1;

			decimal total = (decimal)table.Rows[0][0];

			return ((succ - over) / total) * 100;
		}
    }
}
