using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ospf.Models
{
    class NeighborNode
    {
        public int Id { get; set; }
        public int PriceToNeighbor { get; set; }

        public NeighborNode(int id, int priceToNeighbor)
        {
            Id = id;
            PriceToNeighbor = priceToNeighbor;
        }
    }
}
