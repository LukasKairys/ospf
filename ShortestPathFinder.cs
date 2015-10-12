using System.Collections.Generic;
using ospf.Models;

namespace ospf
{
    static class ShortestPathFinder
    {
        public static void CountAll(List<List<Node>> nodesToVisit, List<Node> nodes, List<PathNode> pathNodes)
        {
             nodesToVisit.ForEach(nodesToVisitList => 
                                    nodesToVisitList.ForEach(node => 
                                        CountAllPaths(node.NodeId, nodes, pathNodes)));
        }

        public static void CountAllPaths(int sourceNodeId, List<Node> nodes, List<PathNode> pathNodes)
        {
            var sourceNode = FindNodeById(sourceNodeId, nodes);
            var sourcePathNode = FindPathNodeById(sourceNodeId, pathNodes);

            foreach (var neighborNodeObj in sourceNode.NeighborNodes)
            {
                var pathNode = FindPathNodeById(neighborNodeObj.Id, pathNodes);

                var totalPriceToNeighbor = neighborNodeObj.PriceToNeighbor + sourcePathNode.ShortestPathPrice;
                if (pathNode.ShortestPathPrice > totalPriceToNeighbor)
                {
                    pathNode.ShortestPathPrice = totalPriceToNeighbor;
                    pathNode.ShortestPathNeighborId = sourceNode.NodeId;
                }
                    
            }
        }

        public static void GetReversedShortestPath(int sourceNodeId, int targetNodeId, List<PathNode> shortestPathNodes, List<PathNode> pathNodes)
        {
            var node = ShortestPathFinder.FindPathNodeById(targetNodeId, pathNodes);
            shortestPathNodes.Add(node);
            if (node.ShortestPathNeighborId != 0 && node.NodeId != sourceNodeId)
            {
                GetReversedShortestPath(sourceNodeId, node.ShortestPathNeighborId, shortestPathNodes, pathNodes);
            }
        }

        public static PathNode FindPathNodeById(int id, List<PathNode> nodes)
        {
            return nodes.Find(node => node.NodeId == id);
        }

        public static Node FindNodeById(int id, List<Node> nodes)
        {
            return nodes.Find(node => node.NodeId == id);
        }
    }
}
