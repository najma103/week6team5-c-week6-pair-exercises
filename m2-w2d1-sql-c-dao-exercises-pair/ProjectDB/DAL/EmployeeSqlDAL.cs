using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProjectDB.DAL
{
    public class EmployeeSqlDAL
    {
        private string connectionString;
        private const string SQL_Employee = @"select * from employee";
        private const string SQL_SearchEmployee = @"select * from employee where @fName = first_name and @lName = last_name";
        private const string SQL_SearchEmpWithOutProj = @"select * from employee e
                                                left outer join project_employee pe on pe.employee_id = e.employee_id
                                                where pe.project_id is null;";
        private const string SQL_UpdateEmployee = @"update department set name = @name where department_id = @dept_id;";

        // Single Parameter Constructor
        public EmployeeSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> listOfEmployees = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_Employee, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        employee.Gender = Convert.ToString(reader["gender"]);
                       // employee.HireDate = Convert.ToDateTime(reader["hire_date"]);

                        listOfEmployees.Add(employee);

                    }
                }
            }
            catch (SqlException ex)
            {
                //Log and throw the exception
                throw ex;
            }

            return listOfEmployees;
        }

        public List<Employee> Search(string firstname, string lastname)
        {
            List<Employee> listSearchedEmp = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_SearchEmployee, conn);
                    cmd.Parameters.AddWithValue("@fName", firstname);
                    cmd.Parameters.AddWithValue("@lName", lastname);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        employee.Gender = Convert.ToString(reader["gender"]);
                        // employee.HireDate = Convert.ToDateTime(reader["hire_date"]);

                        listSearchedEmp.Add(employee);

                    }
                }
            }
            catch (SqlException ex)
            {
                //Log and throw the exception
                throw ex;
            }

            return listSearchedEmp;
        }

        public List<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> listSearchedEmp = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_SearchEmpWithOutProj, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        employee.Gender = Convert.ToString(reader["gender"]);
                        // employee.HireDate = Convert.ToDateTime(reader["hire_date"]);

                        listSearchedEmp.Add(employee);

                    }
                }
            }
            catch (SqlException ex)
            {
                //Log and throw the exception
                throw ex;
            }

            return listSearchedEmp;
        }
    }
}
