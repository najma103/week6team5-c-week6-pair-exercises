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
            DisplayHeader();
            DisplayMainMenu();
            ParkSqlDal dal = new ParkSqlDal(dbConnection);
            List<Park> listOfParks = dal.GetAllParks();
            //Console.WriteLine("Welcome To Campground Reservation System");
            
            //Console.WriteLine("Please Select Park for Park Details");
            while (true)
            {
                string userOption = Console.ReadLine();
                int parkIndex = Convert.ToInt32(userOption);

                if (userOption == "1")
                {
                    DisplayAllParks(listOfParks);
                    //sub menu for customer to choose campground based on park id
                    
                    ProcessCustomerOption();
                    //Console.ReadKey();
                    //break;
                }
                else if (userOption == "2")
                {
                    DisplayAllParks(listOfParks);
                    Console.WriteLine("Please Select A Park__");
                    int parkId = Convert.ToInt32(Console.ReadLine());
                    
                } else if (userOption == "3")
                {
                    Console.WriteLine("Thank You For Using Our Park Reservation System");
                    return;
                }

                DisplayMainMenu();
            }
    


            // DisplayParkDetails(parkIndex-1, listOfParks);
            //Console.WriteLine("1.\t View List of Campgrounds");
            //Console.WriteLine("2.\t View List of Parks");
            ////Console.WriteLine("3.\t Search For Reservations");

            //int option = Convert.ToInt32(Console.ReadLine());
            ////this the park id to access campgrounds
            //int parkId = listOfParks[parkIndex - 1].ParkId;

            //if (option == 1)
            //{
            //    DisplayCampsByParkId(parkId, listOfParks);
            //    Console.ReadKey();
            //}
            //else if (option == 3)
            //{
            //    DisplayCampsByParkId(parkId, listOfParks);
            //    DisplayAvailableSites();
            //}






            //SearchCampsByParkId(parkIndex);


        }

        public void ProcessCustomerOption()
        {
            DisplaySubMenu();
            ParkSqlDal dal = new ParkSqlDal(dbConnection);
            List<Park> listOfParks = dal.GetAllParks();
            int parkId; 
            
            while (true)
            {
                string userInput = Console.ReadLine();
                if (userInput == "1")
                {
                    Console.Clear();
                    DisplayAllParks(listOfParks);
                    Console.WriteLine("Please Select A Park To View Campgrounds");
                    parkId = Convert.ToInt32(Console.ReadLine());
                    DisplayCampsByParkId(parkId, listOfParks);

                }
                else if (userInput == "2")
                {
                    DisplayAllParks(listOfParks);
                    Console.WriteLine("Please Choose A Park To View Details");
                    parkId = Convert.ToInt32(Console.ReadLine());
                    DisplayParkDetails(parkId-1, listOfParks);

                }
                else if (userInput == "3")
                {
                    //search available camp sites by park id 
                }
                else if(userInput == "4")
                {
                    SearchByCampId();
                }
                else if (userInput == "5")
                {
                    return;
                }

                DisplaySubMenu();
            }



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

        public void SearchByCampId()
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

            SearchAvailableSitesByCampId(campId, fromDate, toDate);
        }
        public void SearchAvailableSitesByCampId(int campId, DateTime fromDate, DateTime toDate)
        {
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

                Console.WriteLine();
                Console.WriteLine("Which site should be reserved (enter 0 to cancel)? __  ");
                int userInput = Convert.ToInt32(Console.ReadLine());
                string reservationName = string.Empty;
                if (userInput == 0)
                {
                    return;
                }
                else
                {

                   if (listOfAvailableSites.Count >= userInput)
                   {
                        Console.WriteLine("What name should the reservation be made under? __");
                        reservationName = Console.ReadLine();
                        MakeReservation(userInput, reservationName, fromDate, toDate);
                   }
                }
                
            } else
            {
                Console.WriteLine("Sorry there are no available sites for your requested dates.");
            }

        }

        public void MakeReservation(int userInput, string reservationName, DateTime fromDate, DateTime toDate)
        {
            Reservation newReservation = new Reservation()
            {
                SiteId = userInput,
                Name = reservationName,
                FromDate = fromDate,
                ToDate = toDate,
                CreateDate = DateTime.Now
            };
            ReservationSqlDal dal = new ReservationSqlDal(dbConnection);
            bool rowInserted = dal.CreateReservation(newReservation);

            if (rowInserted)
            {
                Console.WriteLine("Reservation was successfully created and reservation id is " + 1);
                Console.ReadKey();
            }
           // int reservationId = 1 // dal.MakeNewReservation();

            //return reservationId;
        }
        public void DisplayParkDetails(int parkIndex, List<Park> listOfParks)
        {
            while (true)
            {
                if (listOfParks.Count > 0 && parkIndex <= listOfParks.Count)
                {
                    Console.WriteLine(listOfParks[parkIndex].Name + " National Park");
                    Console.WriteLine("Location:\t" + listOfParks[parkIndex].Location);
                    Console.WriteLine("Established:\t" + listOfParks[parkIndex].EstablishDate);
                    Console.WriteLine("Area:\t\t " + listOfParks[parkIndex].Area);
                    Console.WriteLine("Annual Visitors:\t" + listOfParks[parkIndex].Visitors);
                    Console.WriteLine();
                    Console.WriteLine(listOfParks[parkIndex].Description);
                    break;
                }
                else
                {
                    Console.WriteLine("Sorry that was not a valid Park ID");
                    break;
                }

            }
        }

        public void DisplayCampsByParkId(int parkId, List<Park> listOfParks)
        {
            CampgroundSqlDal dal = new CampgroundSqlDal(dbConnection);
            List<Campground> listOfCamps = dal.GetAllCampsByParkId(parkId);
            if (listOfCamps.Count > 0)
            {
                for (int i = 0; i < listOfCamps.Count; i++)
                {
                    Console.WriteLine( listOfCamps[i]);
                }
            }
        }

        //all the menus
        public void DisplayHeader()
        {
            Console.WriteLine();
            Console.WriteLine(@"***********************************************************************");
            Console.WriteLine(@"*                                                                     *");
            Console.WriteLine(@"*                Campground Reservation System                        *");
            Console.WriteLine(@"*                                                                     *");
            Console.WriteLine(@"***********************************************************************");
            Console.WriteLine();
        }

        public void DisplayMainMenu()
        {
            Console.Clear();
            DisplayHeader();
            Console.WriteLine(@"( 1 ) To View All Parks.");
            Console.WriteLine(@"( 2 ) To View Current Reservations.");
            Console.WriteLine(@"( 3 ) To Exit. ");
        }

        public void DisplaySubMenu()
        {
            //Console.Clear();
            Console.WriteLine(@"( 1 ) View Campgrounds By Park Id.");
            Console.WriteLine(@"( 2 ) View Park Details");
            Console.WriteLine(@"( 3 ) View All CampSites By Park ID.");
            Console.WriteLine(@"( 4 ) Search Reservation Availabilty By Campground Id.");
            Console.WriteLine(@"( 5 ) Return To Previous Menu. ");
        }
    }
}
