using System.Collections.Generic;

namespace ospf.Models
{
    class Node
    {
        public delegate void MyEventHandler(int updateId);
        public event MyEventHandler NodesRelationsChanged;

        public List<Node> LatestNodesHierarchy { get; set; }

        public int NodeId { get; set; }
        public int LastUpdateId { get; set; }
        public List<NeighborNode> NeighborNodes { get; set; }

        public Node(int nodeId)
        {
            NodeId = nodeId;
            NeighborNodes = new List<NeighborNode>();

            NodesRelationsChanged += InformNeighbors;
        }

        public void AddNeighborNode(NeighborNode neighborNode)
        {
            NeighborNodes.Add(neighborNode);

        }

        public void FinishedAddingConnections(List<Node> nodes, int updateId)
        {
            LatestNodesHierarchy = nodes;
            NodesRelationsChanged(updateId);
        }

        public void InformNeighbors(int updateId)
        {
            if (updateId > LastUpdateId)
            {
                LastUpdateId = updateId;
                NeighborNodes.ForEach(nN =>
                {
                    var node = ShortestPathFinder.FindNodeById(nN.Id, LatestNodesHierarchy);
                    node.LatestNodesHierarchy = LatestNodesHierarchy;
                    node.NodesRelationsChanged(updateId);
                });
            }
        }

        public override string ToString()
        {
            return "NodeId: " + NodeId;
        }
    }
}
