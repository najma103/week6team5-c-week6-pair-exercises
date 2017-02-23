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
            //Console.WriteLine("3.\t Search For Reservations");

            int option = Convert.ToInt32(Console.ReadLine());
            //this the park id to access campgrounds
            int parkId = listOfParks[parkIndex - 1].ParkId;

            if (option == 1)
            {
                DisplayCampsByParkId(parkId, listOfParks);
                Console.ReadKey();
            }
            else if (option == 3)
            {
                DisplayCampsByParkId(parkId, listOfParks);
                DisplayAvailableSites();
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
        public void DisplayAvailableSites()
        {
            Console.WriteLine();
            Console.WriteLine(@"Which campground (enter 0 to cancel)? __");
            int campId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine(@"What is the arrival date? __/__/____ ");
            DateTime fromDate = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine(@"What is the departure date? __/__/____ ");
            DateTime toDate = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine("Results Matching Your Search Criteria ");
            Console.WriteLine("Site No.\t Max Occup.\t Accessible?\t RV Len \t Utility\t Cost");

            SiteSqlDal dal = new SiteSqlDal(dbConnection);
            List<Site> listOfAvailableSites = dal.GetAllAvailableSitesForCamp(campId, fromDate, toDate);

            //when working on the bonus search - move this to new method and call it
            CampgroundSqlDal campDal = new CampgroundSqlDal(dbConnection);
            List<Campground> listOfCamp = campDal.GetNameByCampId(campId);

            if (listOfAvailableSites.Count > 0)
            {
                for (int i = 0; i < listOfAvailableSites.Count; i++)
                {
                    //Console.Write(listOfCamp[0].Name + "\t");
                    Console.Write(listOfAvailableSites[i].SiteNumber + "\t\t");
                    Console.Write(listOfAvailableSites[i].MaxOccupancy + "\t\t");
                    if (listOfAvailableSites[i].Accessible)
                    {
                        Console.Write("Yes\t\t");
                    }
                    else
                    {
                        Console.Write("No\t\t");
                    }
                    if (listOfAvailableSites[i].MaxRVLength == 0)
                    {
                        Console.Write("N/A\t\t");
                    }
                    else
                    {
                        Console.Write(listOfAvailableSites[i].MaxRVLength + " \t\t");
                    }
                    if (listOfAvailableSites[i].Utilities)
                    {
                        Console.Write("Yes\t\t");
                    }
                    else
                    {
                        Console.Write("N/A\t\t");
                    }
                    Console.Write(listOfCamp[0].DailyFee);
                    Console.WriteLine();

                    
                }

            }
            Console.WriteLine();
            Console.WriteLine("Which site should be reserved (enter 0 to cancel)? __  ");
            Console.WriteLine("What name should the reservation be made under? __");

            Console.ReadKey();





        }
        public void DisplayParkDetails(int parkIndex, List<Park> listOfParks)
        {
            Console.WriteLine(listOfParks[parkIndex].Name + " National Park");
            Console.WriteLine("Location:\t" + listOfParks[parkIndex].Location);
            Console.WriteLine("Established:\t" + listOfParks[parkIndex].EstablishDate);
            Console.WriteLine("Area:\t\t " + listOfParks[parkIndex].Area);
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
                for (int i = 0; i < listOfCamps.Count; i++)
                {
                    Console.WriteLine( listOfCamps[i]);
                }
                DisplayAvailableSites();
            }
        }
    }
}
