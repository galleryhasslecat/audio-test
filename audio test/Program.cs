using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;

namespace audio_test
{
    internal class Program
    {

        //playlist function next major feature!
        //Current Code needs improvement first though!!!!
        //Clear Console more often
        public static void foldersearch(string path, string temppath) //Searches for new songs from a folder
        {
            StreamWriter writer = new StreamWriter(path, true, Encoding.UTF8);
            Console.WriteLine("Where is the folder to search");
            string folderpath = Console.ReadLine();
            string[] files = Directory.GetFiles(folderpath, "*.mp3"); //ensure files found end in .mp3
            foreach (var file in files) //scan each file
            {

                string location = Path.GetDirectoryName(file);
                string name = Path.GetFileName(file);
                int namelenth = name.Length;

                location = location + @"\" + name; //ensure formatting is correct for the location
                name = name.Substring(0, namelenth - 4); //ensure formatting is correct for the name
                Console.WriteLine(name);

                writer.WriteLine(name);

                writer.WriteLine(location);
            }
            writer.Close();
            mainmenu(path, temppath);
        }
        public static void mainmenu(string path, string temppath)//Main menu function
        {
            Console.WriteLine("Add songs (1)");
            Console.WriteLine("Play library (2)");
            Console.WriteLine("Remove songs (3)");
            Console.WriteLine("Add folder (4)");
            Console.WriteLine("Exit (5)");
            string strchoice = Console.ReadLine();

            if (int.TryParse(strchoice, out int choice)) //Main option selection
            {
                if (choice == 1)
                {
                    addsongs(path, temppath);
                }
                else if (choice == 2)
                {
                    playsongs(path, temppath);
                }
                else if (choice == 5)
                {
                    Environment.Exit(0);
                }
                else if (choice == 3)
                {
                    remove(path, temppath);
                }
                else if (choice == 4)
                {
                    foldersearch(path, temppath);
                }
                else
                {
                    mainmenu(path, temppath);
                }
            }
            else //loop back if bad option
            {
                Console.WriteLine("Invalid choice");
                mainmenu(path, temppath);
            }
        }
        public static void addsongs(string path, string temppath)//Add new individual songs
        {
            bool addsong = true;

            string songname = "";
            string pathtosong = "";
            StreamWriter writer = new StreamWriter(path, true, Encoding.UTF8);
            while (addsong == true)
            {
                Console.WriteLine("Name of song to be added");
                songname = Console.ReadLine();

                Console.WriteLine("Path to song");
                pathtosong = Console.ReadLine();
                if ((pathtosong.EndsWith(".mp3")) && (songname != ""))
                {

                }
                else //quit to main menu if insupported file or invalid location
                {
                    Console.WriteLine("Invalid choices quitting to main menu!");
                    addsong = false;
                    continue;
                }
                writer.WriteLine(songname);
                writer.WriteLine(pathtosong);

                Console.WriteLine("Add another song? (y/n)");
                if (Console.ReadLine() == "y")
                {
                    continue;
                }
                else
                {
                    addsong = false;
                }


            }

            writer.Close();
            mainmenu(path, temppath);

        }
        public static void playsongs(string path, string temppath)//Song selection
        {
            bool playsong = true;




            while (playsong == true) //inialise infinite loop
            {






                string buffer = "junk";
                bool hundredreached = false; //initialise variables

                List<string> hundredlist = new List<string> { }; //create list to store song names
                int i = 0;
                StreamReader reader2 = new StreamReader(path, Encoding.UTF8);
                while (hundredreached == false) //not the best coding practice
                {


                    while (((buffer = reader2.ReadLine()) != null)) //presence check
                    {

                        if (!buffer.EndsWith(".mp3")) //ensure correct item is being added
                        {

                            hundredlist.Add(buffer);
                            Console.WriteLine($"({i}) {buffer}");
                            i++;
                        }


                    }
                    Console.WriteLine($"Select a number 0 - {i - 1} or STOP to stop");
                    string choice = Console.ReadLine(); //user selects song
                    int intchoice = 0;

                    if ((int.TryParse(choice, out intchoice)) && (intchoice >= 0) && (intchoice <= i)) //range check
                    {

                        string location = search(2, path, hundredlist[intchoice], temppath); //search for path of selected song
                        Console.WriteLine(hundredlist[intchoice]);
                        play(location, temppath); //play song
                        i = 0;
                        hundredlist.Clear();
                        reader2.Close();
                        playsongs(path, temppath);//loop back after playback is finished
                    }
                    else if (choice == "STOP") //user commands stop
                    {
                        mainmenu(path, temppath);
                    }

                    else
                    {

                        Console.WriteLine("Invalid choice");
                    }

                }
                reader2.Close();







            }

        }
        public static void play(string path, string temppath)//Processing playback of songs
        {
            var player = new WMPLib.WindowsMediaPlayer();
            player.URL = path;
            player.controls.play();
            
            
            System.Threading.Thread.Sleep(500);
            double totalDuration = player.currentMedia.duration;
            
            bool playing = true;
            
            // Progress thread
            Thread progressThread = new Thread(() =>
            {
                double remaining = 999;
                while (playing)
                {
                    if (remaining <= 1)
                    {
                        playing = false;
                        return;
                        
                    }
                    double currentPos = player.controls.currentPosition;
                    remaining = totalDuration - currentPos;
                    
                    int mins = (int)remaining / 60;
                    int secs = (int)remaining % 60;
                    
                    Console.Write($"\rTime left: {mins:D2}:{secs:D2}  ");
                    System.Threading.Thread.Sleep(1000);
                }
            });
            
            progressThread.IsBackground = true;  // thread dies with main program
            progressThread.Start();
            
            // Input on main thread
            while (playing)
            {
                string command = Console.ReadLine();
                if (command == "pause")
                {
                    player.controls.pause();
                    Console.ReadLine();
                    player.controls.play();
                }
                else if (command == "stop")
                {
                    player.controls.stop();
                    playing = false;
                }
            }

        }
        public static string search(int mode, string path, string target, string temppath)//Search function with 2 specific modes
        {
            switch (mode)
            {
                case 1: //Display all song names 
                    StreamReader reader = new StreamReader(path, Encoding.UTF8);
                    string buffer = reader.ReadLine();
                    while (buffer != null)
                    {
                        if (buffer.EndsWith(".mp3"))
                        {

                        }
                        else
                        {
                            Console.WriteLine(buffer);
                        }
                        buffer = reader.ReadLine();
                    }
                    reader.Close();
                    break;
                case 2: //Find Location of a song based on title
                    StreamReader reader2 = new StreamReader(path, Encoding.UTF8);
                    string buffer2 = reader2.ReadLine();
                    while (buffer2 != null)
                    {
                        if (buffer2 == target)
                        {
                            string memorysaver = reader2.ReadLine(); //alows us to close reader2
                            reader2.Close();
                            return memorysaver;
                        }
                        buffer2 = reader2.ReadLine();
                    }
                    reader2.Close();
                    break;



            }
            return "";
        }
        public static void remove(string path, string temppath)//Remove songs needs improvement
        {
            StreamReader tempreader = new StreamReader(path, Encoding.UTF8);
            StreamWriter tempwriter = new StreamWriter(temppath, false, Encoding.UTF8);
            string tempbuffer = tempreader.ReadToEnd();
            tempwriter.Write(tempbuffer);
            tempreader.Close();
            tempwriter.Close();
            string line = null;
            search(1, path, "", temppath);
            Console.WriteLine("Which to remove");
            string line_to_delete = Console.ReadLine();
            string line_to_delete2 = search(2, path, line_to_delete, temppath);

            using (StreamReader reader = new StreamReader(temppath, Encoding.UTF8))
            {
                using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if ((String.Compare(line, line_to_delete) == 0) || (String.Compare(line, line_to_delete2) == 0))
                        {
                            continue;
                        }


                        writer.WriteLine(line);
                    }
                }
            }
            mainmenu(path, temppath);
        }
        static void Main(string[] args)//initialisation
        {

            Console.OutputEncoding = Encoding.UTF8;
            string path = Directory.GetCurrentDirectory();
            string temppath = path + @"\tempfile.txt";
            path = path + @"\file.txt";

            if (File.Exists(path)) //check for song index
            {
                Console.WriteLine("File found!");
            }
            else
            {
                Console.WriteLine("File not found creating file");
                File.WriteAllText(path, "");
            }
            if (File.Exists(temppath)) //check for temporary song index used for deletion
            {
                Console.WriteLine("File found!");
            }
            else
            {
                Console.WriteLine("File not found creating file");
                File.WriteAllText(temppath, "");
            }
            mainmenu(path, temppath);
        }
    }
}
