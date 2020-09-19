using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> map_lines = ReadFile("OLMap.txt");
            Graph graph = new Graph(map_lines);

            FileStream fs = new FileStream(@"C:\Users\lenovo\Desktop\Algorithm_Final\Algorithm\Algorithm\Algorithm\Medium.txt", FileMode.Truncate);
            fs.Close();

            List<string> queries = ReadFile("OLQueries.txt");
            graph.StartQueires(queries);

            fs = new FileStream(@"C:\Users\lenovo\Desktop\Algorithm_Final\Algorithm\Algorithm\Algorithm\Medium.txt", FileMode.Append);
            StreamWriter sr = new StreamWriter(fs);

            sr.Write(graph.ExecTimeAll);
            sr.WriteLine(" ms");

            sr.Close();
        }
        public static List<string> ReadFile(string file_name)
        {
            List<string> lines = new List<string>();

            FileStream fs = new FileStream(@"C:\Users\lenovo\Desktop\Algorithm_Final\Algorithm\Algorithm\Algorithm\Test_Cases\" + file_name, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            while (sr.Peek() != -1) //O(N) N:NUMBER OF LINES IN FILE 
            {
                lines.Add(sr.ReadLine());
            }
            fs.Close();

            return lines;
        }
    }
}
