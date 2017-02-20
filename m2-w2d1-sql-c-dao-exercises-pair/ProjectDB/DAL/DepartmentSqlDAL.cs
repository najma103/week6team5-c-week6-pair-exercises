using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProjectDB.DAL
{
    public class DepartmentSqlDAL
    {
        private string connectionString;
        private const string SQL_Departments = @"select * from department";
        private const string SQL_InsertDepartment = @"insert into department values (@name);";
        private const string SQL_UpdateDepartment = @"update department set name = @name where department_id = @dept_id;";

        // Single Parameter Constructor
        public DepartmentSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Department> GetDepartments()
        {
            List<Department> listOfDept = new List<Department>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_Departments, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Department dept = new Department();
                        dept.Id = Convert.ToInt32(reader["department_id"]);
                        dept.Name = Convert.ToString(reader["name"]);

                        listOfDept.Add(dept);
                       
                    }
                }
            }
            catch (SqlException ex)
            {
                //Log and throw the exception
                throw ex;
            }

            return listOfDept;
    }

        public bool CreateDepartment(Department newDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_InsertDepartment, conn);
                    //cmd.Parameters.AddWithValue("@id", newDepartment.Id);
                    cmd.Parameters.AddWithValue("@name", newDepartment.Name);

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

        public bool UpdateDepartment(Department updatedDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_UpdateDepartment, conn);
                    cmd.Parameters.AddWithValue("@dept_id", updatedDepartment.Id);
                    cmd.Parameters.AddWithValue("@name", updatedDepartment.Name);

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
