using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace audio_test
{

    internal class Program
    {
        public static string playlistpath = $@"{Directory.GetCurrentDirectory()}\playlist.txt";

        //playlist function next major feature!
        //Current Code needs improvement first though!!!!
        //Clear Console more often
        //search to check if something exists when adding new songs
        public static int listoutput(List<string> list)
        {
            int i = 0;
            while (i < list.Count)
            {
                Console.WriteLine($"({i}) {list[i]}");
                i++;
            }
            return i-1;
        }
        public static List<string> listgen(string path, string temppath,string remove)
        {
            StreamReader reader = new StreamReader(path);
            List<string> list = new List<string>();
            string buffer = "";
            while((buffer = reader.ReadLine()) != null)
            {
                if (!buffer.EndsWith(remove))
                {
                    list.Add(buffer);
                }
            }
            reader.Close();
            return list;
        }
        public static void playlist(string path, string temppath, int mode) //mode 2 ammend //mode 3 mass add //mode 4 play //mode 5 remove song from all playlists 
        {
            if (mode == 1) //playlist creation
            {
                Console.Clear();
                Console.WriteLine("What is your playlist name?");
                string playname = Console.ReadLine();
                string indivplaypath = $@"{Directory.GetCurrentDirectory()}\{playname}.txt";
                StreamWriter writer = new StreamWriter(playlistpath, true);
                writer.WriteLine(playname);
                writer.WriteLine(indivplaypath);
                writer.Close();
                File.WriteAllText(indivplaypath, "");
                List<string> list = new List<string>();
                List<string> templist = new List<string>();
                StreamReader reader = new StreamReader(path);
                string buffer = "";
                bool loopbool = false;
                while ((buffer = reader.ReadLine()) != null)
                {
                    if (!buffer.EndsWith(".mp3"))
                    {
                        list.Add(buffer);
                    }
                }

                while (loopbool == false)
                {
                    int i = 0;

                    while (i < list.Count)
                    {

                        Console.WriteLine($"({i}) {list[i]}");
                        i++;
                    }
                    string choice = "";
                    Console.WriteLine($"Please select an number to add to the playlist 0 - {i - 1} or STOP to stop");
                    choice = Console.ReadLine();
                    if ((int.TryParse(choice, out int intchoice)) && (intchoice >= 0) && (intchoice <= i-1))
                    {
                        templist.Add(list[intchoice]);
                        list.Remove(list[intchoice]);
                    }
                    else if (choice == "STOP") //could remove templist and write straight into file
                    {
                        StreamWriter writer2 = new StreamWriter(indivplaypath);
                        i = 0;
                        while (i < templist.Count)
                        {
                            writer2.WriteLine(templist[i]);

                            i++;
                        }
                        writer2.Close();
                        reader.Close();
                        mainmenu(path, temppath);
                    }
                }

            }
            else if (mode == 2)
            {
                StreamReader playnameread = new StreamReader(playlistpath);
                List<string> playnamelist = new List<string>();
                string buffer = "";
                int i = 0;
                bool loop = true;
                while (((buffer = playnameread.ReadLine()) != null))
                {
                    if ((!buffer.EndsWith(".txt")))
                    {
                        playnamelist.Add(buffer);


                    }

                }
                playnameread.Close();
                string pathtoplaylist = "";
                while (loop)
                {
                    Console.Clear();
                    i = 0;
                    while (i < playnamelist.Count)
                    {
                        Console.WriteLine($"({i}) {playnamelist[i]}");
                        i++;
                    }
                    string choice = "";
                    Console.WriteLine($"Please select a playlist to ammend 0 - {i - 1}");
                    choice = Console.ReadLine();
                    if ((int.TryParse(choice, out int intchoice) && (intchoice >= 0) && (intchoice <= i - 1)))
                    {
                        pathtoplaylist = search(2, playlistpath, playnamelist[intchoice], temppath);
                        loop = false;
                        continue;
                    }
                    else
                    {
                        continue;
                    }

                }
                loop = true;
                List<string> playlistsonglist = listgen(pathtoplaylist, temppath,".mp3");
                Console.WriteLine("Would you like to remove (1) or add songs (2) to this playlist?");
                int choice2 = int.Parse(Console.ReadLine());
                if( choice2 == 2)
                {
                    
                    Console.Clear();
                    List<string> mainlist = listgen(path, temppath, ".mp3");
                    Console.Clear();
                    Console.WriteLine("Loading may take time.");
                    for (i = listoutput(playlistsonglist); i > 0; i--)
                    {
                        int j = 0;
                        while(j < mainlist.Count)
                        {
                            
                            if (mainlist[j] == playlistsonglist[i])
                            {
                                mainlist.RemoveAt(j);
                                
                                
                            }
                            j++;

                        }
                    }
                    Console.Clear();
                    bool addsong = true;
                    while (addsong)
                    {
                        i = listoutput(mainlist);
                        string choice = "";
                        Console.WriteLine($"Please choose a song to add 0 - {i} or stop to stop");
                        choice = Console.ReadLine();

                        
                        if ((int.TryParse(choice, out int intchoice) && (intchoice >= 0) && (intchoice <= i)))
                        {
                            playlistsonglist.Add(mainlist[intchoice]);
                            mainlist.RemoveAt(intchoice);

                        }
                        else if (choice == "stop")
                        {
                            addsong = false;
                            continue;
                        }

                    }
                    
                } 
                if (choice2 == 1)
                {
                    while (loop)
                    {
                        i = listoutput(playlistsonglist);
                        string choice = "";
                        Console.WriteLine($"Please select a song to remove 0 - {i} or choose STOP to stop");
                        choice = Console.ReadLine();

                        if ((int.TryParse(choice, out int intchoice)) && (intchoice <= i) && (intchoice >= 0))
                        {

                            playlistsonglist.Remove(playlistsonglist[intchoice]);
                            Console.Clear();

                        }
                        else if(choice == "STOP")
                        {
                            loop = false;
                            Console.Clear();
                            continue;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Invalid choice");
                            continue;
                        }

                    }
                }
                else
                {
                    Console.WriteLine("Bad option");
                }
                
                StreamWriter finalwrite = new StreamWriter(pathtoplaylist, false);
                for(i = 0; i < playlistsonglist.Count ; i++)
                {
                    finalwrite.WriteLine(playlistsonglist[i]);
                    finalwrite.WriteLine(search(2, path, playlistsonglist[i], temppath));
                }
                finalwrite.Close();
                mainmenu(path, temppath);
                
                
            }
        }
        public static void foldersearch(string path, string temppath) //Searches for new songs from a folder
        {
            Console.Clear();
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
            Thread.Sleep(1000);
            mainmenu(path, temppath);
        }
        public static void mainmenu(string path, string temppath)//Main menu function
        {
            Console.Clear();
            Console.WriteLine("Add songs (1)");
            Console.WriteLine("Play library (2)");
            Console.WriteLine("Remove songs (3)");
            Console.WriteLine("Add folder (4)");
            Console.WriteLine("Playlist (5)");
            Console.WriteLine("Exit (6)");
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
                else if (choice == 6)
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
                else if (choice == 5)
                {
                    Console.WriteLine("\n(1) Create playlist\n(2) Ammend current playlist");
                    int modechoice = int.Parse(Console.ReadLine());
                    if (modechoice == 1)
                    {
                        playlist(path, temppath, 1);
                    }
                    else if (modechoice == 2)
                    {
                        playlist(path, temppath, 2);
                    }
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
            Console.Clear();
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
                    Console.Clear();
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
                Console.Clear();





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

                        play(location, temppath, hundredlist[intchoice]); //play song
                        i = 0;
                        hundredlist.Clear();
                        reader2.Close();
                        playsongs(path, temppath);//loop back after playback is finished
                    }
                    else if (choice == "STOP") //user commands stop
                    {
                        i = 0;
                        hundredlist.Clear();
                        reader2.Close();
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
        public static void play(string path, string temppath, string name)//Processing playback of songs
        {
            var player = new WMPLib.WindowsMediaPlayer();
            player.URL = path;
            player.controls.play();

            Console.Clear();
            System.Threading.Thread.Sleep(1000);
            double totalDuration = player.currentMedia.duration;

            bool playing = true;

            // selection thread
            Thread progressThread = new Thread(() =>
            {

                while (playing)
                {
                    Console.WriteLine($"{name}");
                    Console.WriteLine("Player controls: pause, play, stop");
                    string command = Console.ReadLine();
                    if (command == "pause")
                    {
                        player.controls.pause();
                        Console.ReadLine();
                        player.controls.play();
                        Console.Clear();
                    }
                    else if (command == "stop")
                    {
                        Console.Clear();
                        player.controls.stop();
                        playing = false;
                    }
                }

            });

            progressThread.IsBackground = true;  // thread dies with main program
            progressThread.Start();

            // progress on main thread
            double remaining = 999;
            while (playing)
            {
                Thread.Sleep(50);
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
                System.Threading.Thread.Sleep(200);
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
                case 3://check if something already exists in a given context
                    StreamReader reader3 = new StreamReader(path); //path is context target is item to check
                    string buffer3 = "";
                    while ((buffer3 = reader3.ReadLine()) != null)
                    {
                        if (buffer3 == target)
                        {
                            reader3.Close();
                            return "1";
                        }
                        else
                        {

                            continue;
                        }
                    }
                    reader3.Close();
                    return "2";




            }
            return "";
        }
        public static void remove(string path, string temppath)//Remove songs needs improvement
        {
            Console.Clear();
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
            if (File.Exists(playlistpath)) //check for song index
            {
                Console.WriteLine("File found!");
            }
            else
            {
                Console.WriteLine("File not found creating file");
                File.WriteAllText(playlistpath, "");
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
//364 lines of bullshit!!!!