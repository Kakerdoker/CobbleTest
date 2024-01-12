using UnityEngine;

public class UI_Canvas : MonoBehaviour
{
    [Header("Manager Script References")]
    [SerializeField] UI_Manager uiManager;

    //Updates button positions and statsbox sizes upon Canvas changing size.
    private void OnRectTransformDimensionsChange()
    {
        uiManager.PositionButtons();
        uiManager.ResizeStatsboxes();
    }
}
