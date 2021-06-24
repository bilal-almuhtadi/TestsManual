using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace modifyText
{
    class Program
    {

        public class PeekableStreamReaderAdapter
        {
            private StreamReader Underlying;
            private Queue<string> BufferedLines;

            public PeekableStreamReaderAdapter(StreamReader underlying)
            {
                Underlying = underlying;
                BufferedLines = new Queue<string>();
            }

            public string PeekLine()
            {
                string line = Underlying.ReadLine();
                if (line == null)
                    return null;
                BufferedLines.Enqueue(line);
                return line;
            }


            public string ReadLine()
            {
                if (BufferedLines.Count > 0)
                    return BufferedLines.Dequeue();
                return Underlying.ReadLine();
            }
        }

        static void Main(string[] args)
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader("test_manual.txt");
            StreamWriter sw = new StreamWriter("test_manual_Updated.txt");

            PeekableStreamReaderAdapter peekableStreamReaderAdapter = new PeekableStreamReaderAdapter(sr);
            string line = " ";
            string nextLine;
            int counter = 0;

            while (line != null)
            {


                line = peekableStreamReaderAdapter.ReadLine();
                nextLine = peekableStreamReaderAdapter.PeekLine();



                if (nextLine == null)
                {
                    nextLine = "";
                }

                if (line == null)
                {
                    continue;
                }

                if (line == "" || line == " " || line.Contains("of 231 16/11/2020"))
                {
                    continue;
                }


                if (nextLine.Contains("Laboratory:"))
                {
                    sw.WriteLine($"\n\n\nName: {line}");
                }
                else if (line.Contains("Special requirements") || line.Contains("comments:"))
                {
                    sw.Write($"{line} ");
                }
                else
                {
                    sw.WriteLine(line);
                }

                counter++;
            }

            sr.Close();
            sw.Close();
            Console.WriteLine("DONE!");

            StreamReader sr1 = new StreamReader("test_manual_Updated.txt");
            StreamWriter sw1 = new StreamWriter("parameters.txt");
            line = " ";
            List<string> paraList = new List<string>();
            List<string> checkingList = new List<string>();


            while (line != null)
            {
                line = sr1.ReadLine();
                if(line == null)
                {
                    continue;
                }

                if(line.Contains(":") && !line.Contains("WARNING:"))
                {
                    var arr = line.Split(':');
                    checkingList = paraList.FindAll(x => x == arr[0]);
                    if (checkingList.Count() == 0)
                    {
                        paraList.Add(arr[0]);
                    }

                    checkingList.Clear();
                }
            }
            sr1.Close();
            paraList.Sort();
            //var noDupes = new HashSet<string>(paraList);
            //var noDublicates = paraList.Distinct().ToList<string>();
            foreach (string item in paraList)
            {
                //var arr = item.Split(':');
                sw1.WriteLine(item);
            }


            sw1.Close();

        }



    }
}
