using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using TaskApp.DataBase;
using TaskApp.Models;

namespace TaskApp.Repositary
{
    public class Data : IData
    {
        public string connectionstring { get; set; }
        private readonly AppDbContext _context;
        public  Data(AppDbContext context)
        {
            _context = context;

            connectionstring = _context.Database.GetConnectionString();
        }
        public TaskDetals CreateTask(TaskDetals taskdata)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand("spt_save_taskdata", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Title", taskdata.Title);
                        command.Parameters.AddWithValue("@TaskDescription", taskdata.TaskDescription);
                        command.Parameters.AddWithValue("@DueDate", taskdata.DueDate);

                        SqlParameter outputparm = new SqlParameter();
                        outputparm.DbType = DbType.Int32;
                        outputparm.Direction = ParameterDirection.Output;
                        outputparm.ParameterName = "@Id";
                        command.Parameters.Add(outputparm);

                        command.Transaction = transaction;
                        command.ExecuteNonQuery();

                        int id = Convert.ToInt32(outputparm.Value);
                        taskdata.Id = id;

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }

            return taskdata;
        
    }
    public  List<TaskDetals> GetAllTask()
        {
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            SqlCommand command = new SqlCommand("spt_getAllTaskDetails", connection);
           using SqlDataReader reader = command.ExecuteReader();
            List<TaskDetals> data = new List<TaskDetals>(); 
            command.CommandType = CommandType.StoredProcedure;
            try
            {
                TaskDetals taskdata = new TaskDetals();
                while (reader.Read())
                {
                    taskdata.Id = int.Parse(reader["id"].ToString());
                    taskdata.Title = reader["Title"].ToString();
                    taskdata.TaskDescription = reader["TaskDescription"].ToString();
                   // taskdata.DueDate = reader.GetDateTime(["Due"]);
                   data.Add(taskdata);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string CalculateEndDate(DateTime startDate, int workingDays)
        {
            string jsonResult = string.Empty;
            try
            {
                string taskStartDate = startDate.ToString("dd/MM/yyyy");
                DateTime splitedTaskStartDate = DateTime.ParseExact(taskStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                DateTime modifiedTaskStartDate = new DateTime(splitedTaskStartDate.Year, splitedTaskStartDate.Month, splitedTaskStartDate.Day);
                List<DateTime> holidayDates = new List<DateTime>
                {
                new DateTime(2022, 8, 23)
                 };

                int totalDays = 0;
                while (workingDays > 0)
                {
                    modifiedTaskStartDate = modifiedTaskStartDate.AddDays(1);
                    if (modifiedTaskStartDate.DayOfWeek != DayOfWeek.Saturday && modifiedTaskStartDate.DayOfWeek != DayOfWeek.Sunday && !holidayDates.Contains(startDate.Date))
                    {
                        workingDays--;
                    }
                    totalDays++;
                }
                var result = new { endDate = modifiedTaskStartDate.ToString("dd/MM/yyyy") };
                jsonResult = JsonConvert.SerializeObject(result);
            }
            catch (Exception)
            {
                return "Error: Unable to calculate the end date.";
            }
            return jsonResult;
        }
    }
}
