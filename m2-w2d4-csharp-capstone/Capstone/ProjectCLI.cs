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
            Dictionary<int,Park> dictionaryParks = dal.GetAllParks();

            while (true)
            {
                string userOption = Console.ReadLine();
                int parkIndex = Convert.ToInt32(userOption);

                if (userOption == "1")
                {
                    DisplayAllParks(dictionaryParks);
                    //sub menu for customer to choose campground based on park id
                    ProcessCustomerOption();
                }
                else if (userOption == "2")
                {
                    GetAllCurrentReservations(dictionaryParks);

                } else if (userOption == "3")
                {
                    Console.WriteLine("Thank You For Using Our Park Reservation System");
                    return;
                }

                DisplayMainMenu();
            }

        }

        public void GetAllCurrentReservations(Dictionary<int,Park> listOfParks)
        {
            DisplayAllParks(listOfParks);

            Console.WriteLine();
            Console.WriteLine("Please Select A Park_____");

            int parkId = Convert.ToInt32(Console.ReadLine());
            if (listOfParks.ContainsKey(parkId))
            {
                ReservationSqlDal dal = new ReservationSqlDal(dbConnection);
                Dictionary<int, Reservation> dictReservations = dal.GetCurrentReservations(parkId);
                if (dictReservations.Count > 0)
                {
                    Console.WriteLine("Current Park Reservations: ");
                    Console.WriteLine("ID\t Site ID\t\t Reservation Name\t\t Start Date\t" +
                                        "End date\t Create Date");
                    foreach (var key in dictReservations.Keys)
                    {
                        Console.WriteLine(dictReservations[key].ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Sorry there are no current reservations. ");
                }
                Console.ReadKey();

            }

        }

        public void ProcessCustomerOption()
        {
            DisplaySubMenu();
            ParkSqlDal dal = new ParkSqlDal(dbConnection);
            Dictionary<int,Park> listOfParks = dal.GetAllParks();
            int parkId; 
            
            while (true)
            {
                string userInput = Console.ReadLine();
                if (userInput == "1") // park details
                {
                    DisplayAllParks(listOfParks);
                    Console.WriteLine("Please Choose A Park To View Details");
                    parkId = Convert.ToInt32(Console.ReadLine());
                    DisplayParkDetails(parkId, listOfParks);
                }
                else if (userInput == "2")
                {
                    DisplayAllParks(listOfParks);
                    Console.WriteLine("Please Select A Park ID To View All Campground Sites");
                    parkId = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine(@"What is the arrival date? __/__/____ ");
                    DateTime fromDate = Convert.ToDateTime(Console.ReadLine());

                    Console.WriteLine(@"What is the departure date? __/__/____ ");

                    DateTime toDate = Convert.ToDateTime(Console.ReadLine());

                    if (listOfParks.ContainsKey(parkId))
                    {
                        CampgroundSqlDal campDal = new CampgroundSqlDal(dbConnection);
                        Dictionary<int, Campground> listOfCamp = campDal.GetAllCampsByParkId(parkId);
                        Console.WriteLine();
                        Console.WriteLine("Results Matching Your Search Criteria ");
                        foreach (int key  in listOfCamp.Keys)
                        {
                            if (fromDate.Month >= listOfCamp[key].OpenFromMonth && toDate.Month <= listOfCamp[key].OpenToMonth)
                            {
                                Console.WriteLine("Camp Name \t\t Site No.\t Max Occup.\t Accessible?\t RV Len \t Utility\t Cost");

                                SearchAvailableSitesByCampId(key, fromDate, toDate);
                                //MakeReservation(fromDate, toDate);
                            }
                            else
                            {
                                Console.WriteLine(@"Invalid Date(s), park is not open on one of these dates. ");
                            }
                        }
                        //after printing out all the available sites, calls make reservation method
                        MakeReservation(fromDate, toDate);
                    }

                }
                else if (userInput == "3")
                {
                    //Console.Clear();
                    DisplayAllParks(listOfParks);
                    Console.WriteLine("Please Select A Park ID To View Campgrounds");
                    parkId = Convert.ToInt32(Console.ReadLine());
                    if (listOfParks.ContainsKey(parkId))
                    {
                        DisplayCampsByParkId(parkId, listOfParks);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Park ID Selection, Please try again. ");
                    }
                }
                else if (userInput == "4")
                {
                    return;
                }

                DisplaySubMenu();
            }

        }
        public void DisplayAllParks(Dictionary<int,Park> listOfParks)
        {
            foreach (int key  in listOfParks.Keys)
            {
                Console.Write(key + "-  ");
                Console.Write(listOfParks[key].Name.ToString());
                Console.WriteLine();
            }

        }

        public void SearchByCampId(int campId)
        {
            CampgroundSqlDal campDal = new CampgroundSqlDal(dbConnection);
            Dictionary<int, Campground> listOfCamp = campDal.GetNameByCampId(campId);

            Console.WriteLine(@"What is the arrival date? __/__/____ ");
            DateTime fromDate = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine(@"What is the departure date? __/__/____ ");

            DateTime toDate = Convert.ToDateTime(Console.ReadLine());

            if (fromDate.Month >= listOfCamp[campId].OpenFromMonth  && toDate.Month <= listOfCamp[campId].OpenToMonth )
            {
                Console.WriteLine("Results Matching Your Search Criteria\r\n ");
                Console.WriteLine("Camp Name. \t\t Site No.\t Max Occup.\t Accessible?\t RV Len \t Utility\t Cost");

                SearchAvailableSitesByCampId(campId, fromDate, toDate);
                MakeReservation(fromDate, toDate);
            }
            else
            {
                Console.WriteLine(@"Invalid Date(s), park is not open on one of these dates. ");
            }


        }
        public void SearchAvailableSitesByCampId(int campId, DateTime fromDate, DateTime toDate)
        {
            TimeSpan totalDays = toDate.Subtract(fromDate);
            int days = Convert.ToInt32(totalDays.Days);

            SiteSqlDal dal = new SiteSqlDal(dbConnection);
            List<Site> listOfAvailableSites = dal.GetAllAvailableSitesForCamp(campId, fromDate, toDate);

            //when working on the bonus search - move this to new method and call it
            CampgroundSqlDal campDal = new CampgroundSqlDal(dbConnection);
            Dictionary<int,Campground> listOfCamp = campDal.GetNameByCampId(campId);

            if (listOfAvailableSites.Count > 0)
            {
                for (int i = 0; i < listOfAvailableSites.Count; i++)
                {
                    Console.Write(listOfCamp[campId].Name + "\t\t");
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
                    Console.Write(listOfCamp[campId].DailyFee * days);
                    Console.WriteLine();

                }
            }
            else
            {
                Console.WriteLine("Sorry there are no available sites for your requested dates.");
            }

        }

        public void MakeReservation( DateTime fromDate, DateTime toDate)
        {
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
                Console.WriteLine("What name should the reservation be made under? __");
                reservationName = Console.ReadLine();
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
                    int reservationId = dal.GetLastReservations();
                    Console.WriteLine("Reservation was successfully created and reservation id is " +
                              reservationId);
                    Console.ReadKey();
                }
            }
        }
        public void DisplayParkDetails(int parkIndex, Dictionary<int,Park> listOfParks)
        {
            while (true)
            {
                if (listOfParks.ContainsKey(parkIndex))
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

        public void DisplayCampsByParkId(int parkId, Dictionary<int,Park> listOfParks)
        {
            if (listOfParks.ContainsKey(parkId))
            {
                CampgroundSqlDal dal = new CampgroundSqlDal(dbConnection);
                Dictionary<int,Campground> listOfCamps = dal.GetAllCampsByParkId(parkId);

                Console.WriteLine("Camp Id\t\t Name\t\t\t\t Open From\t Open Until\t Daily Fee");
                if (listOfCamps.Count > 0)
                {
                    foreach(int key in listOfCamps.Keys)
                    {
                        Console.WriteLine(listOfCamps[key].ToString());
                    }
                    Console.WriteLine("Please select campground to search for available dates or press 0 to return ___");
                    int campId = Convert.ToInt32(Console.ReadLine());
                    if (campId == 0)
                    {
                        return;
                    }
                    if (listOfCamps.ContainsKey(campId))
                    {
                        SearchByCampId(campId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Campground ID: ");
                    }
                    
                }
            } else
            {
                Console.WriteLine("Invalid Park ID - Please choose a valid park.");
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
            Console.WriteLine();
            Console.WriteLine(@"( 1 ) View Park Details");
            Console.WriteLine(@"( 2 ) View All CampSites By Park ID.");
            Console.WriteLine(@"( 3 ) View Campgrounds By Park Id.");
            Console.WriteLine(@"( 4 ) Return To Previous Menu. ");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
