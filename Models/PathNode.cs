namespace ospf.Models
{
    class PathNode
    {
        public int NodeId { get; set; }
        public int ShortestPathPrice { get; set; }
        public int ShortestPathNeighborId { get; set; }

        public PathNode(int nodeId, int shortestPathPrice)
        {
            NodeId = nodeId;
            ShortestPathPrice = shortestPathPrice;
        }
        public override string ToString()
        {
            return "NodeId: " + NodeId +
                   ", ShortestPathCount: " + ShortestPathPrice +
                   "ShortestPathNeighborId: " + ShortestPathNeighborId;
        }
    }
}
