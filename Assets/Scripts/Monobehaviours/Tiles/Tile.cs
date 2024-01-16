using UnityEngine;
using Tile_Coordinates;
using Tile_Astar;

public class Tile : MonoBehaviour
{
    /// <summary><c>Tile</c>'s coordinates on the <c>Tile_Manager</c>'s grid.</summary>
    public Axial coordinates { get; private set; }
    /// <summary><c>Tile</c>'s travel node used for pathfinding.</summary>
    public TileTravelNode travelNode { get; private set; }
    /// <summary>Can a <c>Player</c> stand on this tile?</summary>
    public bool walkable;

    /// <summary>
    /// Initializes the tile (Constructor).
    /// </summary>
    public void Init(Axial coords)
    {
        coordinates = coords;
        travelNode = new TileTravelNode(this);
    }


}
