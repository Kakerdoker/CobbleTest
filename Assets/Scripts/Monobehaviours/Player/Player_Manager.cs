using System.Collections.Generic;
using UnityEngine;
public class Player_Manager : MonoBehaviour
{
    [Header("Manager Script References")]
    [SerializeField] UI_Manager uiManager;
    [SerializeField] Tile_Manager tileManager;

    [Header("Prefabs")]
    [SerializeField] GameObject playerPrefab;

    /// <summary>Stores <c>Player scripts</c> in their moving order, with index 0 being first in line.</summary>
    List<Player> playerOrder;
    /// <summary>Stores <c>Player scripts</c> in an unchanging (unless deleted) order.</summary>
    List<Player> playerList;


    /// <summary>
    /// Makes sure the players inside <c>playerOrder</c> are in correct order and invokes methods used to update them if necessary.
    /// </summary>
    /// <param name="destination">Where the leader will go.</param>
    public void MovePlayers(Tile destination)
    {
        if (playerOrder.Count == 0 || destination == null)
            return;
        //Change player order only if there is a new leader.
        if (playerOrder[0] != uiManager.selectedPlayer)
        {
            ChangePlayerOrder(uiManager.selectedPlayer);
        }
        UpdatePaths(destination);
    }

    /// <summary>
    /// Creates, initializes, and adds a new Player <c>GameObject</c> to the world.
    /// </summary>
    public void AddNewPlayer()
    {
        GameObject playerInstance = HandleNewPlayerInstance();
        InitializePlayer(playerInstance);
    }

    /// <summary>
    /// Updates all the <c>Players</c> paths to their desired location. Destination parameter is the path of the leader.
    /// </summary>
    private void UpdatePaths(Tile destination)
    {
        if (UpdateLeaderPath(destination))
            UpdateFollowerPaths();
    }

    /// <summary>
    /// Updates paths of the <c>Players</c> to follow the path of the <c>Player</c> in front of them in <c>playerOrder</c>.
    /// </summary>
    private void UpdateFollowerPaths()
    {
        for (int i = 1; i < playerOrder.Count; i++)
        {
            Player following = playerOrder[i];
            Player followed = playerOrder[i - 1];

            //If the followed player moved farther than two tiles.
            if (followed.tilePath.Count > 1)
            {
                //Find path from the following player's position to the second to last tile of the followed players path.
                Tile behindFollowedPlayer = followed.tilePath[^2];
                following.SetPath(tileManager.GetPath(following.occupiedTile, behindFollowedPlayer));
            }
            else
            {
                //Find a path to any neighbour of the followed player.
                Tile walkToTile = tileManager.GetFirstViableNeighbour(followed.occupiedTile);
                following.SetPath(tileManager.GetPath(following.occupiedTile, walkToTile));
            }
        }
    }

    /// <summary>
    /// Updates the leader's path to the given destination.<br/>
    /// Returns true if path exists or given destination is the same tile the leader is currently on.
    /// </summary>
    /// The same-tile-as-the-leader-thing is so the the Players can be untied when leader is stuck around them.
    private bool UpdateLeaderPath(Tile destination)
    {
        Player leader = playerOrder[0];
        leader.SetPath(tileManager.GetPath(leader.occupiedTile, destination));

        if (leader.tilePath.Count == 0 && destination != leader.occupiedTile)
            return false;
        return true;
    }

    /// <summary>
    /// Initializes <c>Player's</c> internal variables as well as handles the given <c>Player's</c> references in <c>Player_Manager</c> and other scripts.
    /// </summary>
    private void InitializePlayer(GameObject playerObject)
    {
        Player playerScript = HandlePlayerLists(playerObject);

        playerScript.Init(uiManager.AddStatsbox(), MakePlayerName(), tileManager.SnapToGrid(playerObject.transform.position));
        uiManager.AddButton(playerScript);

        playerScript.occupiedTile = tileManager.GetTileFromWorldspace(playerObject.transform.position);
    }

    /// <summary>
    /// Creates a new <c>Player</c> instance and initializes it.
    /// </summary>
    private GameObject HandleNewPlayerInstance()
    {
        GameObject playerInstance = Instantiate(playerPrefab);
        playerInstance.transform.parent = gameObject.transform;

        return playerInstance;
    }

    /// <summary>
    /// Returns a new player name.<br/>
    /// </summary>
    /// Made as a seperate method to make sure every part of the code uses the same naming convention.
    private string MakePlayerName()
    {
        return "Player " + playerOrder.Count;
    }

    /// <summary>
    /// Adds a new playerScript to lists containing them.<br/>
    /// Returns <c>Player</c> script of the <c>playerInstance</c> <c>gameObject</c>.
    /// </summary>
    private Player HandlePlayerLists(GameObject playerInstance)
    {
        Player playerScript = playerInstance.GetComponent<Player>();
        playerOrder.Add(playerScript);
        playerList.Add(playerScript);

        //If the current player is the first one then make them the leader.
        if (playerList.Count == 1)
            uiManager.ChangeSelectedPlayer(playerScript);

        return playerScript;
    }

    /// <summary>
    /// This method changes the playerOrder in such a way, that the leader gets moved to index 0 while preserving an intuitive order for the rest of the players.<br/>
    /// If in <c>0 1 2 3 4</c> you make <c>2</c> the new leader then the order will become <c>2 3 4 1 0</c>
    /// </summary>
    private void ChangePlayerOrder(Player leader)
    {

        int leaderIndex = playerOrder.IndexOf(leader);
        List<Player> newPlayerOrder = new List<Player>{ leader };


        for(int i = leaderIndex + 1; i < playerOrder.Count; i++)
        {
            newPlayerOrder.Add(playerOrder[i]);

        }
        for (int i = leaderIndex - 1; i >= 0; i--)
        {
            newPlayerOrder.Add(playerOrder[i]);
        }
        playerOrder = newPlayerOrder;
    }

    /// <summary>
    /// Adds <c>Players</c> added as part of the scene to the <c>Player_Manager</c>.<br/>
    /// The <c>Players</c> have to be children of the Object holding this script.
    /// </summary>
    private void CheckForAlreadyExistingPlayers()
    {
        int childAmount = gameObject.transform.childCount;
        for(int i = 0; i < childAmount; i++)
        {
            GameObject possiblePlayerObject = gameObject.transform.GetChild(i).gameObject;
            if (possiblePlayerObject.GetComponent<Player>() != null)
                InitializePlayer(possiblePlayerObject);
        }
    }

    void Start()
    {
        playerOrder = new List<Player>();
        playerList = new List<Player>();
        CheckForAlreadyExistingPlayers();
    }

}
