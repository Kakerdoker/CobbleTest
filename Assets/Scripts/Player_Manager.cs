using System.Collections.Generic;
using UnityEngine;


public class Player_Manager : MonoBehaviour
{
    /// <summary>Represents the currently selected <c>player</c> as the next leader.
    /// <br/>Will probably be moved to Canvas Manager once I make it.</summary>
    [HideInInspector] public Player selectedPlayer;
    /// <summary>Stores <c>Player scripts</c> in their moving order, with index 0 being first in line.</summary>
    [HideInInspector] public List<Player> playerOrder { get; private set; }
    /// <summary>Stores <c>Player scripts</c> in an unchanging (unless deleted) order.</summary>
    private List<Player> playerList;


    /// <summary>
    /// Makes sure the players inside <c>playerOrder</c> are in correct order and invokes methods used to update them if necessary.
    /// </summary>
    /// <param name="destination">Where the leader will go.</param>
    public void MovePlayers(Vector3 destination)
    {
        if (playerOrder.Count == 0)
            return;
        //Change and update player order only if there is a new leader.
        if (playerOrder[0] != selectedPlayer)
        {
            
            ChangePlayerOrder(selectedPlayer);
            UpdatePlayersFollowStatus();
        }
        //Always change leaders destination.
        ChangeLeaderDestination(destination);
    }

    public GameObject playerObject;
    /// <summary>
    /// Adds a new player GameObject to the world.
    /// </summary>
    public void AddPlayer()
    {
        GameObject playerInstance = HandleNewPlayerInstance();
        HandlePlayerLists(playerInstance);
        UpdatePlayersFollowStatus();
    }

    /// <summary>
    /// Creates a new player instance and initializes its variables.
    /// </summary>
    private GameObject HandleNewPlayerInstance()
    {
        GameObject playerInstance = Instantiate(playerObject);
        playerInstance.name = "player " + playerOrder.Count;
        playerInstance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * playerOrder.Count;
        playerInstance.transform.parent = gameObject.transform.parent;

        return playerInstance;
    }

    /// <summary>
    /// Adds a new playerScript to lists containing them.
    /// </summary>
    private void HandlePlayerLists(GameObject playerInstance)
    {
        Player playerScript = playerInstance.GetComponent<Player>();
        playerOrder.Add(playerScript);
        playerList.Add(playerScript);
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
    private void UpdatePlayersFollowStatus()
    {
        for(int i = 1; i < playerOrder.Count; i++)
        {
            playerOrder[i].isFollowing = true;
            playerOrder[i].followingPlayer = playerOrder[i - 1];
        }
    }

    void Start()
    {
        playerOrder = new List<Player>();
        playerList = new List<Player>();
    }

}
