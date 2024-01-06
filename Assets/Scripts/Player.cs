using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed, agility, resistance;
    [HideInInspector] public bool isFollowing;
    [HideInInspector] public Player followingPlayer;
    [HideInInspector] public Vector3 destinationVector;

    /// <summary>
    /// Temporary method<br/>
    /// Moves the player's <c>gameObject</c> to his <c>destinationVector</c>.
    /// </summary>
    private void Move()
    {
        gameObject.transform.position = destinationVector;
    }

    /// <summary>
    /// If the player <c>isFollowing</c> another player, then set his <c>destinationVector</c> to the <c>followingPlayer</c>'s position.
    /// </summary>
    private void SetDestination()
    {
        if (isFollowing)
            destinationVector = followingPlayer.transform.position-new Vector3(-1f,0,-1f);//Substract some value for now so the players don't end up exactly on eachother.
    }

    void Start()
    {
        speed = Random.Range(1, 100);
        agility = Random.Range(1, 100);
        resistance = Random.Range(1, 100);
    }

    void Update()
    {
        SetDestination();
        Move();
    }
}
