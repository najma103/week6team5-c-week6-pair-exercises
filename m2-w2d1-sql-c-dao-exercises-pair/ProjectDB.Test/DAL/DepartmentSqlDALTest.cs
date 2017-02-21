using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;
using ProjectDB.DAL;
using ProjectDB.Models;


namespace ProjectDB.Test.DAL
{
    [TestClass()]
    public class DepartmentSqlDALTest
    {
        private TransactionScope tran;      //<-- used to begin a transaction during initialize and rollback during cleanup
        private string connectionString = @"Data Source=DESKTOP-58F8CH1\SQLEXPRESS;Initial Catalog=Week6Project;Integrated Security=True";
        // Set up the database before each test        
        [TestInitialize]
        public void Initialize()
        {
            // Initialize a new transaction scope. This automatically begins the transaction.
            tran = new TransactionScope();

            // Open a SqlConnection object using the active transaction
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();

                //Insert a Dummy Record for Department                
                cmd = new SqlCommand("INSERT INTO department VALUES ('Accounting');", conn);
                cmd.ExecuteNonQuery();
            }
        }

        // Cleanup runs after every single test
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose(); //<-- disposing the transaction without committing it means it will get rolled back
        }
    
        [TestMethod()]
        public void UpdateDepartmentTest()
        {

            Department updatedDepartment = new Department
            {
                Id = 3,
                Name = "Retail",
            };
            DepartmentSqlDAL dal = new DepartmentSqlDAL(connectionString);
            bool result = dal.UpdateDepartment(updatedDepartment);
            //Assert
            Assert.AreEqual(true,result);               
        }

        [TestMethod()]
        public void CreateDepartmentTest()
        {
            Department newDept = new Department
            {
                Name = "CustomerService"
            };
            DepartmentSqlDAL dal = new DepartmentSqlDAL(connectionString);

            bool result = dal.CreateDepartment(newDept);

            Assert.AreEqual(true, result);
        }
        [TestMethod()]
        public void GetAllDepartmentTest()
        {
            DepartmentSqlDAL dal = new DepartmentSqlDAL(connectionString);
            List<Department> departments = dal.GetDepartments();

            Assert.IsNotNull(departments);
            Assert.AreEqual(7, departments.Count);
          

        }
    }
}
