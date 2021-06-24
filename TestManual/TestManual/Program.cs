using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestManual
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
            StreamReader sr = new StreamReader("test_manual_Updated.txt");
            PeekableStreamReaderAdapter peekableStreamReaderAdapter = new PeekableStreamReaderAdapter(sr);

            string line = " ";
            string nextLine;
            int counter = 0;
            bool goNext = false;

            List<Test> testList = new List<Test>();
            while (line != null)
            {
                Test test = new Test();
                Leaf leaf = new Leaf();

                line = peekableStreamReaderAdapter.ReadLine();
                nextLine = peekableStreamReaderAdapter.PeekLine();
                if (nextLine == null)
                {
                    nextLine = "";
                }

                if (line == "")
                {
                    continue;
                }

                //Console.WriteLine(counter);
                List<Leaf> tempList = new List<Leaf>();
                while (line != "")
                {
                    
                    if (line == null)
                    {
                        break;
                    }
                    if (line.Contains("Name: "))
                    {
                        var arr = line.Split(':');
                        test.name = arr[1].Trim();
                    }
                    else if (line.Contains("Laboratory: "))
                    {
                        var arr = line.Split(':');
                        test.lab = arr[1].Trim();
                    }
                    else
                    {
                        //goNext = false;


                        if (line.Contains(":"))
                        {
                            var arr = line.Split(':');
                            leaf.name = arr[0];
                            leaf.val = arr[1];
                            nextLine = peekableStreamReaderAdapter.PeekLine();

                        }
                        else
                        {
                            leaf.val += $" {line}";
                            nextLine = peekableStreamReaderAdapter.PeekLine();
                        }

                        if (nextLine == null)
                        {
                            nextLine = ":";
                        }

                        if (nextLine.Contains(":"))
                        {
                            goNext = true;
                        }

                        if (goNext)
                        {
                            tempList.Add(leaf);
                            goNext = false;
                            leaf = new Leaf();
                        }

                    }//end else
                    line = peekableStreamReaderAdapter.ReadLine();


                    //Console.WriteLine(counter);
                }//inner while
                if (counter == 608)
                {
                    Console.WriteLine();
                }
                Console.WriteLine(counter);
                test.list = tempList;
                testList.Add(test);
                counter++;
            }//outter while

            string json = JsonConvert.SerializeObject(testList);
            File.WriteAllText("Tests.json", json);

            Console.WriteLine("DONE!");
        }
    }
}
