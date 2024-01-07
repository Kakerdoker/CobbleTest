using UnityEngine;

public class Input_Manager : MonoBehaviour
{
    private static int floor = 1 << 8;

    public Camera camera;
    public Player_Manager playerManager;

    /// <summary>
    /// Returns true if a raycast from the camera to the cursor hit a floor. Stores the position in the given parameter.
    /// </summary>
    private bool GetFloorHitPositionFromCamera(out Vector3 position)
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 500f))
        {
            position = hit.point;
            return true;
        }
        position = new Vector3(0, 0, 0);
        return false;
    }

    /// <summary>
    /// Moves players to a position if cursor clicked on a floor.
    /// </summary>
    private void MovePlayersOnFloorHit()
    {
        Vector3 floorClick;
        if (GetFloorHitPositionFromCamera(out floorClick))
            playerManager.MovePlayers(floorClick);
    }
    
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
            MovePlayersOnFloorHit();

        if (Input.GetButtonDown("space"))
            playerManager.AddPlayer();
    }
}
