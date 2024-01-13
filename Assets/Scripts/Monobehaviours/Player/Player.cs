using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [HideInInspector] public bool isFollowing;
    [HideInInspector] public Vector3 destinationVector;
    [HideInInspector] public Player followingPlayer;

    int speed, agility, resistance;

    UI_Statsbox statsbox;
    NavMeshAgent agent;

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
    /// Changes Unity AI's destination to <c>Player's</c> destination vector.
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

    /// <summary>
    /// Sets the given values inside the <c>Player</c> (basically a constructor).
    /// </summary>
    public void Init(UI_Statsbox statsbox, string name)
    {
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

    void Start()
    {
        speed = Random.Range(1, 100);
        agility = Random.Range(1, 100);
        resistance = Random.Range(1, 100);

        ChangePlayerColor();

        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        SetDestination();
        Move();
    }
}
