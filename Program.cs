using System;
using System.Collections.Generic;

namespace TimecardCheck
{
    //This is a dirty piece of code that quickly takes a dump of a slack channel whcih contains timecard bot data
    //will scan through and pick out email entries matching Monday morning ie. LATE timecards and add them to a list
    //increments the count of the times that email has been seen resulting in a count of all lateness over the given text dump period


    class Entry
    {
        //will hold each individual persons email and number of times their timecard was late
        public string Email { get; set; }
        public int Count { get; set; }

       
    }
    class Program
    {
        //filename of the full text dump from SlackBot channel
        private static string FileNameToRead = @".\TimeCard.txt";
        //set the two below strings to match the local time slackbot gets time card alerts on a Monday
        private static string TimeCardBotText1 = "TimeCard AlertAPP  09:15";
        private static string TimeCardBotText2 = "TimeCard AlertAPP  08:15";
        //set the two below strings to match the local time slackbot gets time card alerts on a Friday
        private static string TimeCardBotText3 = "TimeCard AlertAPP  18:15";
        private static string TimeCardBotText4 = "TimeCard AlertAPP  17:15";



        static void Main(string[] args)
        {
            string line;
            bool late = false;

            List<Entry> entries = new List<Entry>();




            Console.WriteLine("Reading File...\n");

            //open and read the file
            System.IO.StreamReader file = new System.IO.StreamReader(FileNameToRead);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains(TimeCardBotText1) || line.Contains(TimeCardBotText2))
                {
                    //we are in a late session
                    late = true;
                }
                else if (line.Contains("red_circle") && late == true)
                {
                    bool found = false;
                    String splitline = line.Split(' ')[1];
                    if (entries.Count == 0) { entries.Add(new Entry() { Email = splitline, Count = 1 }); }

                    for (int i = 0; i < entries.Count; i++)
                    {
                        if (entries[i].Email == splitline)
                        {
                            entries[i].Count++;
                            found = true;
                        }
                    }

                    if (found == false)
                    {
                        entries.Add(new Entry() { Email = splitline, Count = 1 });
                    }


                    //System.Console.WriteLine(splitline);

                }
                else if (line.Contains(TimeCardBotText3) || line.Contains(TimeCardBotText4))
                {
                    late = false;
                }
                else
                {
                    //discard line
                }




            }





            //Console.ReadKey();
            for (int i = 0; i < entries.Count; i++)
            {
                Console.WriteLine(entries[i].Email + " " + entries[i].Count);
            }

            Console.WriteLine("\nPress any key to exit...\n");
            Console.ReadKey();
        }
    }
}
