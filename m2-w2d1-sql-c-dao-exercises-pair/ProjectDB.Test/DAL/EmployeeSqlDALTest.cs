using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Transactions;
using ProjectDB.DAL;
using ProjectDB.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectDB.Test.DAL
{
    /// <summary>
    /// Summary description for EmployeeSqlDALTest
    /// </summary>
    [TestClass]
    public class EmployeeSqlDALTest
    {
        private string connectionString = @"Data Source=DESKTOP-58F8CH1\SQLEXPRESS;Initial Catalog=Week6Project;Integrated Security=True";
        public EmployeeSqlDALTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetAllEmployeesTest()
        {
            EmployeeSqlDAL dal = new EmployeeSqlDAL(connectionString);
            List<Employee> employees = dal.GetAllEmployees();

            Assert.IsNotNull(employees);
            Assert.AreEqual(12, employees.Count);
        }
        [TestMethod()]
        public void SearchEmployeesTest()
        {
            string firstname = "Jammie";
            string lastname = "Mohl";

            EmployeeSqlDAL dal = new EmployeeSqlDAL(connectionString);
            // Search employee by first name and last name
            List<Employee> employees = dal.Search(firstname, lastname);

            Assert.IsNotNull(employees);
            Assert.AreEqual("Following the Boss Around", employees[0].JobTitle.ToString());
        }

        [TestMethod]
        public void GetAllEmployeesWithoutProjTest()
        {
            EmployeeSqlDAL dal = new EmployeeSqlDAL(connectionString);
            List<Employee> employees = dal.GetEmployeesWithoutProjects();

            Assert.IsNotNull(employees);
            Assert.AreEqual(1, employees.Count);

            //check if employee_id that is returned is = 4
            Assert.AreEqual(4, employees[0].EmployeeId);

        }
    }
}
