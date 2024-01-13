using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Button_Box")]
public class Button_Box_ScriptableObject : ScriptableObject
{
    [Header("Button Box Properties")]
    [Tooltip("Size delta of the buttons in the box.")] public Vector2 buttonSize;
    [Tooltip("Gap between walls of the screen and the button box.")] public Vector2 outerMargin;
    [Tooltip("Gap between the buttons.")] public Vector2 buttonMargin;
    [Tooltip("Offset from the top left corner of the box.")] public Vector2 cornerOffset;
    [Tooltip("How many buttons are there in a row for every button in a column.")] public int ratio;
    [Tooltip("How much the box occupies it's maximum available space.")] [Range(0.1f, 1f)] public float boxWidthPercent;

}
