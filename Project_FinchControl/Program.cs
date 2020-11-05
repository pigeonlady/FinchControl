using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control - Menu Starter + Talent Show 
    //          + Data Recorder + Alarm System + User Programming
    // Description: Starter solution with the helper methods,
    //              opening and closing screens, and the menu.
    //              Finch robot talent show with lights, sound,
    //              movement, and user choice. A data recorder that
    //              displays finch sensor temperature in fahrenheit.
    //              An alarm system that sounds when thresholds set by
    //              the user are exceeded. User programming where the 
    //              user can type commands and the finch robot performs.
    // Application Type: Console
    // Author: Velis, John
    // Co-Author: Brobst, Victoria
    // Dated Created: 1/22/2020
    // Last Modified: 11/03/2020
    //
    // **************************************************

    /// <summary>
    /// user commands
    /// </summary>
    public enum Command 
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF,
        NOTEON,
        NOTEOFF,
        GETTEMPERATURE,
        GETLEFTLIGHTSENSOR,
        GETRIGHTLIGHTSENSOR,
        DANCE,
        DONE
    }


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
                        AlarmSystemDisplayMenuScreen(finchRobot);
                        break;

                    case "e":
                        UserProgrammingDisplayMenuScreen(finchRobot);
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

        #region USER PROGRAMMING

        /// <summary>
        /// *****************************************************************
        /// *                  User Programming Menu                        *
        /// *****************************************************************
        /// </summary>
        static void UserProgrammingDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            (int motorSpeed, int ledBrightness, int noteFrequency, double waitSeconds) commandParameters = (0, 0, 0, 0);
            List<Command> commands = new List<Command>();

            do
            {
                DisplayScreenHeader("User Programming Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute Commands");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        commandParameters = UserProgrammingDisplayGetCommandParameters();
                        break;

                    case "b":
                        UserProgrammingDisplayGetCommands(commands);
                        break;

                    case "c":
                        UserProgrammingDisplayCommands(commands);
                        break;

                    case "d":
                        UserProgrammingDisplayExecuteCommands(finchRobot, commands, commandParameters);
                        commandParameters = (0, 0, 0, 0);
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

        static void UserProgrammingDisplayExecuteCommands(Finch finchRobot, List<Command> commands, (int motorSpeed, int ledBrightness, int noteFrequency, double waitSeconds) commandParameters)
        {
            DisplayScreenHeader("Execute Commands");

            Console.WriteLine("\tThe Finch Robot is ready to execute the commands.");
            DisplayContinuePrompt();
            Console.WriteLine();

            foreach (Command command in commands)
            {
                Console.WriteLine(command);

                switch (command)
                {
                    case Command.NONE:
                        Console.WriteLine("\tInvalid Command");
                        break;

                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(commandParameters.motorSpeed, commandParameters.motorSpeed);
                        break;

                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-commandParameters.motorSpeed, -commandParameters.motorSpeed);
                        break;

                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        break;

                    case Command.WAIT:
                        int waitMilliseconds = (int)(commandParameters.waitSeconds * 1000);
                        finchRobot.wait(waitMilliseconds);
                        break;

                    case Command.TURNRIGHT:
                        finchRobot.setMotors(commandParameters.motorSpeed, -commandParameters.motorSpeed);
                        break;

                    case Command.TURNLEFT:
                        finchRobot.setMotors(-commandParameters.motorSpeed, commandParameters.motorSpeed);
                        break;

                    case Command.LEDON:
                        finchRobot.setLED(commandParameters.ledBrightness, commandParameters.ledBrightness, commandParameters.ledBrightness);
                        break;

                    case Command.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        break;

                    case Command.NOTEON:
                        finchRobot.noteOn(commandParameters.noteFrequency);
                        break;

                    case Command.NOTEOFF:
                        finchRobot.noteOff();
                        break;

                    case Command.GETTEMPERATURE:
                        Console.WriteLine($"Temperature: {finchRobot.getTemperature()}");
                        break;

                    case Command.GETLEFTLIGHTSENSOR:
                        Console.WriteLine($"Left Light Sensor Value: {finchRobot.getLeftLightSensor()}");
                        break;

                    case Command.GETRIGHTLIGHTSENSOR:
                        Console.WriteLine($"Left Light Sensor Value: {finchRobot.getRightLightSensor()}");
                        break;

                    case Command.DANCE:
                        finchRobot.setLED(commandParameters.ledBrightness, 0, 0);
                        finchRobot.noteOn(commandParameters.noteFrequency);
                        finchRobot.wait(1000);
                        finchRobot.noteOff();
                        finchRobot.wait(500);
                        MrRobotoTune(finchRobot);
                        finchRobot.wait(200);
                        finchRobot.setMotors(commandParameters.motorSpeed, -commandParameters.motorSpeed);
                        finchRobot.wait(3000);
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.setMotors(0, 0);
                        break;
                        
                    case Command.DONE:
                        break;

                    default:
                        break;
                }


            }

            DisplayMenuPrompt("User Programming");
        }

        static void UserProgrammingDisplayCommands(List<Command> commands)
        {
            DisplayScreenHeader("Commands");

            foreach  (Command command in commands)
            {
                Console.WriteLine("\t\t" + command);
            }

            DisplayMenuPrompt("User Programming");
        }

        static void UserProgrammingDisplayGetCommands(List<Command> commands)
        {
            Command command;
            bool validResponse;
            bool isDoneAddingCommands = false;
            string userResponse;

            DisplayScreenHeader("Enter Commands");

            foreach (Command commandName in Enum.GetValues(typeof(Command)))
            {
                if (commandName.ToString() != "NONE")
                {
                    Console.WriteLine("\t\t" + commandName);
                }

            }

            do
            {
                validResponse = true;

                Console.Write("Enter Command: ");
                userResponse = Console.ReadLine().ToUpper();

                if (userResponse != "DONE")
                {
                    //
                    // user entered invalid command
                    //
                    if (!Enum.TryParse(userResponse, out command))
                    {
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a proper command.");
                        DisplayContinuePrompt();
                        validResponse = false;
                    }
                    //
                    // user entered valid command
                    //
                    else
                    {
                        commands.Add(command);
                    }
                }
                else
                {
                    isDoneAddingCommands = true;
                }


            } while (!validResponse || !isDoneAddingCommands);


            DisplayMenuPrompt("User Programming");
        }

        static (int motorSpeed, int ledBrightness, int noteFrequency, double waitSeconds) UserProgrammingDisplayGetCommandParameters()
        {
           (int motorSpeed, int ledBrightness, int noteFrequency, double waitSeconds) commandParameter;
            commandParameter = (0, 0, 0, 0);

            DisplayScreenHeader("Command Parameters");


            commandParameter.motorSpeed = GetValidInteger("Enter Motor Speed [0-255]: ", 0, 255);

            commandParameter.ledBrightness = GetValidInteger("Enter LED Brightness [0-255]: ", 0, 255);

            commandParameter.noteFrequency = GetValidInteger("Enter Note Frequency [100-4000 Hz]: ", 100, 4000);

            commandParameter.waitSeconds = GetValidDouble("Enter Wait Time [seconds]: ", 0, 60);


            DisplayMenuPrompt("User Programming");

            return commandParameter;
        }

        static int GetValidInteger(string prompt)
        {
            int integer = 0;
            bool validResponse;

            do
            {
                validResponse = true;

                Console.Write(prompt);

                if (!int.TryParse(Console.ReadLine(), out integer))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter an integer.");

                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();

                    validResponse = false;

                }


            } while (!validResponse);

            return integer;
        }

        static int GetValidInteger(string prompt, int minimumValue, int maximumValue)
        {
            int integer = 0;
            bool validResponse;

            do
            {
                validResponse = true;

                Console.Write(prompt);

                //
                // user did not enter an integer
                //
                if (!int.TryParse(Console.ReadLine(), out integer))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter an integer.");

                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();

                    validResponse = false;
                }
                //
                // user entered an integer, test for max value
                //
                else
                {
                    if (integer < minimumValue || integer > maximumValue)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Please enter an integer between {minimumValue} and {maximumValue}.");

                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();

                        validResponse = false;
                    }
                }


            } while (!validResponse);



            return integer;

        }


        static double GetValidDouble(string prompt, double minimumValue, double maximumValue)
        {
            double number = 0;
            bool validResponse;

            do
            {
                validResponse = true;

                Console.Write(prompt);

                //
                // user did not enter a double
                //
                if (!double.TryParse(Console.ReadLine(), out number))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter an double.");

                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();

                    validResponse = false;
                }
                //
                // user entered a valid double, test for max value
                //
                else
                {
                    if (number < minimumValue || number > maximumValue)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Please enter an integer between {minimumValue} and {maximumValue}.");

                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();

                        validResponse = false;
                    }
                }

            } while (!validResponse);



            return number;

        }

        #endregion

        #region ALARM SYSTEM

        /// <summary>
        /// *****************************************************************
        /// *                     Alarm System Menu                         *
        /// *****************************************************************
        /// </summary>
        static void AlarmSystemDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            string sensorToMonitor = "";
            string rangeType = "";
            int minMaxLightThresholdValue = 0;
            double minMaxTemperatureThresholdValue = 0.0;
            int timeToMonitor = 0;

            do
            {
                DisplayScreenHeader("Alarm System Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors to Monitor");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Min/Max Light Threshold Value");
                Console.WriteLine("\td) Set Min/Max Temperature Threshold Value");
                Console.WriteLine("\te) Set Time to Monitor");
                Console.WriteLine("\tf) Set Alarm");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        sensorToMonitor = AlarmSystemSetSensorsToMonitor();
                        break;

                    case "b":
                        rangeType = AlarmSystemSetRangeType();
                        break;

                    case "c":
                        minMaxLightThresholdValue = AlarmSystemSetLightThresholdValue(finchRobot, rangeType);
                        break;

                    case "d":
                        minMaxTemperatureThresholdValue = AlarmSystemSetTemperatureValue(finchRobot, rangeType);
                        break;

                    case "e":
                        timeToMonitor = AlarmSystemSetTimetoMonitor();
                        break;

                    case "f":
                        if (sensorToMonitor == "" || rangeType == "" || minMaxLightThresholdValue == 0 || minMaxTemperatureThresholdValue == 0.0 || timeToMonitor ==0)
                        {
                            Console.WriteLine("Please enter all required values.");
                            DisplayContinuePrompt();
                        }
                        else
                        {
                            AlarmSystemSetAlarm(finchRobot, sensorToMonitor, rangeType, minMaxLightThresholdValue, minMaxTemperatureThresholdValue, timeToMonitor);
                        }
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


        static void AlarmSystemSetAlarm(Finch finchRobot, string sensorToMonitor, string rangeType, int minMaxLightThresholdValue, double minMaxTemperatureThresholdValue, int timeToMonitor)
        {
            DisplayScreenHeader("Set Alarm");

            Console.WriteLine($"\tSensors to Monitor: {sensorToMonitor}");
            Console.WriteLine($"\tRange Type: {rangeType}");
            Console.WriteLine($"\tMin/Max Light Threshold Value: {minMaxLightThresholdValue}");
            Console.WriteLine($"\tMin/Max Temperature Threshold Value: {minMaxTemperatureThresholdValue}");
            Console.WriteLine($"\tTime to Monitor: {timeToMonitor}");

            Console.WriteLine("\tPress any key to set the alarm.");
            Console.CursorVisible = false;
            Console.ReadKey();
            Console.CursorVisible = true;

            //bool thresholdExceeded = false;
            //for (int second = 1; second <= timeToMonitor; second++)
            //{
            //    Console.WriteLine($"\t\tTime: {second}");

            //    thresholdExceeded = AlarmSystemThresholdExceeded(finchRobot, sensorToMonitor, rangeType, minMaxThresholdValue);

            //    //
            //    // display message is threshold is exceeded
            //    //
            //    if (thresholdExceeded)
            //    {
            //        Console.WriteLine("\tThreshold Exceeded.");
            //        break;
            //    }

            //    finchRobot.wait(1000);
            //}

            bool lightThresholdExceeded = false;
            bool temperatureThresholdExceeded = false;
            int seconds = 1;
            do
            {
                Console.SetCursorPosition(10, 10);
                Console.WriteLine($"\t\tTime: {seconds++}");
                lightThresholdExceeded = AlarmSystemLightThresholdExceeded(finchRobot, sensorToMonitor, rangeType, minMaxLightThresholdValue);
                temperatureThresholdExceeded = AlarmSystemTemperatureThresholdExceeded(finchRobot, sensorToMonitor, rangeType, minMaxTemperatureThresholdValue);
                finchRobot.wait(1000);

            } while (!(lightThresholdExceeded || temperatureThresholdExceeded) && seconds <= timeToMonitor);

            if (lightThresholdExceeded)
            {
                Console.WriteLine("\tLight Threshold Exceeded.");
                finchRobot.noteOn(200);
                finchRobot.wait(3000);
                finchRobot.noteOff();

            }
            else if (temperatureThresholdExceeded)
            {
                Console.WriteLine("\tTemperature Threshold Exceeded.");
                finchRobot.noteOn(400);
                finchRobot.wait(3000);
                finchRobot.noteOff();
            }
            else
            {
                Console.WriteLine("\tThreshold Not Exceeded.");
                MrRobotoTune(finchRobot);
            }

            DisplayMenuPrompt("Alarm System");
        }

        static bool AlarmSystemLightThresholdExceeded(Finch finchRobot, string sensorToMonitor, string rangeType, int minMaxLightThresholdValue)
        {
            //
            // get current light sensor values
            //
            int currentLeftLightSensorValue;
            int currentRightLightSensorValue;
            currentLeftLightSensorValue = finchRobot.getLeftLightSensor();
            currentRightLightSensorValue = finchRobot.getRightLightSensor();

            Console.WriteLine($"\t\t\tLeft Light Sensor: {currentLeftLightSensorValue}");
            Console.WriteLine($"\t\t\tRight Light Sensor: {currentRightLightSensorValue}");
 

            //
            // test current light sensor values
            //
            bool lightThresholdExceeded = false;

            switch (sensorToMonitor)
            {
                case "left":
                    if (rangeType == "minimum")
                    {
                        lightThresholdExceeded = currentLeftLightSensorValue < minMaxLightThresholdValue;
                    }
                    else
                    {
                        lightThresholdExceeded = currentLeftLightSensorValue > minMaxLightThresholdValue;
                    }
                    break;

                case "right":
                    if (rangeType == "minimum")
                    {
                        lightThresholdExceeded = currentRightLightSensorValue < minMaxLightThresholdValue;
                    }
                    else
                    {
                        lightThresholdExceeded = currentRightLightSensorValue > minMaxLightThresholdValue;
                    }
                    break;

                case "both":
                    if (rangeType == "minimum")
                    {
                        lightThresholdExceeded = (currentLeftLightSensorValue < minMaxLightThresholdValue) || (currentRightLightSensorValue < minMaxLightThresholdValue);
                    }
                    else
                    {
                        lightThresholdExceeded = (currentLeftLightSensorValue > minMaxLightThresholdValue) || (currentRightLightSensorValue > minMaxLightThresholdValue);
                    }
                    break;

                default:
                    Console.WriteLine("\tUnknown Sensor Type.");
                    break;
            }

            return lightThresholdExceeded;
        }

        static bool AlarmSystemTemperatureThresholdExceeded(Finch finchRobot, string sensorToMonitor, string rangeType, double minMaxTemperatureThresholdValue)
        {
            double currentTemperatureValue = 0.0;
            currentTemperatureValue = finchRobot.getTemperature();

            Console.WriteLine($"\t\t\tTemperature: {currentTemperatureValue}");

            //
            // test current light sensor values
            //
            bool temperatureThresholdExceeded = false;


            if (rangeType == "minimum")
            {
                temperatureThresholdExceeded = currentTemperatureValue < minMaxTemperatureThresholdValue;
            }
            else
            {
                temperatureThresholdExceeded = currentTemperatureValue > minMaxTemperatureThresholdValue;
            }

            return temperatureThresholdExceeded;
        }

        /// <summary>
        /// get time to monitor from the user
        /// </summary>
        /// <returns>time to monitor</returns>
        private static int AlarmSystemSetTimetoMonitor()
        {
            int timeToMonitor = 0;

            DisplayScreenHeader("Time to Monitor");

            timeToMonitor = GetValidInteger("Time to Monitor: ", 0, int.MaxValue);

            Console.WriteLine();
            Console.WriteLine($"\tTime to Monitor: {timeToMonitor}");


            DisplayContinuePrompt();

            return timeToMonitor;
        }

        /// <summary>
        /// get the threshold value from the user
        /// </summary>
        /// <returns>threshold value</returns>
        private static int AlarmSystemSetLightThresholdValue(Finch finchRobot, string rangeType)
        {
            int minMaxLightThresholdValue = 0;

            DisplayScreenHeader("Min/Max Light Threshold Value");

            Console.WriteLine($"\tThe Ambient Left Light Sensor Value: {finchRobot.getLeftLightSensor()}");
            Console.WriteLine($"\tThe Ambient Right Light Sensor Value: {finchRobot.getRightLightSensor()}");

            //
            // use the method that brings back an array of values
            //
            //Console.WriteLine($"\tThe Ambient Left Light Sensor Value: {finchRobot.getLeftLightSensor()[0]}");
            //Console.WriteLine($"\tThe Ambient Right Light Sensor Value: {finchRobot.getRightLightSensor()[1]}");


            minMaxLightThresholdValue = GetValidInteger($"\tEnter the {rangeType} Light Threshold Value: ", 0, 255);

            Console.WriteLine();
            Console.WriteLine($"\tLight Threshold Value: {minMaxLightThresholdValue}");

            DisplayContinuePrompt();

            return minMaxLightThresholdValue;

        }


        static double AlarmSystemSetTemperatureValue(Finch finchRobot, string rangeType)
        {
            int minMaxTemperatureThresholdValue = 0;

            DisplayScreenHeader("Min/Max Temperature Threshold Value");

            Console.WriteLine($"\tThe Temperature Sensor Value: {finchRobot.getTemperature()}");


            minMaxTemperatureThresholdValue = GetValidInteger($"\tEnter the {rangeType} Temperature Threshold Value: ", -255, 255);

            Console.WriteLine();
            Console.WriteLine($"\tTemperature Threshold Value: {minMaxTemperatureThresholdValue}");

            DisplayContinuePrompt();

            return minMaxTemperatureThresholdValue;
        }


        /// <summary>
        /// get range type from user
        /// </summary>
        /// <returns>range type</returns>
        static string AlarmSystemSetRangeType()
        {
            string rangeType = "";

            DisplayScreenHeader("Range Type");

            bool validResponse = false;

            do
            {
                Console.Write("\tEnter Range Type: [minimum, maximum]: ");
                rangeType = Console.ReadLine().ToLower();

                if (rangeType == "minimum" || rangeType == "maximum")
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine($"\tYou entered {rangeType}. Please enter 'minimum' or 'maximum'.");
                }

            } while (!validResponse);


            Console.WriteLine();
            Console.WriteLine($"\tRange Type: {rangeType}");

            DisplayContinuePrompt();

            return rangeType;
        }

        /// <summary>
        /// get sensors to monitor from user
        /// </summary>
        /// <returns>sensors to monitor</returns>
        static string AlarmSystemSetSensorsToMonitor()
        {
            string sensorsToMonitor = "";

            DisplayScreenHeader("Sensors to Monitor");

            bool validResponse = false;

            do
            {
                Console.Write("\tEnter Sensors to Monitor [left, right, both]:");
                sensorsToMonitor = Console.ReadLine().ToLower();

                if (sensorsToMonitor == "left" || sensorsToMonitor == "right" || sensorsToMonitor == "both")
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine($"\tYou entered {sensorsToMonitor}. Please enter 'left', 'right' or 'both'.");
                }

            } while (!validResponse);


            Console.WriteLine();
            Console.WriteLine($"\tSensors to Monitor: {sensorsToMonitor}");

 
            DisplayContinuePrompt();

            return sensorsToMonitor;
        }

        #endregion

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
                double fahrenheitTemperature = DataRecorderFahrenheitConversion(temperatures[index]);

                Console.WriteLine(
                    (index + 1).ToString().PadLeft(15) +
                    fahrenheitTemperature.ToString("n2").PadLeft(15)
                    );
            }


        }

        /// <summary>
        /// *****************************************************************
        /// *        Data Recorder > Convert Celcius to Fahrenheit          *
        /// *****************************************************************
        /// </summary>
        static double DataRecorderFahrenheitConversion(double temperatureinCelcius)
        {
            return (temperatureinCelcius * 9 / 5) + 32;
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

            frequencyOfDataPoints = GetValidDouble("\tEnter the Frequency of Data Points: ", 0, int.MaxValue);

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

            numberOfDataPoints = GetValidInteger("\tEnter the Number of Data Points: ", 0, int.MaxValue);
            
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
            MrRobotoTune(finchRobot);

            DisplayMenuPrompt("Talent Show Menu");
        }

        static void MrRobotoTune(Finch finchRobot)
        {
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
