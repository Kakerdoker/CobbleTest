using System.Collections.Generic;
using UnityEngine;


public class Player_Manager : MonoBehaviour
{
    [Header("Manager Script References")]
    [SerializeField] UI_Manager uiManager;

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
    public void MovePlayers(Vector3 destination)
    {
        if (playerOrder.Count == 0)
            return;
        //Change and update player order only if there is a new leader.
        if (playerOrder[0] != uiManager.selectedPlayer)
        {
            ChangePlayerOrder(uiManager.selectedPlayer);
            UpdatePlayersFollowStatus();
        }
        //Always change leaders destination.
        ChangeLeaderDestination(destination);
    }

    /// <summary>
    /// Creates and adds a new player <c>GameObject</c> to the world and the <c>Player_Manager</c>.
    /// </summary>
    public void AddNewPlayer()
    {
        GameObject playerInstance = HandleNewPlayerInstance();
        Player playerScript = HandlePlayerLists(playerInstance);
        playerScript.statsbox = uiManager.AddStatsbox();
        UpdatePlayersFollowStatus();
        uiManager.AddButton(playerScript);
    }

    /// <summary>
    /// Adds an existing player <c>GameObject</c> to the <c>Player_Manager</c>.
    /// </summary>
    private void AddExistingPlayer(GameObject playerObject)
    {
        ChangePlayerColor(playerObject);
        playerObject.name = MakePlayerName();

        Player playerScript = HandlePlayerLists(playerObject);
        playerScript.statsbox = uiManager.AddStatsbox();
        UpdatePlayersFollowStatus();
        uiManager.AddButton(playerScript);
    }

    /// <summary>
    /// Creates a new player instance and initializes its variables.
    /// </summary>
    private GameObject HandleNewPlayerInstance()
    {
        GameObject playerInstance = Instantiate(playerPrefab);
        ChangePlayerColor(playerInstance);

        playerInstance.name = MakePlayerName();
        playerInstance.transform.parent = gameObject.transform;

        return playerInstance;
    }

    /// <summary>
    /// Changes the given <c>GameObjects's</c> <c>Renderer's color</c> to a random value.
    /// </summary>
    private void ChangePlayerColor(GameObject playerObject)
    {
        Renderer renderer = playerObject.GetComponent<Renderer>();
        Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        renderer.material.color = newColor;
    }

    /// <summary>
    /// Returns a new player name.<br/>
    /// Made as a seperate method to make sure every part of the code uses the same naming convention.
    /// </summary>
    /// <returns></returns>
    private string MakePlayerName()
    {
        return "Player " + (playerOrder.Count + 1);
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

        //If the current player is the first one then make them the leader and change their destination to their location.
        if (playerList.Count == 1)
        {
            uiManager.ChangeSelectedPlayer(playerScript);
            playerScript.destinationVector = playerScript.transform.position;
        }
        return playerScript;
    }


    /// <summary>
    /// This method changes the playerOrder in such a way, that the leader gets moved to index 0 while preserving an intuitive order for the rest of the players.<br/>
    /// If in <c>0 1 2 3 4</c> you make <c>2</c> the new leader then the order will become <c>2 3 4 0 1</c>
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
    /// Sets the leaders <c>destinationVector</c> to the one provided as a parameter and sets his <c>isFollowing</c> flag to false.
    /// </summary>
    private void ChangeLeaderDestination(Vector3 destination)
    {
        playerOrder[0].destinationVector = destination;
        playerOrder[0].isFollowing = false;
    }

    /// <summary>
    /// Sets the <c>isFollowing</c> flag to true and changes <c>followingPlayer</c> to whoever is in front of them in the <c>playerQueue</c>.<br/>
    /// Does this to everyone in <c>playerQueue</c> except the leader who is at index 0.
    /// </summary>
    public void UpdatePlayersFollowStatus()
    {
        for(int i = 1; i < playerOrder.Count; i++)
        {
            playerOrder[i].isFollowing = true;
            playerOrder[i].followingPlayer = playerOrder[i - 1];
        }
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
                AddExistingPlayer(possiblePlayerObject);
        }
    }


    void Start()
    {
        playerOrder = new List<Player>();
        playerList = new List<Player>();
        CheckForAlreadyExistingPlayers();
    }

}
