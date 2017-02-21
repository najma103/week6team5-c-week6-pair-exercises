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
    /// Summary description for ProjectSqlDALTest
    /// </summary>
    [TestClass]
    public class ProjectSqlDALTest
    {
        private string connectionString = @"Data Source=DESKTOP-58F8CH1\SQLEXPRESS;Initial Catalog=Week6Project;Integrated Security=True";
        public ProjectSqlDALTest()
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
        public void GetAllProjectsTest()
        {
            ProjectSqlDAL dal = new ProjectSqlDAL(connectionString);
            List<Project> projects = dal.GetAllProjects();

            Assert.IsNotNull(projects);
            Assert.AreEqual(7, projects.Count);
        }
        [TestMethod()]
        public void AssignEmployeeToProjectTest()
        {
            int projectId = 3;
            int employeeId = 5;

            ProjectSqlDAL dal = new ProjectSqlDAL(connectionString);
            bool result = dal.AssignEmployeeToProject(projectId, employeeId);
            Assert.AreEqual(true, result);

        }
    }
}
