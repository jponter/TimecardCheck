using System;
using System.Collections.Generic;

namespace TimecardCheck
{
    //This is a dirty piece of code that quickly takes a dump of a slack channel whcih contains timecard bot data
    //will scan through and pick out email entries matching Monday morning ie. LATE timecards and add them to a list
    //increments the count of the times that email has been seen resulting in a count of all lateness over the given text dump period


    class Entry
    {
        public string Email = "";
        public int Count = 0;

        public Entry(string email)
        {
            Email = email;
            Count = 1;
        }
    }
    class Program
    {



        static void Main(string[] args)
        {
            string line;
            bool late = false;

            List<Entry> entries = new List<Entry>();




            Console.WriteLine("Reading File");

            //open and read the file
            System.IO.StreamReader file = new System.IO.StreamReader(@".\TimeCard.txt");

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("TimeCard AlertAPP  09:15") || line.Contains("TimeCard AlertAPP  08:15"))
                {
                    //we are in a late session
                    late = true;
                }
                else if (line.Contains("red_circle") && late == true)
                {
                    bool found = false;
                    String splitline = line.Split(' ')[1];
                    if (entries.Count == 0) { entries.Add(new Entry(splitline)); }

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
                        entries.Add(new Entry(splitline));
                    }


                    //System.Console.WriteLine(splitline);

                }
                else if (line.Contains("TimeCard AlertAPP  18:15") || line.Contains("TimeCard AlertAPP  17:15"))
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
            Console.ReadKey();
        }
    }
}
