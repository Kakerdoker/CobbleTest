using Tile_Coordinates;
using System.Collections.Generic;

namespace Tile_Astar
{
    /// <summary>
    /// Stores all of the necessary information for performing an AStar search on.
    /// </summary>
    public class TileTravelNode
    {
        public float gCost, hCost, fCost;
        public Axial coordinates;
        public TileTravelNode cameFrom;
        public Tile parent;

        public TileTravelNode(Tile parent)
        {
            gCost = hCost = fCost = 0f;
            cameFrom = null;
            this.parent = parent;
            coordinates = parent.coordinates;
        }
    }


    public static class Astar
    {
        /// <summary>List containing all of the tiles</summary>
        static List<List<Tile>> fullList;
        /// <summary>All of the queued TileTravelNodes to be searched for the quickest path.</summary>
        static List<TileTravelNode> queued;
        /// <summary>All of the already processed nodes.</summary>
        static List<TileTravelNode> visited;

        /// <summary>
        /// Get neighbour of the given node. The neighbour is moved by q and r. Returns null if the neighbour doesn't exist.
        /// </summary>
        private static TileTravelNode GetNeighbour(TileTravelNode node, int q, int r)
        {
            q += node.coordinates.q;
            r += node.coordinates.r;

            if (q < fullList.Count && q >= 0)
                if (r < fullList[q].Count && r >= 0)
                {
                    return fullList[q][r].travelNode;
                }
            return null;
        }

        /// <summary>
        /// Returns all of the neighbours nodes.
        /// </summary>
        private static List<TileTravelNode> FetchNeighbours(TileTravelNode node)
        {
            List<TileTravelNode> neighbours = new List<TileTravelNode>();

            neighbours.Add(GetNeighbour(node, 1, 0));
            neighbours.Add(GetNeighbour(node, -1, 0));
            neighbours.Add(GetNeighbour(node, 1, -1));
            neighbours.Add(GetNeighbour(node, -1, 1));
            neighbours.Add(GetNeighbour(node, 0, -1));
            neighbours.Add(GetNeighbour(node, 0, 1));

            neighbours.RemoveAll(item => item == null);
            return neighbours;
        }

        /// <summary>
        /// Goes through all of the nodes neighbours, sets/updates their costs, and adds them to the queue.
        /// </summary>
        private static void UpdateNodesNeighbours(TileTravelNode node, TileTravelNode destination)
        {
            //Loop through the neighbours.
            foreach(TileTravelNode neighbour in FetchNeighbours(node))
            {
                //Ignore if not walkable or already visited.
                if (!neighbour.parent.walkable || visited.Contains(neighbour))
                    continue;

                float newG = node.gCost + 1f;

                //If the looped through neighbour isn't already queued, then add him to the queue and set his costs and the node he came from.
                if (!queued.Contains(neighbour))
                {
                    queued.Insert(0, neighbour);

                    neighbour.cameFrom = node;
                    neighbour.gCost = newG;
                    neighbour.hCost = Axial.GetDistance(destination.coordinates, neighbour.coordinates);
                    neighbour.fCost = neighbour.hCost + neighbour.gCost;
                }
                else if (neighbour.gCost > newG)//If the neighbour is already queued but there turns out to be a better path from the current TileTravelNode then update it.
                {
                    neighbour.gCost = newG;
                    neighbour.cameFrom = node;
                }  
            }
        }

        /// <summary>
        /// Goes through all of the queued <c>TileTravelNodes</c> and returns the one with the lowest <c>fCost</c>.
        /// </summary>
        private static TileTravelNode PickBestFromQueued()
        {
            if (queued == null || queued.Count == 0)
                return null;//End of the road, there isn't a path available.

            TileTravelNode best = queued[0];
            foreach (TileTravelNode node in queued)
            {
                if (best.fCost > node.fCost)
                    best = node;
            }

            queued.Remove(best);
            visited.Add(best);
            return best;
        }

        /// <summary>
        /// Goes back from the <c>destination</c> to the <c>origin</c> by following <c>comeFrom</c> nodes inside <c>TileTravelNodes</c>.<br/>
        /// Then reverses the path so the first index will be the first tile the <c>Player</c> needs to go to.
        /// </summary>
        private static List<Tile> RetracePath(TileTravelNode destination)
        {
            List<Tile> path = new List<Tile>();
            while (destination.cameFrom != null)
            {
                path.Add(destination.parent);
                destination = destination.cameFrom;
            }
            path.Reverse();
            return path;
        }


        /// <summary>
        /// Performs an AStar search on Tiles inside <c>tileList</c>.<br/>
        /// Tries to find a path from <c>originTile</c> to <c>destinationTile</c>.
        /// </summary>
        public static List<Tile> Search(List<List<Tile>> tileList, Tile originTile, Tile destinationTile)
        {
            if (destinationTile == null || originTile == null)
                return null;

            TileTravelNode current = originTile.travelNode;
            TileTravelNode destination = destinationTile.travelNode;
            if (current == destination)
                return null;

            queued = new List<TileTravelNode>();
            visited = new List<TileTravelNode>() { current };
            fullList = tileList;

            //Clean the first travel node, so previous gCosts and cameFrom nodes won't affect the current run of the algorithm.
            current = new TileTravelNode(current.parent);
            
            while(current != destination && current != null)
            {
                UpdateNodesNeighbours(current, destination);
                current = PickBestFromQueued();
            }

            //Returns null if there were no queued TileTravelNodes.
            if (current == null)
                return null;

            return RetracePath(current);
        }
    }

}