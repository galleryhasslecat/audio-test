using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace audio_test
{
    internal class Program
    {
        public static void mainmenu()
        {
            Console.WriteLine("Add songs (1)");
            Console.WriteLine("Play library (2)");
            int choice = int.Parse(Console.ReadLine());
            if (choice == 1)
            {
                addsongs();
            }
            else if (choice == 2)
            {
                playsongs();
            }
        }
        public static void addsongs()
        {
            bool addsong = true;
            Console.WriteLine("Where is the txt!!!");
            string pathtotxt = Console.ReadLine();
            string songname = "";
            string pathtosong = "";
            StreamWriter writer = new StreamWriter(pathtotxt, true);
            while (addsong == true)
            {               
                Console.WriteLine("Name of song to be added");
                songname = Console.ReadLine();
                writer.WriteLine(songname);
                Console.WriteLine("Path to song");
                pathtosong = Console.ReadLine();
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
            mainmenu();

        }
        public static void playsongs()
        {
            bool playsong = true;
            Console.WriteLine("Where is the txt!!!");
            string pathtotxt = Console.ReadLine();
            string songname = "";
            string pathtosong = "";
            StreamReader reader = new StreamReader (pathtotxt);
            while (playsong = true)
            {
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine("What song would you like?");
                songname = Console.ReadLine();
                bool pathflag = false;
                reader.DiscardBufferedData();
                string buffer = "junk";
                StreamReader reader2 = new StreamReader(pathtotxt);
                while (buffer != null)
                {
                    buffer = reader2.ReadLine();
                    if (pathflag == true)
                    {
                        play(buffer);
                    } 
                    if (buffer == songname)
                    {
                        pathflag = true;
                        Console.WriteLine("Found!");
                    }
                    else
                    {
                        Console.WriteLine("Not found :( ");
                        continue;
                    }
                }
            }

        }
        public static void play(string path)
        {
            bool playbool = true;
            var player = new WMPLib.WindowsMediaPlayer();
            player.URL = path;
            player.controls.play();
            while(playbool == true)
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
        static void Main(string[] args)
        {
            var player = new WMPLib.WindowsMediaPlayer();

            
            mainmenu();
        }
    }
}
