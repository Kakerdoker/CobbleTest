using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    /// <summary>A list containing the path of the <c>Player</c> with <c>Tile</c> 0 being the first destination.</summary>
    public List<Tile> tilePath;
    /// <summary>The <c>Tile</c> that the <c>Player</c> is currently moving towards or standing on.</summary>
    public Tile occupiedTile;

    /// <summary>Stat of the <c>Player</c></summary>
    int speed, agility, resistance;

    /// <summary><c>Statsbox</c> containing information about the current <c>Player</c>.</summary>
    UI_Statsbox statsbox;

    /// <summary>
    /// Disables the <c>Player</c>'s <c>statsbox</c>.
    /// </summary>
    public void HideStatsbox()
    {
        statsbox.Disable();
    }

    /// <summary>
    /// Enables the <c>Player</c>'s <c>statsbox</c> with updated stats.
    /// </summary>
    public void ShowStatsbox()
    {
        statsbox.UpdateBox(speed, agility, resistance, gameObject.name);
        statsbox.Enable();
    }

    /// <summary>
    /// Changes <c>Player</c>'s <c>statsbox</c> position to the given <c>mousePosition</c>.
    /// </summary>
    public void UpdateStatsboxPosition(Vector2 mousePosition)
    {
        statsbox.gameObject.transform.position = mousePosition;
    }

    /// <summary>
    /// Moves towards the first <c>Tile</c> in <c>tilePath</c>. Once it reaches it, the <c>Tile</c> gets removed from the list and <c>Player</c> can start moving towards the next one.
    /// </summary>
    private void Move()
    {
        if (tilePath == null || tilePath.Count == 0 || tilePath[0] == null)
            return;

        Vector3 destinationPosition = tilePath[0].gameObject.transform.position;
        Vector3 positionDiff = destinationPosition - gameObject.transform.position;
        positionDiff.y = 0;

        if (positionDiff.magnitude > 0.1f)
        {
            float speedModifier = 10f;
            Vector3 movementVector = positionDiff.normalized * Time.deltaTime * speedModifier;
            gameObject.transform.position += movementVector;
        }
        else
        {
            tilePath.RemoveAt(0);
        }
    } 

    /// <summary>
    /// Sets the given values inside the <c>Player</c> (basically a constructor).
    /// </summary>
    public void Init(UI_Statsbox statsbox, string name, Vector3 position)
    {
        //Push the gameObject up by 0.5 of it's scale, so half of the object won't be inside the Tile.
        Vector3 extraScale = new Vector3(0, gameObject.transform.localScale.y*0.5f, 0);

        gameObject.transform.position = position + extraScale;

        gameObject.name = name;
        this.statsbox = statsbox;
    }

    /// <summary>
    /// Changes the current <c>GameObject's</c> <c>Renderer's</c> color to a random value.
    /// </summary>
    private void ChangePlayerColor()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        renderer.material.color = newColor;
    }

    /// <summary>
    /// Sets the given path and updates walkability boolean on affected tiles.
    /// </summary>
    public void SetPath(List<Tile> path)
    {
        if(path == null)
            return;

        occupiedTile.walkable = true;

        tilePath = path;
        if (path.Count > 0)
            occupiedTile = path[^1];

        occupiedTile.walkable = false;
    }

    void Start()
    {
        speed = Random.Range(1, 100);
        agility = Random.Range(1, 100);
        resistance = Random.Range(1, 100);

        ChangePlayerColor();

        tilePath = new List<Tile>();
    }

    void Update()
    {
        Move();
    }
}
