using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone
{
    class ProjectCLI
    {
        private string dbConnection;
        public ProjectCLI(string strConnection)
        {
            dbConnection = strConnection;
        }

        public void RunCLI()
        {
            ParkSqlDal dal = new ParkSqlDal(dbConnection);
            List<Park> listOfParks = dal.GetAllParks();
            Console.WriteLine("Welcome To Campground Reservation System");
            DisplayAllParks(listOfParks);

            Console.WriteLine("Please Select A Park Further Details");
            string userOption = Console.ReadLine();
            int parkIndex = Convert.ToInt32(userOption);
            DisplayParkDetails(parkIndex-1, listOfParks);

            Console.WriteLine("1.\t View List of Campgrounds");
            Console.WriteLine("2.\t View List of Parks");
            Console.WriteLine("3.\t Search For Reservations");

            int option = Convert.ToInt32(Console.ReadLine());
            //this the park id to access campgrounds
            int parkId = listOfParks[parkIndex - 1].ParkId;

            if (option == 1)
            {
                DisplayCampsByParkId(parkId, listOfParks);
                Console.ReadKey();
            }
           
            




            //SearchCampsByParkId(parkIndex);


        }
        public void DisplayAllParks(List<Park> listOfParks)
        {
            int index;
            for (int i = 0; i < listOfParks.Count; i++)
            {
                index = i + 1;
                Console.Write(index + ".  ");
                Console.Write(listOfParks[i].Name);
                Console.WriteLine();
            }

        }

        public void DisplayParkDetails(int parkIndex, List<Park> listOfParks)
        {
            Console.WriteLine(listOfParks[parkIndex].Name + " National Park");
            Console.WriteLine("Location:\t" + listOfParks[parkIndex].Location);
            Console.WriteLine("Established:\t" + listOfParks[parkIndex].EstablishDate);
            Console.WriteLine("Area:\t " + listOfParks[parkIndex].Area);
            Console.WriteLine("Annual Visitors:\t" + listOfParks[parkIndex].Visitors);
            Console.WriteLine();
            Console.WriteLine(listOfParks[parkIndex].Description);
        }

         public void DisplayCampsByParkId(int index, List<Park> listOfParks)
        {
            CampgroundSqlDal dal = new CampgroundSqlDal(dbConnection);
            List<Campground> listOfCamps = dal.GetAllCampsByParkId(index);
            if (listOfCamps.Count > 0)
            {
                int campNum;
                for (int i = 0; i < listOfCamps.Count; i++)
                {
                    campNum = i + 1;
                    Console.WriteLine(campNum + ".\t" + listOfCamps[i]);
                }
                
            }
        }
    }
}
