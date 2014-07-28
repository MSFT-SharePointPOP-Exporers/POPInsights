using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MvcApplication1.Models
{
	class MSRperformance
	{
		private SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["ViRTConnection"].ConnectionString);

		/// <summary>
		/// Empty constructor
		/// </summary>
		public MSRperformance()
		{

		}

		/// <summary>
		/// Calculate the percentage of performance for the month
		/// </summary>
		/// <param name="monthYear">Month to get percent</param>
		/// <returns>Percent Perfomance</returns>
		public decimal PerformancePercent(DateTime monthYear)
		{
			dbConnect.Open();
			DateTime start = monthYear;
			DateTime end = (monthYear.AddMonths(1)).AddDays(-1);

			DataTable raw = GetPercent(start, end);

			if (raw.Rows[0].IsNull(0)) return 0;

			dbConnect.Close();
			return (decimal)raw.Rows[0][0];
		}

		/// <summary>
		/// Queries DB and calculates the percent
		/// </summary>
		/// <param name="start">Start date for pulling data</param>
		/// <param name="end">End fate for pulling data</param>
		/// <returns></returns>
		private DataTable GetPercent(DateTime start, DateTime end)
		{
			String query = "SELECT ((1 - CAST(CAST(SUM(TotalMonitoredScopesOverThreshold) AS decimal(38,0)) / SUM(TotalMonitoredScopes) AS decimal(7,4))) *100) FROM Prod_PerformanceRaw WHERE Date BETWEEN '" +
				start.ToString() + "' AND '" + end.ToString() + "'";
			SqlCommand queryCommand = new SqlCommand(query, dbConnect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();
			DataTable table = new DataTable();
			table.Load(queryCommandReader);

			return table;
		}

		/// <summary>
		/// Retrieves a table of percentages
		/// </summary>
		/// <param name="monthYear">Month to pull data for</param>
		/// <returns>DataTable with performance percent daily for a month</returns>
		public DataTable PerformancePercentDaily(DateTime monthYear)
		{
			dbConnect.Open();
			DateTime start = monthYear;
			DateTime end = (monthYear.AddMonths(1)).AddDays(-1);
			DataTable table = GetDays(start, end);
			dbConnect.Close();
			return table;
		}

		/// <summary>
		/// Gets the data for the days
		/// </summary>
		/// <param name="start">Start date</param>
		/// <param name="end">End Date</param>
		/// <returns>DT with daily performance percents</returns>
		private DataTable GetDays(DateTime start, DateTime end)
		{
			String query = "SELECT CAST(Date AS DATE) AS Date, ((1 - CAST(CAST(SUM(TotalMonitoredScopesOverThreshold) AS decimal(38,0)) / SUM(TotalMonitoredScopes) AS decimal(7,4))) *100)  AS Percentage " +
				"FROM Prod_PerformanceRaw WHERE Date BETWEEN '" + start.ToString() + "' AND '" + end.ToString() + "' GROUP BY CAST(Date AS DATE) ORDER BY Date";
			SqlCommand queryCommand = new SqlCommand(query, dbConnect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();
			DataTable table = new DataTable();
			table.Load(queryCommandReader);

			return table;
		}

		/// <summary>
		/// Calculates the 95th percentile for SPClaimsCounterScope for a complete month
		/// </summary>
		/// <param name="monthYear">Start date of the month to calculate 95th percentile</param>
		/// <returns>Integer with 95th percentile in milliseconds</returns>
		public int Percentile95th(DateTime monthYear)
		{
			dbConnect.Open();
			DateTime start = monthYear;
			DateTime end = (monthYear.AddMonths(1)).AddDays(-1);
			DataTable tileDaily = PercentileDailyTable(start, end);
			int time = Calculate95th(tileDaily);
			dbConnect.Close();
			return time;
		}

		/// <summary>
		/// Pulls data from DB and formats it into a date view
		/// </summary>
		/// <param name="start">Start date to pull</param>
		/// <param name="end">End date to include</param>
		/// <returns>DateTable with Date, Sample Count for day, and the sum of the 95th percentiles</returns>
		private DataTable PercentileDailyTable(DateTime start, DateTime end)
		{
			String query = "SELECT CAST(LogDate AS DATE) AS Date, SUM(SampleCount) AS SampleCount, SUM(SampleCount * ExecutionTimeMilliseconds_95) AS Sum95 FROM Prod_MonitoredScopes WHERE LogDate BETWEEN '" +
				start.ToString() + "' AND '" + end.ToString() + "' AND MonitoredScopeName = 'SPClaimsCounterScope' " + "GROUP BY CAST(LogDate AS DATE) ORDER BY Date";
			SqlCommand queryCommand = new SqlCommand(query, dbConnect);
			//queryCommand.CommandTimeout = 1000;
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();
			DataTable dateExecutionCount = new DataTable();
			dateExecutionCount.Load(queryCommandReader);

			return dateExecutionCount;
		}

		/// <summary>
		/// Gets the time in millisecond
		/// </summary>
		/// <param name="tileDaily">Table with daily data</param>
		/// <returns>95th percentile in milliseconds</returns>
		private int Calculate95th(DataTable tileDaily)
		{
			long total = 0;

			for (int i = 0; i < tileDaily.Rows.Count; i++)
			{
				total += (long)(tileDaily.Rows[i]["SampleCount"]);
			}

			int sum = 0;

			for (int i = 0; i < tileDaily.Rows.Count; i++)
			{
				sum += (int)((long)tileDaily.Rows[i]["Sum95"] / total);
			}

			return sum;
		}

		/// <summary>
		/// Creates a datatable which has the percentile for each day in the specified month
		/// </summary>
		/// <param name="monthYear">Start of the month to pull data from</param>
		/// <returns>Data table with Date and Percentile</returns>
		public DataTable Percentile95Table(DateTime monthYear)
		{
			dbConnect.Open();
			DateTime start = monthYear;
			DateTime end = (monthYear.AddMonths(1)).AddDays(-1);

			DataTable tileDaily = PercentileDailyTable(start, end);
			DataTable dailyPercentile = Format(tileDaily);

			dbConnect.Close();
			return dailyPercentile;
		}

		/// <summary>
		/// Formats the output for Percentile95
		/// </summary>
		/// <param name="tileDaily">Table to format</param>
		/// <returns>Formatted table with Date and Percentile</returns>
		private DataTable Format(DataTable tileDaily)
		{
			DataTable retTable = new DataTable();
			retTable.Columns.Add("Date", typeof(DateTime));
			retTable.Columns.Add("Percentile", typeof(int));
			DataRow toAdd = retTable.NewRow();

			for (int i = 0; i < tileDaily.Rows.Count; i++)
			{
				toAdd["Date"] = tileDaily.Rows[i]["Date"];
				toAdd["Percentile"] = (int)((long)tileDaily.Rows[i]["Sum95"] / (long)tileDaily.Rows[i]["SampleCount"]);
				retTable.Rows.Add(toAdd);
				toAdd = retTable.NewRow();
			}

			return retTable;
		}

	}
}
