using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Input_Manager : MonoBehaviour
{
    [Header("Manager Script References")]
    [SerializeField] Player_Manager playerManager;
    [SerializeField] UI_Manager uiManager;

    [Header("Miscellaneous References ")]
    [SerializeField] Camera camera;
    [SerializeField] GraphicRaycaster raycaster;
    [SerializeField] EventSystem eventSystem;

    //Layers
    private const int floor = 1 << 8;

    /// <summary>
    /// Returns true if mouse clicked on a UI component.
    /// </summary>
    private bool GetUIHit()
    {
        PointerEventData pointerEventData;

        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
            return true;
        return false;
    }

    /// <summary>
    /// Returns true if a raycast from the camera to the cursor hit a floor. Stores the position in the given parameter.
    /// </summary>
    private bool GetFloorHitPositionFromCamera(out Vector3 position)
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 500f, floor))
        {
  

            position = hit.point;
            return true;
        }
        position = new Vector3(0, 0, 0);
        return false;
    }

    /// <summary>
    /// Moves players to a position if cursor clicked directly on a floor.
    /// </summary>
    private void MovePlayersOnFloorHit()
    {
        Vector3 floorClick;
        if (GetFloorHitPositionFromCamera(out floorClick) && !GetUIHit())
            playerManager.MovePlayers(floorClick);
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            MovePlayersOnFloorHit();

        if (Input.GetKeyDown("space"))
            playerManager.AddNewPlayer();
    }


}
