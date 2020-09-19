using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Algorithm
{
    public class Intersection
    {
        public double X_coordinate;
        public double Y_coordinate;
        public Intersection()
        {
            X_coordinate = 0;
            Y_coordinate = 0;
        }
        public Intersection(double X, double Y)
        {
            X_coordinate = X;
            Y_coordinate = Y;
        }
    }
    public class Node : IComparable<Node>
    {
        public int Intersection_ID;
        public double Road_Length;
        public double Road_Time;

        public Node()
        {
            Intersection_ID = 0;
            Road_Length = 0;
            Road_Time = 0;
        }
        public Node(int ID, double Len, double T)
        {
            Intersection_ID = ID;
            Road_Length = Len;
            Road_Time = T;
        }

        public int CompareTo(Node other)
        {
            return this.Road_Time.CompareTo(other.Road_Time);
        }
    }
    public class Graph
    {
        public int number_of_intersections;
        public int number_of_roads;
        public int number_of_queries;
        public int source_node;
        public int destination_node;
        public int ExecTimeAll;

        public List<Intersection> Intersections;
        public List<List<Node>> adj_list;
        public List<int> Path;
        public double[] ShortestTime;
        public double[] Length;
        public int[] MyParent;
        public bool[] VisitedNode;
        public Graph()
        {
             number_of_intersections=0;
             number_of_roads=0;
             number_of_queries=0;
             source_node = 0;
             destination_node = 0;
             ExecTimeAll = 0;
        }
        //<<<<< OVERALL CONSTRUCTION OF GRAPH O(V+E) >>>>>
        public Graph(List<string> lines)
        {
            ExecTimeAll = 0;                                                                           /* O(1) */
            Intersections = new List<Intersection>();                                                 /* O(1) */
            int i = 0;                                                                               /* O(1) */
            number_of_intersections = int.Parse(lines[i]);                                          /* O(1) */
            i++;                                                                                   /* O(1) */
            // number of vertices * O(body) = O(number of vertices) * O(1) = O(number of vertices)  O(V)
            for (int j = 0; j < number_of_intersections; j++, i++) //j iterates on num of intersections,i iteraters on num of lines of intersections
            {
                string[] _Intersection = lines[i].Split(' ');                                     /* O(1) */
                int Intersection_ID = int.Parse(_Intersection[0]);                               /* O(1) */
                double X_coordinate = Convert.ToDouble(_Intersection[1]);                       /* O(1) */
                double Y_coordinate = Convert.ToDouble(_Intersection[2]);                      /* O(1) */
                Intersections.Add(new Intersection(X_coordinate, Y_coordinate));              /* O(1) */
            }

            source_node = number_of_intersections;                                             /* O(1) */
            number_of_intersections++;                                                        /* O(1) */
            destination_node = number_of_intersections;                                     /* O(1) */
            adj_list = new List<List<Node>>();                                             /* O(1) */
            // number_of_Vertcies * O(body) = O(number_of_Vertcies) * O(1)= O(V)
            for (int j = 0; j < number_of_intersections; j++) //j iterates on num of intersections   
            {
                adj_list.Add(new List<Node>()); /* O(1) */
            }


            number_of_roads = int.Parse(lines[i]);                                     /* O(1) */
            i++;/* O(1) */
            // number of edges * O(body) = O(number of edges ) * O(1) = O(E)
            for (int j = 0; j < number_of_roads; j++, i++) //j iterates on num of roads  i iteraters on num of lines of roads 
            {
                string[] R = lines[i].Split(' ');                                   /* O(1) */
                int First_Intersection_ID = int.Parse(R[0]);                       /* O(1) */
                int Second_Intersection_ID = int.Parse(R[1]);                     /* O(1) */
                double Road_Length = Convert.ToDouble(R[2]);                     /* O(1) */
                double Road_Speed = int.Parse(R[3]);                            /* O(1) */
                double Raod_Time = Road_Length / Road_Speed;                   /* O(1) */

                adj_list[First_Intersection_ID].Add(new Node(Second_Intersection_ID, Road_Length, Raod_Time));      /* O(1) */
                adj_list[Second_Intersection_ID].Add(new Node(First_Intersection_ID, Road_Length, Raod_Time));     /* O(1) */
            }

            ShortestTime = new double[number_of_intersections + 3];           /* O(1) */
            Length = new double[number_of_intersections + 3];                /* O(1) */
            MyParent = new int[number_of_intersections + 3];                /* O(1) */
            VisitedNode = new bool[number_of_intersections + 3];           /* O(1) */
            Path = new List<int>();                                       /* O(1) */
        }

        //<<<<<OVERALL O(V) V:number_of_Vertcies >>>>>
        public void ClearData() //FUNCTION O(V) V:number_of_Vertcies
        {
            for (int i = 0; i <= number_of_intersections; i++)
            {
                ShortestTime[i] = Double.MaxValue; //O(1)  INTIALIZE WITH INFINITY
                VisitedNode[i] = false;           //O(1)
                Length[i] = 0;                   //O(1)
                MyParent[i] = -1;               //O(1)
            } //LOOP O(N) N:number_of_intersections
        }
        //<<<<< OVERALL CONSTRUCTION OF DIJKSTRA O(E log V) >>>>>
        public void Dijkstra(int StartNode)
        {
            ClearData();                                           //O(V) V:number_of_Vertcies
            PriorityQueue<Node> SetOfNodes = new PriorityQueue<Node>();   // O(1)

            SetOfNodes.Enqueue(new Node(StartNode, 0, 0));  //add source            //O(log V) V:number_of_Vertcies

            int ListLen;         //num of neigbours                                 //O(1) 
            Node TopNode, CurNode;         //top:with min cost   ,   cur:Next Node                      // O(1)
            while (true) //WHILE LOOP = O(N log N)
            {
                if (SetOfNodes.Count() == 0) //finish
                {
                    break;                                          //O(1)
                }
                TopNode = SetOfNodes.Peek();                          //O(1) 
                if (TopNode.Intersection_ID == destination_node) // reach to destination      
                {
                    break;                                       //O(1)
                }
                SetOfNodes.Dequeue();                     // O(log v) number of vertices

                if (VisitedNode[TopNode.Intersection_ID] == true) //lw zortha 2bl kida mro7lhash tany
                {
                    continue;   //O(1)
                }
                else
                {
                    VisitedNode[TopNode.Intersection_ID] = true;         //O(1)
                }

                ListLen = adj_list[TopNode.Intersection_ID].Count;      //O(1)   NUMBER OF NAIGBOURS
                for (int i = 0; i < ListLen; i++)                      //FOR LOOP O(E) E= listLen  NUMBER OF EDGES
                {
                    CurNode = adj_list[TopNode.Intersection_ID][i]; //O(1)  //loop of neigbours of the node to get min
                    if (TopNode.Road_Time + CurNode.Road_Time < ShortestTime[CurNode.Intersection_ID])     //O(1) RELAXATION
                    {
                        ShortestTime[CurNode.Intersection_ID] = TopNode.Road_Time + CurNode.Road_Time;   //O(1)
                        Length[CurNode.Intersection_ID] = TopNode.Road_Length + CurNode.Road_Length;    //O(1)
                        MyParent[CurNode.Intersection_ID] = TopNode.Intersection_ID;                   //O(1)
                        SetOfNodes.Enqueue(new Node(CurNode.Intersection_ID, Length[CurNode.Intersection_ID], ShortestTime[CurNode.Intersection_ID]));  //O(log v)
                    }//IF CONDITION = O(log V)
                } //WHOLE FOR LOOP =O(E log V)
            }

            return;  //O(1)
        }
        public void StartQueires(List<string> lines)   //FUNCTION COMPLXITY = O(N)
        {
            int i = 0;                                                 //O(1)
            number_of_queries = int.Parse(lines[i]);                  //O(1)
            for (i = 1; i <= number_of_queries; i++) 
            {
                string[] _Query = lines[i].Split(' ');                  //O(1)
                double Source_X = Convert.ToDouble(_Query[0]);         //O(1)
                double Source_Y = Convert.ToDouble(_Query[1]);        //O(1)
                double Destination_X = Convert.ToDouble(_Query[2]);  //O(1)
                double Destination_Y = Convert.ToDouble(_Query[3]); //O(1)
                double Rad = int.Parse(_Query[4]);                 //O(1)

                AnswerQuery(Source_X, Source_Y, Destination_X, Destination_Y, Rad / 1000.0); //O(1)
            } //LOOP O(N) N:number_of_queries
        }
        public void CalculateDistance(int ExecTime) //FUNCTION COMPLEXITY O(1)
        {
            ExecTimeAll += ExecTime;                      //O(1)

            int StartNodeInPath = Path[1];               //O(1)
            int EndNodeInPath = Path[Path.Count - 2];  //O(1)

            
            double DistanceOfCar = Length[EndNodeInPath] - Length[StartNodeInPath];    //O(1)
            double DistanceOfWalk = Length[destination_node] - DistanceOfCar;         //O(1)
            double DistanceOfAll = DistanceOfCar + DistanceOfWalk;                   //O(1)

            FileStream fs = new FileStream(@"C:\Users\lenovo\Desktop\Algorithm_Final\Algorithm\Algorithm\Algorithm\Answer.txt", FileMode.Append);  //O(1)
            StreamWriter sr = new StreamWriter(fs);                                                                               //O(1)

            foreach (var node in Path)              //O(n) , number of nodes in path
            {
                if (node != source_node && node != destination_node)          //O(1)
                {
                    sr.Write(node + " ");                                    //O(1)
                }
            }
            sr.WriteLine();

            sr.Write(Math.Round(ShortestTime[destination_node] * 60.0, 2));              //O(1)
            sr.WriteLine(" mins");                                                      //O(1)

            sr.Write(Math.Round(DistanceOfAll, 2));                                   //O(1)
            sr.WriteLine(" km");                                                     //O(1)

            sr.Write(Math.Round(DistanceOfWalk, 2));                               //O(1)
            sr.WriteLine(" km");                                                  //O(1)

            sr.Write(Math.Round(DistanceOfCar, 2));                             //O(1)
            sr.WriteLine(" km");                                               //O(1)
            

            sr.WriteLine("");                                             //O(1)

            sr.Close();
        }
        //TOTAL O(V) v num of vertics in path
        public void GetPath()
        {
            Path.Clear();                                           //O(1)
            int CurNode = destination_node;                        //O(1)
            while (MyParent[CurNode] != -1)                       //LOOP O(V) V:number_of_Vertcies
            {
                Path.Add(CurNode);                              //O(1)
                CurNode = MyParent[CurNode];                   //O(1)
            }
            Path.Add(CurNode);                               //O(1) N:number_of_intersections
            Path.Reverse();                                 //O(V) O(V) V:number_of_Vertcies
        }
        public void AnswerQuery(double Source_X, double Source_Y, double Destination_X, double Destination_Y, double R)
        {
            var StopWatch = new System.Diagnostics.Stopwatch();            //O(1)
            StopWatch.Start();                                            //O(1)

            //         ID,  length
            List<Tuple<int, double>> SourceNodes = ChooseAvailableNodes(Source_X, Source_Y, R);                     //O(1)
            List<Tuple<int, double>> DestinationNodes = ChooseAvailableNodes(Destination_X, Destination_Y, R);     //O(1)

            
            // add source_node , destination_node To GRAPH
            int Len = SourceNodes.Count;                       //O(1)             
            for (int i = 0; i < Len; i++)                     //LOOP: O(N*V)  N:num of available source nodes
            {
                adj_list[source_node].Add(new Node(SourceNodes[i].Item1, SourceNodes[i].Item2, SourceNodes[i].Item2 / 5.0));     //O(N)  N:number_of_intersections
            }

            Len = DestinationNodes.Count; //O(1) 
            for (int i = 0; i < Len; i++)  //LOOP = O(N^2)  N:Len * N:number_of_intersections 
            {
                adj_list[DestinationNodes[i].Item1].Add(new Node(destination_node, DestinationNodes[i].Item2, DestinationNodes[i].Item2 / 5.0)); //O(N)
            }
           
            Dijkstra(source_node);  //O(E log V)

            StopWatch.Stop(); //O(1)


            GetPath();  //O(V) verticies in path
            CalculateDistance(int.Parse(StopWatch.ElapsedMilliseconds.ToString())); //O(1)

            // Clear 
            Len = DestinationNodes.Count; //O(1)
            for (int i = 0; i < Len; i++)  // O(N^2)   N:Len * N:number_of_intersections 
            {
                Node LastRaod = adj_list[DestinationNodes[i].Item1].Last();  //O(1)
                adj_list[DestinationNodes[i].Item1].Remove(LastRaod);  //O(N) N:number_of_intersections 
            }
            adj_list[source_node].Clear(); //O(1)
        }
        public List<Tuple<int, double>> ChooseAvailableNodes(double X, double Y, double R)
        {
            List<Tuple<int, double>> nodes = new List<Tuple<int, double>>();              /* O(1) */
            double X2, Y2, Length;                                                       /* O(1) */
            // number of vertices * O(body) = O(number of vertices) * O(log V) = O(V log (n))
            for (int i = 0; i < number_of_intersections - 1; i++)
            {
                X2 = Intersections[i].X_coordinate;                                    /* O(1) */
                Y2 = Intersections[i].Y_coordinate;                                   /* O(1) */
                Length = Math.Sqrt(((X - X2) * (X - X2)) + ((Y - Y2) * (Y - Y2)));   /* O(log n) */
                if (Length <= R)                                                    /* O(1) */
                {
                    nodes.Add(new Tuple<int, double>(i, Length));                  /* O(1) */
                }
            }

            return nodes;                                                        /* O(1) */
        }
    }
}

