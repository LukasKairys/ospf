using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ospf.Models;

namespace ospf
{
    class NodesManager
    {
        public List<Node> Nodes { get; set; } 
        public List<NodeState> NodeStates { get; set; } 

        public int UpdateId { get; set; }

        public NodesManager()
        {
            UpdateId = 0;

            var node1 = new Node(1);
            var node2 = new Node(2);
            var node3 = new Node(3);
            var node4 = new Node(4);
            var node5 = new Node(5);
            var node6 = new Node(6);

            node1.NeighborNodes.Add(new NeighborNode(2, 7));
            node1.NeighborNodes.Add(new NeighborNode(3, 9));
            node1.NeighborNodes.Add(new NeighborNode(6, 14));

            node2.NeighborNodes.Add(new NeighborNode(4, 15));
            node2.NeighborNodes.Add(new NeighborNode(3, 10));
            node2.NeighborNodes.Add(new NeighborNode(1, 7));

            node3.NeighborNodes.Add(new NeighborNode(4, 11));
            node3.NeighborNodes.Add(new NeighborNode(2, 10));
            node3.NeighborNodes.Add(new NeighborNode(1, 9));
            node3.NeighborNodes.Add(new NeighborNode(6, 2));

            node4.NeighborNodes.Add(new NeighborNode(2, 15));
            node4.NeighborNodes.Add(new NeighborNode(3, 11));
            node4.NeighborNodes.Add(new NeighborNode(5, 6));

            node5.NeighborNodes.Add(new NeighborNode(4, 6));
            node5.NeighborNodes.Add(new NeighborNode(6, 9));

            node6.NeighborNodes.Add(new NeighborNode(5, 9));
            node6.NeighborNodes.Add(new NeighborNode(3, 2));
            node6.NeighborNodes.Add(new NeighborNode(1, 14));

            Nodes = new List<Node> { node1, node2, node3, node4, node5, node6 };

            NodeStates = new List<NodeState>();

            foreach (var node in Nodes)
            {
                NodeStates.Add(new NodeState(Nodes, node.NodeId));
            }
        }

        public List<PathNode> GetShortestPath(int sourceNodeId, int targetNodeId)
        {
            var sourceNodeState = NodeStates.Find(nodeState => nodeState.NodeId == sourceNodeId);

            return sourceNodeState.GetShortestPath(targetNodeId);
        }

        public void AddNode(List<NeighborNode> neighborNodes)
        {
            var node = new Node(Nodes.Count + 1);
            node.NeighborNodes = neighborNodes;
            UpdateId++;

            Nodes.Add(node);
            NodeStates.Add(new NodeState(Nodes, node.NodeId));

            foreach (var neighborNode in node.NeighborNodes)
            {
                var neighborNodeObj = ShortestPathFinder.FindNodeById(neighborNode.Id, Nodes);
                neighborNodeObj.AddNeighborNode(new NeighborNode(node.NodeId, neighborNode.PriceToNeighbor));
            }

            node.FinishedAddingConnections(Nodes, UpdateId);
        }

        public void DeleteNode(int nodeToDeleteId)
        {
            var node = ShortestPathFinder.FindNodeById(nodeToDeleteId, Nodes);

            Nodes.Remove(node);

            UpdateId++;

            NodeStates.RemoveAll(nodeState => nodeState.NodeId == node.NodeId);

            foreach (var neighborNode in node.NeighborNodes)
            {
                var neighborNodeObj = ShortestPathFinder.FindNodeById(neighborNode.Id, Nodes);
                neighborNodeObj.NeighborNodes.RemoveAll(nN => nN.Id == node.NodeId);
            }

            foreach (var neighborNode in node.NeighborNodes)
            {
                var neighborNodeObj = ShortestPathFinder.FindNodeById(neighborNode.Id, Nodes);
                neighborNodeObj.FinishedAddingConnections(Nodes, UpdateId);
            }

            node = null;
        }
    }
}
