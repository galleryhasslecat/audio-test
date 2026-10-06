using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace audio_test
{
    internal class Program
    {


        public static void foldersearch(string path, string temppath)
        {
            StreamWriter writer = new StreamWriter(path, true);
            Console.WriteLine("Where is the folder to search");
            string folderpath = Console.ReadLine();
            string[] files = Directory.GetFiles(folderpath, "*.mp3");
            foreach (var file in files)
            {

                string location = Path.GetDirectoryName(file);
                string name = Path.GetFileName(file);
                int namelenth = name.Length;
                
                location =   location + @"\" +  name;
                name = name.Substring(0, namelenth-4);
                Console.WriteLine(name);

                writer.WriteLine(name);
                
                writer.WriteLine(location);
            }
            writer.Close();
            mainmenu(path, temppath);
        }
        public static void mainmenu(string path, string temppath)
        {
            Console.WriteLine("Add songs (1)");
            Console.WriteLine("Play library (2)");
            Console.WriteLine("Remove songs (3)");
            Console.WriteLine("Add folder (4)");
            Console.WriteLine("Exit (5)");
            string strchoice = Console.ReadLine();

            if (int.TryParse(strchoice, out int choice))
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
            else
            {
                Console.WriteLine("Invalid choice");
                mainmenu(path, temppath);
            }
        }
        public static void addsongs(string path, string temppath)
        {
            bool addsong = true;

            string songname = "";
            string pathtosong = "";
            StreamWriter writer = new StreamWriter(path, true);
            while (addsong == true)
            {
                Console.WriteLine("Name of song to be added");
                songname = Console.ReadLine();

                Console.WriteLine("Path to song");
                pathtosong = Console.ReadLine();
                if ((pathtosong.EndsWith(".mp3")) && (songname != ""))
                {

                }
                else
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
        public static void playsongs(string path, string temppath)
        {
            bool playsong = true;

            string songname = "";


            while (playsong == true)
            {
                search(1, path, "", temppath);
                Console.WriteLine("What song would you like? (Say STOP to end music playback)");
                songname = Console.ReadLine();
                if (songname == "STOP")
                {
                    mainmenu(path, temppath);
                }
                bool pathflag = false;

                string buffer = "junk";
                bool played = false;
                StreamReader reader2 = new StreamReader(path);
                while (buffer != null)
                {
                    buffer = reader2.ReadLine();
                    if (pathflag == true)
                    {
                        play(buffer, temppath);
                        played = true;
                        pathflag = false;
                    }
                    if (buffer == songname)
                    {
                        pathflag = true;
                        Console.WriteLine("Found!");
                    }
                    else
                    {

                        if ((buffer == null) && (played == false))
                        {
                            Console.WriteLine("Not found :( ");

                        }


                    }

                }
                reader2.Close();
            }

        }
        public static void play(string path, string temppath)
        {
            bool playbool = true;
            var player = new WMPLib.WindowsMediaPlayer();
            player.URL = path;
            Console.WriteLine("Player controls play, stop, pause");
            player.controls.play();
            while (playbool == true)
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
                    playbool = false;
                }
                else
                {
                    continue;
                }
            }

        }
        public static string search(int mode, string path, string target, string temppath)
        {
            switch (mode)
            {
                case 1:
                    StreamReader reader = new StreamReader(path);
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
                case 2:
                    StreamReader reader2 = new StreamReader(path);
                    string buffer2 = reader2.ReadLine();
                    while (buffer2 != null)
                    {
                        if (buffer2 == target)
                        {
                            string memorysaver = reader2.ReadLine();
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
        public static void remove(string path, string temppath)
        {
            StreamReader tempreader = new StreamReader(path);
            StreamWriter tempwriter = new StreamWriter(temppath);
            string tempbuffer = tempreader.ReadToEnd();
            tempwriter.Write(tempbuffer);
            tempreader.Close();
            tempwriter.Close();
            string line = null;
            search(1, path, "", temppath);
            Console.WriteLine("Which to remove");
            string line_to_delete = Console.ReadLine();
            string line_to_delete2 = search(2, path, line_to_delete, temppath);

            using (StreamReader reader = new StreamReader(temppath))
            {
                using (StreamWriter writer = new StreamWriter(path))
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
        static void Main(string[] args)
        {


            string path = Directory.GetCurrentDirectory();
            string temppath = path + @"\tempfile.txt";
            path = path + @"\file.txt";

            if (File.Exists(path))
            {
                Console.WriteLine("File found!");
            }
            else
            {
                Console.WriteLine("File not found creating file");
                File.WriteAllText(path, "");
            }
            if (File.Exists(temppath))
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
