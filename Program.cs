using System;
using System.Collections.Generic;
using ospf.Models;

namespace ospf
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new NodesManager();

            GetPath(manager);

            AddNode(manager);

            GetPath(manager);

            DeleteNode(manager);

            GetPath(manager);
        }

        static void AddNode(NodesManager manager)
        {
            var neibghorNodes = new List<NeighborNode>();

            Console.WriteLine("Enter nodes first neighbor id:");
            int firstNeighborId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter nodes first neighbor price:");
            int firstNeighborPrice = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter nodes second neighbor id:");
            int secondNeighborId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter nodes second neighbor price:");
            int secondNeighborPrice = Convert.ToInt32(Console.ReadLine());

            neibghorNodes.Add(new NeighborNode(firstNeighborId, firstNeighborPrice));
            neibghorNodes.Add(new NeighborNode(secondNeighborId, secondNeighborPrice));

            manager.AddNode(neibghorNodes);

            Console.WriteLine("Node added!");
        }

        static void DeleteNode(NodesManager manager)
        {

            Console.WriteLine("Enter node to delete id:");
            int nodeToDeleteId = Convert.ToInt32(Console.ReadLine());

            manager.DeleteNode(nodeToDeleteId);

            Console.WriteLine("Node deleted!");
        }

        static void GetPath(NodesManager manager)
        {
            Console.WriteLine("Enter source id:");
            int sourceNodeId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter target id:");
            int targetNodeId = Convert.ToInt32(Console.ReadLine());

            var nodesPath = manager.GetShortestPath(sourceNodeId, targetNodeId);

            nodesPath.ForEach(node => Console.WriteLine("Node id: " + node.NodeId + ", path to node price: " + node.ShortestPathPrice));

            Console.ReadLine();
        }
    }
}
