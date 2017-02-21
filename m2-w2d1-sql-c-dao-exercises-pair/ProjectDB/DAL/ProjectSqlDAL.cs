using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProjectDB.DAL
{
    public class ProjectSqlDAL
    {
        private string connectionString;
        private const string SQL_Projects = @"select * from project";
        private const string SQL_InsertProjects = @"insert into project_employee values (@projId, @empId);";
        private const string SQL_InsertNewProject =
                                @"insert into project values(@projectName,@fromDate, @toDate)";
        private const string SQL_DeleteProjects = @"delete from project_employee where @empId = employee_id and @projId = project_id";

        // Single Parameter Constructor
        public ProjectSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Project> GetAllProjects()
        {
            List<Project> listOfProjects = new List<Project>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_Projects, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Project project = new Project();
                        project.ProjectId = Convert.ToInt32(reader["project_id"]);
                        project.Name = Convert.ToString(reader["name"]);
                        project.StartDate = Convert.ToDateTime(reader["from_date"]);
                        project.EndDate = Convert.ToDateTime(reader["to_date"]);

                        listOfProjects.Add(project);

                    }
                }
            }
            catch (SqlException ex)
            {
                //Log and throw the exception
                throw ex;
            }

            return listOfProjects;
        }

        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_InsertProjects, conn);
                    
                    cmd.Parameters.AddWithValue("@projId", projectId);
                    cmd.Parameters.AddWithValue("@empId", employeeId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return (rowsAffected > 0); //true if one row was affected
                }
            }
            catch (SqlException ex)
            {
                //log
                throw;
            }
        }

        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_DeleteProjects, conn);

                    cmd.Parameters.AddWithValue("@projId", projectId);
                    cmd.Parameters.AddWithValue("@empId", employeeId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return (rowsAffected > 0); //true if one row was affected
                }
            }
            catch (SqlException ex)
            {
                //log
                throw;
            }
        }

        public bool CreateProject(Project newProject)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_InsertNewProject, conn);
                
                    cmd.Parameters.AddWithValue("@projectName", newProject.Name);
                    cmd.Parameters.AddWithValue("@fromDate", newProject.StartDate);
                    cmd.Parameters.AddWithValue("@toDate", newProject.EndDate);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return (rowsAffected > 0); //true if one row was affected
                }
            }
            catch (SqlException ex)
            {
                //log
                throw;
            }
        }

    }
}
