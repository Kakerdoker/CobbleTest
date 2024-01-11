using UnityEngine;

public class UI_Canvas : MonoBehaviour
{
    [Header("Manager Script References")]
    [SerializeField] UI_Manager uiManager;

    //Updates button positions upon Canvas changing size.
    private void OnRectTransformDimensionsChange()
    {
        uiManager.PositionButtons();
    }
}
