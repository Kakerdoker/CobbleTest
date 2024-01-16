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

    /// <summary><c>Player</c> hovered over in last frame.</summary>
    Player lastFramePlayerScript = null;

    //Layers
    private const int floor = 1 << 8;
    private const int player = 1 << 9;


    /// <summary>
    /// Shows <c>Statsbox</c> of the currently hovered over <c>Player</c>.
    /// </summary>
    private void ShowStatsOnPlayerHover()
    {
        GameObject playerObject = GetHitPlayerObject();
        Player playerScript = playerObject?.GetComponent<Player>();

        //If there is a change in what the mouse is hovering over, then show the new and hide the old.
        if (playerScript != lastFramePlayerScript)
        {
            playerScript?.ShowStatsbox();
            lastFramePlayerScript?.HideStatsbox();
            lastFramePlayerScript = playerScript;
        }

        playerScript?.UpdateStatsboxPosition(Input.mousePosition);
    }

    /// <summary>
    /// Returns a <c>Player</c> <c>GameObject</c> if a raycast from the camera to the cursor hit any.
    /// </summary>
    private GameObject GetHitPlayerObject()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 500f, player))
        {
            return hit.transform.gameObject;
        }
        return null;
    }

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
    /// Returns true if a raycast from the camera to the cursor hit a tile. Passes the hit <c>GameObject</c>'s <c>Tile</c> script in the given argument.
    /// </summary>
    private bool GetTileHitFromCamera(out Tile tile)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 500f, floor))
        {
            tile = hit.transform.gameObject?.GetComponent<Tile>();
            if (tile != null)
                return true;
        }
        tile = null;
        return false;
    }

    /// <summary>
    /// Moves players to a position if cursor clicked directly on a floor.
    /// </summary>
    private void MovePlayersOnFloorHit()
    {
        if (GetTileHitFromCamera(out Tile tile) && !GetUIHit())
            playerManager.MovePlayers(tile);
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            MovePlayersOnFloorHit();

        if (Input.GetKeyDown("space"))
            playerManager.AddNewPlayer();

        ShowStatsOnPlayerHover();
    }


}
