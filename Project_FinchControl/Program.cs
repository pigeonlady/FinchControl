using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control - Menu Starter + Talent Show
    // Description: Starter solution with the helper methods,
    //              opening and closing screens, and the menu.
    //              Finch robot talent show with lights, sound,
    //              movement, and user choice.
    // Application Type: Console
    // Author: Velis, John
    // Co-Author: Brobst, Victoria
    // Dated Created: 1/22/2020
    // Last Modified: 10/04/2020
    //
    // **************************************************

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "c":
                        DataRecorderDisplayMenuScreen(finchRobot);
                        break;

                    case "d":

                        break;

                    case "e":

                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region DATA RECORDER

        /// <summary>
        /// *****************************************************************
        /// *                     Data Recorder Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DataRecorderDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            int numberOfDataPoints = 0;
            double frequencyOfDataPointsSeconds = 0;
            double[] temperatures = null;

            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Get the Number of Data Points");
                Console.WriteLine("\tb) Get the Frequency of Data Points");
                Console.WriteLine("\tc) Get the Data Set");
                Console.WriteLine("\td) Display the Data Set");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        frequencyOfDataPointsSeconds = DataRecorderDisplayGetFrequencyOfDataPoints();
                        break;

                    case "c":
                        if (numberOfDataPoints == 0 || frequencyOfDataPointsSeconds == 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Please enter the number and frequency of data points first.");
                            DisplayContinuePrompt();
                        }
                        else
                        {
                            temperatures = DateRecorderDisplayGetDataSet(numberOfDataPoints, frequencyOfDataPointsSeconds, finchRobot);
                        }
                        break;

                    case "d":
                        DataRecorderDisplayGetDataSet(temperatures);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        /// <summary>
        /// *****************************************************************
        /// *          Data Recorder > Display the Data Table               *
        /// *****************************************************************
        /// </summary>
        static void DataRecorderDisplayDataTable(double[] temperatures)
        {
            Console.WriteLine(
               "Reading #".PadLeft(15) +
               "Temperature".PadLeft(15)
               );

            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
                    (index + 1).ToString().PadLeft(15) +
                    temperatures[index].ToString("n2").PadLeft(15)
                    );
            }


        }

        /// <summary>
        /// *****************************************************************
        /// *          Data Recorder > Display the Data Points              *
        /// *****************************************************************
        /// </summary>
        static void DataRecorderDisplayGetDataSet(double[] temperatures)
        {
            DisplayScreenHeader("Data Set");

            DataRecorderDisplayDataTable(temperatures);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *            Data Recorder > Get the Data Points                *
        /// *****************************************************************
        /// </summary>
        /// </param name="finchRobot">finch robot object</param>
        static double[] DateRecorderDisplayGetDataSet(int numberOfDataPoints, double frequencyOfDataPointsSeconds, Finch finchRobot)
        {
            double[] temperatures = new double[numberOfDataPoints];

            DisplayScreenHeader("Get Data Set");

            Console.WriteLine($"\tNumber of Data Points: {numberOfDataPoints}");
            Console.WriteLine($"\tFrequency of Data Points: {frequencyOfDataPointsSeconds}");
            Console.WriteLine();

            Console.WriteLine("The Finch robot is ready to record temperature data. Press any key to begin.");
            Console.ReadKey();

            double temperature;
            int frequencyOfDataPointsMilliSeconds;
            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temperature = finchRobot.getTemperature();
                Console.WriteLine($"Temperature Reading: {index + 1}: {temperature}");
                temperatures[index] = temperature;
                frequencyOfDataPointsMilliSeconds = (int)(frequencyOfDataPointsSeconds * 1000);
                finchRobot.wait(frequencyOfDataPointsMilliSeconds);
            }


            DisplayContinuePrompt();

            return temperatures;
        }

        /// <summary>
        /// *****************************************************************
        /// *    Data Recorder > Get the Frequency of Data Points           *
        /// *****************************************************************
        /// </summary>
        static double DataRecorderDisplayGetFrequencyOfDataPoints()
        {
            double frequencyOfDataPoints;

            DisplayScreenHeader("\tFrequency of Data Points");

            Console.Write("\tEnter the Frequency of Data Points: ");
            double.TryParse(Console.ReadLine(), out frequencyOfDataPoints);

            Console.WriteLine();
            Console.WriteLine($"\tFrequency of Data Points: {frequencyOfDataPoints}");


            DisplayContinuePrompt();

            return frequencyOfDataPoints;
        }

        /// <summary>
        /// *****************************************************************
        /// *      Data Recorder > Get the Number of Data Points            *
        /// *****************************************************************
        /// </summary>
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;

            DisplayScreenHeader("\tNumber of Data Points");

            Console.Write("\tEnter the Number of Data Points: ");
            int.TryParse(Console.ReadLine(), out numberOfDataPoints);
            
            Console.WriteLine();
            Console.WriteLine($"\tNumber of Data Points: {numberOfDataPoints}");


            DisplayContinuePrompt();

            return numberOfDataPoints;
        }


        #endregion

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light");
                Console.WriteLine("\tb) Sound");
                Console.WriteLine("\tc) Movement");
                Console.WriteLine("\td) User's Choice");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayLight(finchRobot);
                        break;

                    case "b":
                        DisplaySound(finchRobot);
                        break;

                    case "c":
                        DisplayMotors(finchRobot);
                        break;

                    case "d":
                        UsersChoice(finchRobot);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light                             *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayLight(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light");

            Console.WriteLine("\tThe Finch robot will now show off its glowing talent!");
            DisplayContinuePrompt();


            Console.WriteLine("Red!");
            for (int i = 0; i < 100; i++)
            {
                finchRobot.setLED(i, 0, 0);
            }
            for (int i = 100; i > 0; i--)
            {
                finchRobot.setLED(i, 0, 0);
            }

            Console.WriteLine("Green!");
            for (int i = 0; i < 100; i++)
            {
                finchRobot.setLED(0, i, 0);
            }
            for (int i = 100; i > 0; i--)
            {
                finchRobot.setLED(0, i, 0);
            }

            Console.WriteLine("Blue!");
            for (int i = 0; i < 100; i++)
            {
                finchRobot.setLED(0, 0, i);
            }
            for (int i = 100; i > 0; i--)
            {
                finchRobot.setLED(0, 0, i);
            }


            Console.WriteLine("~Rainbow~");
            finchRobot.setLED(255, 0, 0);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.setLED(255, 128, 0);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.setLED(255, 255, 0);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.setLED(128, 255, 0);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.setLED(0, 255, 0);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.setLED(0, 255, 128);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.setLED(0, 255, 255);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.setLED(0, 128, 255);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.setLED(0, 0, 255);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.setLED(127, 0, 255);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.setLED(255, 0, 255);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);
            finchRobot.setLED(255, 0, 127);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);


            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Sound                             *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplaySound(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Sound");

            Console.WriteLine("\tThe Finch robot will now sing its favorite tune!");
            DisplayContinuePrompt();

            Console.WriteLine("Play: Bad Mr. Roboto Tune");
            finchRobot.noteOn(783);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(50);
            finchRobot.noteOn(783);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(150);

            finchRobot.noteOn(800);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(50);
            finchRobot.noteOn(800);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(50);
            finchRobot.noteOn(800);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(25);
            finchRobot.noteOn(698);
            finchRobot.wait(200);
            finchRobot.noteOff();
            finchRobot.wait(250);

            finchRobot.noteOn(783);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(50);
            finchRobot.noteOn(783);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(150);

            finchRobot.noteOn(800);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(50);
            finchRobot.noteOn(800);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(25);
            finchRobot.noteOn(698);
            finchRobot.wait(200);
            finchRobot.noteOff();
            finchRobot.wait(300);

            finchRobot.noteOn(466);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(50);
            finchRobot.noteOn(466);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(50);

            finchRobot.noteOn(415);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(50);
            finchRobot.noteOn(415);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(150);

            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Movement                          *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayMotors(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Movement");

            Console.WriteLine("\tThe Finch robot will now move in circles!");
            DisplayContinuePrompt();

            finchRobot.setMotors(-255, -255);
            finchRobot.wait(1000);
            finchRobot.setMotors(0, 0);

            Console.WriteLine("Clockwise!");
            finchRobot.setMotors(255, -255);
            finchRobot.wait(3000);
            finchRobot.setMotors(0, 0);

            Console.WriteLine("Now counter clockwise!");
            finchRobot.setMotors(-255, 0);
            finchRobot.wait(3000);
            finchRobot.setMotors(0, 0);

            finchRobot.setMotors(255, 128);
            finchRobot.wait(1000);
            finchRobot.setMotors(0, 0);

            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > User's Choice (Color Choice)      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void UsersChoice(Finch finchRobot)
        {
            string userResponse;

            Console.CursorVisible = true;

            DisplayScreenHeader("User's Choice");

            Console.WriteLine("\tNow you get to decide what you want the finch to do!");
            Console.WriteLine("\tYou will get to choose a color, sound and rotation for the Finch to display all at once!");
            DisplayContinuePrompt();

            //
            // ask user to display finch color!
            //

            Console.WriteLine();
            Console.WriteLine("\tChoose a color you would like the finch to display:");
            Console.WriteLine("\tred");
            Console.WriteLine("\tpurple");
            Console.WriteLine("\tblue");
            Console.WriteLine("\tgreen");
            Console.WriteLine("\tyellow");
            Console.WriteLine();
            Console.Write("\tEnter Choice: ");
            userResponse = Console.ReadLine().ToLower();

            switch (userResponse)
            {
                case "red":
                    finchRobot.setLED(255, 0, 0);
                    UsersChoiceSound(finchRobot);
                    UsersChoiceMove(finchRobot);
                    finchRobot.setMotors(0, 0);
                    finchRobot.noteOff();
                    finchRobot.setLED(0, 0, 0);
                    break;

                case "purple":
                    finchRobot.setLED(127, 0, 255);
                    UsersChoiceSound(finchRobot);
                    UsersChoiceMove(finchRobot);
                    finchRobot.setMotors(0, 0);
                    finchRobot.noteOff();
                    finchRobot.setLED(0, 0, 0);
                    break;

                case "blue":
                    finchRobot.setLED(0, 0, 255);
                    UsersChoiceSound(finchRobot);
                    UsersChoiceMove(finchRobot);
                    finchRobot.setMotors(0, 0);
                    finchRobot.noteOff();
                    finchRobot.setLED(0, 0, 0);
                    break;

                case "green":
                    finchRobot.setLED(0, 255, 0);
                    UsersChoiceSound(finchRobot);
                    UsersChoiceMove(finchRobot);
                    finchRobot.setMotors(0, 0);
                    finchRobot.noteOff();
                    finchRobot.setLED(0, 0, 0);
                    break;

                case "yellow":
                    finchRobot.setLED(255, 255, 0);
                    UsersChoiceSound(finchRobot);
                    UsersChoiceMove(finchRobot);
                    finchRobot.setMotors(0, 0);
                    finchRobot.noteOff();
                    finchRobot.setLED(0, 0, 0);
                    break;

                default:
                    Console.WriteLine();
                    Console.Write("\tPlease enter a valid color option: ");
                    userResponse = Console.ReadLine().ToLower();
                    break;
            }

            Console.ReadKey();
            DisplayContinuePrompt();
            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > User's Choice (Sound Method)      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void UsersChoiceSound(Finch finchRobot)
        {
            string userResponse;
            bool validResponse;
            int finchSound;

            Console.WriteLine();
            Console.Write("\tEnter a number between 100 and 4000 to make the finch sing!: ");
            userResponse = Console.ReadLine();
            int.TryParse(userResponse, out finchSound);

            do
            {
                validResponse = true;

                if (finchSound >= 100 && finchSound <= 4000)
                {
                    finchRobot.noteOn(finchSound);
                }
                else
                {
                    Console.WriteLine();
                    Console.Write("\tPlease enter a valid sound number: ");
                    userResponse = Console.ReadLine();
                    int.TryParse(userResponse, out finchSound);
                    DisplayContinuePrompt();
                    validResponse = false;
                }

            } while (!validResponse);

        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > User's Choice (Move Method)       *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void UsersChoiceMove(Finch finchRobot)
        {
            string userResponse;

            Console.WriteLine();
            Console.WriteLine("\tHow would you like to make the finch move?");
            Console.WriteLine("\ta) clockwise");
            Console.WriteLine("\tb) counter clockwise");
            Console.WriteLine();
            Console.Write("\tEnter Choice: ");
            userResponse = Console.ReadLine().ToLower();

            switch (userResponse)
            {
                case "a":
                    finchRobot.setMotors(255, -255);
                    finchRobot.wait(3000);
                    break;

                case "b":
                    finchRobot.setMotors(-255, 0);
                    finchRobot.wait(3000);
                    break;

                default:
                    Console.WriteLine();
                    Console.WriteLine("\tPlease enter a valid move option [a or b]: ");
                    userResponse = Console.ReadLine().ToLower();
                    break;
            }

        }

        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnect.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            if (robotConnected == true)
            {
                finchRobot.setLED(0, 255, 0);
                finchRobot.wait(300);
                finchRobot.noteOn(146);
                finchRobot.wait(100);
                finchRobot.noteOff();
                finchRobot.noteOn(293);
                finchRobot.wait(100);
                finchRobot.noteOff();
                finchRobot.noteOn(587);
                finchRobot.wait(100);
                finchRobot.noteOff();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\tFinch Robot Connected!");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tPlease make sure your USB cable is connected properly and try again.");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.ReadKey();
                robotConnected = false;
            }


            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
