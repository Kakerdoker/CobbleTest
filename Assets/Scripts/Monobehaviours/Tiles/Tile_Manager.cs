using System.Collections.Generic;
using UnityEngine;
using Tile_Coordinates;
using Tile_Astar;

public class Tile_Manager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject tilePrefab;

    [Header("Map properties")]
    [Tooltip("Length and width of the map.")][SerializeField] int mapSize;
    [Tooltip("Scale of the tiles.")][SerializeField] float tileScale;
    [Tooltip("Size of the gaps between tiles")][SerializeField] [Range(0f, 1f)] float gap;

    /// <summary>List of all of the scripts of the <c>Tiles</c> with their indexes corresponding to their <c>Axial</c> coordinates.</summary>
    List<List<Tile>> tileList;
    /// <summary><c>Axial</c> coordinates for the center of the grid.</summary>
    Axial gridCenter;
    /// <summary><c>Worldspace</c> coordinates where <c>Tile[0][0]</c> starts at.</summary>
    Vector3 wordlspaceCenterOffset;

    /// <summary>
    /// Returns a <c>Worldpspace</c> position snapped to the center of the nearest <c>Tile</c>.
    /// </summary>
    public Vector3 SnapToGrid(Vector3 worldspacePosition)
    {
        Axial coordinates = Axial.WorldspaceToAxial(worldspacePosition, tileScale);
        worldspacePosition = coordinates.ToWorldspace(tileScale);
        worldspacePosition.y = ElevationAboveTile();
        return worldspacePosition;
    }

    /// <summary>
    /// Returns the first non null and walkable neighbour of the given <c>Tile</c> or null if there aren't any.
    /// </summary>
    public Tile GetFirstViableNeighbour(Tile tile)
    {
        Tile neighbour;
        if (GetWalkableNeighbour(tile, out neighbour, new Axial(1, -1)))
            return neighbour;
        if (GetWalkableNeighbour(tile, out neighbour, new Axial(1, 0)))
            return neighbour;
        if (GetWalkableNeighbour(tile, out neighbour, new Axial(0, 1)))
            return neighbour;
        if (GetWalkableNeighbour(tile, out neighbour, new Axial(-1, 1)))
            return neighbour;
        if (GetWalkableNeighbour(tile, out neighbour, new Axial(-1, 0)))
            return neighbour;
        if (GetWalkableNeighbour(tile, out neighbour, new Axial(0, -1)))
            return neighbour;

        return neighbour;
    }


    /// <summary>
    /// Returns <c>Tile</c> that is located on the given position inside of <c>Worldspace</c>.
    /// </summary>
    public Tile GetTileFromWorldspace(Vector3 worldspacePosition)
    {
        Axial coordinates = Axial.WorldspaceToAxial(worldspacePosition - wordlspaceCenterOffset, tileScale);
        return GetTileFromCoordinates(coordinates);
    }

    /// <summary>
    /// Performs an AStar search on the <c>Tile_Manager</c>'s <c>tileList</c>.
    /// </summary>
    public List<Tile> GetPath(Tile from, Tile to)
    {
        return Astar.Search(tileList, from, to);
    }

    /// <summary>
    /// Returns a <c>Worldspace</c> Y coordinate positioned above the the <c>Tiles</c>.
    /// </summary>
    private float ElevationAboveTile()
    {
        return tileScale * gap;
    }

    /// <summary>
    /// Creates a new <c>Tile</c> instance and initializes it.
    /// </summary>
    private GameObject HandleNewTileInstance(Axial coordinates, float size)
    {
        GameObject tileObject = Instantiate(tilePrefab);
        tileObject.transform.localScale = new Vector3(1, 1, 1) * size;
        tileObject.name = "Tile " + coordinates.q + "," + coordinates.r;
        tileObject.transform.position = coordinates.ToWorldspace(tileScale) + wordlspaceCenterOffset;
        tileObject.transform.parent = gameObject.transform;

        return tileObject;
    }

    /// <summary>
    /// Places new <c>Tiles</c> in a square-like formation.
    /// </summary>
    private void PlaceTiles()
    {
        for (int i = 0; i < mapSize; i++)
        {
            tileList.Add(new List<Tile>());
            for (int j = 0; j < mapSize; j++)
            {
                AddNewTile(new Axial(i,j));
            }
        }
    }

    /// <summary>
    /// Adds a new <c>Tile</c> on the given coordinates.
    /// </summary>
    private void AddNewTile(Axial coordinates)
    {
        GameObject tileObject = HandleNewTileInstance(coordinates, tileScale * gap);
        Tile tileScript = tileObject.GetComponent<Tile>();
        tileScript.Init(coordinates);
        tileList[coordinates.q].Add(tileScript);
    }


    /// <summary>
    /// Returns true and passes the <c>Tile</c>'s <c>neighbour</c> in the given <c>direction</c> if it exists and is walkable, returns false and passes null otherwise.
    /// </summary>
    private bool GetWalkableNeighbour(Tile tile, out Tile neighbour, Axial direction)
    {
        Tile tempNeighbour;
        tempNeighbour = GetTileFromCoordinates(tile.coordinates + direction);
        if (tempNeighbour != null && tempNeighbour.walkable)
        {
            neighbour = tempNeighbour;
            return true;
        }
        neighbour = null;
        return false;
    }

    /// <summary>
    /// Returns the tile on the given coordinates if it exists, null otherwise.
    /// </summary>
    private Tile GetTileFromCoordinates(Axial coordinates)
    {
        int q = coordinates.q;
        int r = coordinates.r;
        if (q < tileList.Count && q >= 0)
            if (r < tileList[q].Count && r >= 0)
            {
                return tileList[q][r];
            }
        return null;
    }

    /// <summary>
    /// Initializes properties of the Tile Grid.
    /// </summary>
    private void InitializeProperties()
    {
        gap = 1f - gap;
        gridCenter = new Axial(mapSize / 2, mapSize / 2);
        wordlspaceCenterOffset = -gridCenter.ToWorldspace(tileScale);
    }

    void Start()
    {
        tileList = new List<List<Tile>>();

        InitializeProperties();
        PlaceTiles();
    }

}
