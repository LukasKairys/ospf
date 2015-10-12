using System;
using System.Collections.Generic;
using System.Linq;
using ospf.Models;

namespace ospf
{
    class NodeState
    {
        public List<Node> Nodes { get; set; } 
        public List<PathNode> PathNodes { get; set; } 

        public int LastUpdateId { get; set; }
        public int NodeId { get; set; }

        public NodeState(List<Node> nodes, int nodeId)
        {
            Nodes = nodes;
            NodeId = nodeId;

            var thisNode = ShortestPathFinder.FindNodeById(NodeId, nodes);
            thisNode.NodesRelationsChanged += NodeRelationsChangedHandler;

            ResetPathNodes();
            CalculateAllPaths();
        }


        void NodeRelationsChangedHandler(int updateId)
        {
            if (updateId > LastUpdateId)
            {
                ResetPathNodes();
                CalculateAllPaths();
                LastUpdateId = updateId;
                Console.WriteLine("Node with id: " + NodeId + " has been updated");
            }     
               
        }

        public void ResetPathNodes()
        {
            var pathNodes = new List<PathNode>();

            Nodes.ForEach(node => pathNodes.Add(new PathNode(node.NodeId, NodeId == node.NodeId ? 0 : 9999999)));

            PathNodes = pathNodes;
        } 

        public void CalculateAllPaths()
        {
            var sourceNode = ShortestPathFinder.FindNodeById(NodeId, Nodes);
            var sourceNodeNeighbors = new List<Node>();

            var nodesToVisit = new List<List<Node>> { new List<Node> {sourceNode} };

            sourceNode.NeighborNodes.ForEach(node =>
            {
                sourceNodeNeighbors.Add(ShortestPathFinder.FindNodeById(node.Id, Nodes));
            });

            nodesToVisit.Add(sourceNodeNeighbors);

            FormNodesToVisitList(nodesToVisit);

            ShortestPathFinder.CountAll(nodesToVisit, Nodes, PathNodes);
        }

        private void FormNodesToVisitList(List<List<Node>> nodesToVisit)
        {
            var neighboringNodes = new List<Node>();

            nodesToVisit.Last().ForEach(nodeToVisit => nodeToVisit.NeighborNodes.ForEach(neighborNode =>
            {
                if (IsNodeAlreadyInVisitedList(nodesToVisit, neighborNode.Id))
                {
                    var node = ShortestPathFinder.FindNodeById(neighborNode.Id, Nodes);
                    neighboringNodes.Add(node);
                }
            }));
              
            // Exclude nodes, which are already in nodesToVisitList
            neighboringNodes = neighboringNodes.Where(node => !nodesToVisit.Last().Exists(sourcenode => node.NodeId == sourcenode.NodeId)).Distinct().ToList();

            if (neighboringNodes.Count > 0)
            {
                nodesToVisit.Add(neighboringNodes);
                FormNodesToVisitList(nodesToVisit);
            }
        }

        private bool IsNodeAlreadyInVisitedList(List<List<Node>> nodesToVisit, int neighborNodeId)
        {
            return !nodesToVisit[nodesToVisit.Count - 2].Exists(node => node.NodeId == neighborNodeId);
        }


        public List<PathNode> GetShortestPath(int targetNodeId)
        {
            var nodesPath = new List<PathNode>();

            ShortestPathFinder.GetReversedShortestPath(NodeId, targetNodeId, nodesPath, PathNodes);
            nodesPath.Reverse();

            return nodesPath;
        }


        

    }
}
