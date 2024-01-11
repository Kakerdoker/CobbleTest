using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public int speed, agility, resistance;
    [HideInInspector] public bool isFollowing;
    [HideInInspector] public Player followingPlayer;
    [HideInInspector] public Vector3 destinationVector;
    NavMeshAgent agent;

    /// <summary>
    /// Temporary method<br/>
    /// Moves the player's <c>gameObject</c> to his <c>destinationVector</c>.
    /// </summary>
    private void Move()
    {
        agent.destination = destinationVector;
    }

    /// <summary>
    /// Sets the variables to follow their <c>followingPlayer</c>.
    /// </summary>
    private void SetDestination()
    {
        if (isFollowing && followingPlayer != null)
        {
            destinationVector = followingPlayer.transform.position;
            agent.stoppingDistance = followingPlayer.transform.localScale.x+0.5f;//Make the stopping distance the size of their followee + some margin.
        }
    }

    void Start()
    {
        speed = Random.Range(1, 100);
        agility = Random.Range(1, 100);
        resistance = Random.Range(1, 100);

        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        SetDestination();
        Move();
    }
}
