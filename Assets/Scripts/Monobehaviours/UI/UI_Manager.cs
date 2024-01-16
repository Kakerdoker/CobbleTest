using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [Header("Manager Script References")]
    [SerializeField] Player_Manager playerManager;

    [Header("Miscellaneous References")]
    [SerializeField] GameObject canvas;
    [SerializeField] RectTransform canvasTransform;
    [SerializeField] GameObject statsboxHolder;
    [SerializeField] GameObject buttonBoxHolder;
    [SerializeField] Button_Box_ScriptableObject buttonBox;


    [Header("Prefab References")]
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject statsboxPrefab;

    /// <summary>Stores the currently selected <c>Player</c> as the next leader.</summary>
    [HideInInspector] public Player selectedPlayer { get; private set; }
    /// <summary>Stores all of the buttons shown on the Canvas.</summary>
    List<UI_Button> buttonList;
    /// <summary>Stores all of the statsboxes shown on the Canvas.</summary>
    List<UI_Statsbox> statsboxList;



    /// <summary>
    /// Places all of the buttons found inside of <c>buttonList</c> in their desired position on the Canvas.
    /// </summary>
    public void PositionButtons()
    {
        if (buttonList == null || buttonList.Count == 0)
            return;

        int buttonAmount = buttonList.Count;
        int maxHeight = GetMaxHeight(buttonAmount);
        int buttonIndex = 0;
        int remainder = buttonAmount % maxHeight;//How many rows of buttons need to add an extra column.

        Vector2 positionVector, scale, startingPosition;
        (positionVector, scale, startingPosition) = CalculateBoxProperties(maxHeight);

        for (int row = 0; row < maxHeight; row++)
        {
            int width = buttonAmount / maxHeight;

            //Add an extra row to the column.
            if (remainder > 0) 
            {
                width++; 
                remainder--;
            }
            for (int column = 0; column < width; column++)
            {
                Vector2 buttonCoordinates = new Vector2(column, row);
                Vector2 position = (buttonCoordinates * positionVector) + startingPosition;

                RectTransform buttonTransform = (RectTransform)buttonList[buttonIndex].transform;
                ChangeButtonTransform(position, scale, buttonTransform);

                //Return after all buttons have been positioned.
                buttonIndex++;
                if (buttonIndex == buttonAmount) return;
            }
        }
    }

    /// <summary>
    /// Adds a new button used for selecting the <c>Player</c> passed as a parameter.
    /// </summary>
    public void AddButton(Player player)
    {
        HandleNewButtonInstance(player);
        PositionButtons();
    }

    /// <summary>
    /// Wrapper function for changing the <c>selectePlayer</c>.
    /// </summary>
    public void ChangeSelectedPlayer(Player player)
    {
        selectedPlayer = player;
    }

    /// <summary>
    /// Changes the scale of every <c>statsbox</c> so it won't be unnaturally large or small for the Canvas.
    /// </summary>
    public void ResizeStatsboxes()
    {
        if (statsboxList == null || statsboxList.Count == 0)
            return;

        float scale = CalculateStatsScale();
        for(int i = 0; i < statsboxList.Count; i++)
        {
            statsboxList[i].gameObject.transform.localScale = new Vector2(scale, scale);
        }
    }

    /// <summary>
    /// Adds a new <c>statsbox</c> used for displaying stats of a <c>Player</c>.
    /// </summary>
    public UI_Statsbox AddStatsbox()
    {
        GameObject statsboxObject = Instantiate(statsboxPrefab);

        statsboxObject.transform.parent = statsboxHolder.transform;

        UI_Statsbox statsboxScript = statsboxObject.GetComponent<UI_Statsbox>();
        statsboxList.Add(statsboxScript);
        statsboxScript.Init();

        return statsboxScript;
    }

    /// <summary>
    /// Instantiates and initializes a new <c>Button</c>.
    /// </summary>
    private void HandleNewButtonInstance(Player playerScript)
    {
        GameObject buttonInstance = Instantiate(buttonPrefab);

        buttonInstance.name = "Button - " + playerScript.name;
        buttonInstance.transform.parent = buttonBoxHolder.transform;

        UI_Button buttonScript = buttonInstance.GetComponent<UI_Button>();
        buttonScript.Init(playerScript);
        buttonList.Add(buttonScript);
    }

    /// <summary>
    /// Returns the desired scale of <c>statsboxes</c>.
    /// </summary>
    private float CalculateStatsScale()
    {
        float shorterAxis;
        if (canvasTransform.sizeDelta.x < canvasTransform.sizeDelta.y)
            shorterAxis = canvasTransform.sizeDelta.x;
        else
            shorterAxis = canvasTransform.sizeDelta.y;

        return shorterAxis / 500f;
    }

    /// <summary>
    /// Calculates properties used for placing the buttons inside the box.<br/>
    /// The box is a metaphor for the bounds in which the buttons are placed.<br/>
    /// It returns a tuple containing three Vector2 properties: <br/><br/>
    /// <c>positionVector</c>: A vector which if multiplied by a buttons coordinates returns the buttons desired position within the button box.<br/>
    /// <c>scale</c>: Scale of the button.<br/>
    /// <c>startingPosition</c>: Starting position of the button box/Place from which buttons need to be placed.<br/>
    /// </summary>
    private (Vector2, Vector2, Vector2) CalculateBoxProperties(int maxHeight)
    {
        int maxWidth = maxHeight * buttonBox.ratio;

        //Where the box will start
        Vector2 canvasTopLeft = (canvasTransform.sizeDelta / -2f);

        //Corrects for the offset made by starting from the corner (half of the first buttton is outside the camera).
        Vector2 cornerAdj = buttonBox.buttonSize / 2f;

        //Extra margin away from the walls of the Canvas (divided by two so box stays in the middle).
        Vector2 oMargin = buttonBox.outerMargin / 2;

        //Scale of the buttons and their margins. Needed to make sure buttons don't go outside their box when adding more.
        Vector2 scale = new Vector2(1f / maxHeight, 1f / maxHeight);

        //Maximum width of the box generated by buttons and their margin.
        float maxButtonWidth = buttonBox.buttonSize.x * maxWidth * scale.x;
        float maxPaddingWidth = buttonBox.buttonMargin.x * (maxWidth - 1) * scale.x;//-1 because there is always one gap less than the amount of buttons
        float maxBoxWidth = maxButtonWidth + maxPaddingWidth;

        //Required rescaling to fit the box perfectly inside the screen (excluding outer margin).
        float rescale = (canvasTransform.sizeDelta.x - buttonBox.outerMargin.x) / maxBoxWidth;

        //Make it smaller by a value given in the inspector (used for customization).
        rescale *= buttonBox.boxWidthPercent;

        scale *= rescale;

        //Corrects for the gap thats generated after buttons change size.
        Vector2 scaleAdj = -(new Vector2(1f, 1f) - scale) * buttonBox.buttonSize / 2f;

        //Vector that holds how far a button should move in each direction depending on buttons coordinates.
        Vector2 positionVector = (buttonBox.buttonMargin + buttonBox.buttonSize) * scale;

        //All the corrections required so the placing of the buttons starts from the correct position.
        Vector2 startingPosition = canvasTopLeft + scaleAdj + cornerAdj + oMargin + buttonBox.cornerOffset;

        //Flip properties along their y axis, so going further on that axis means going down on the canvas.
        positionVector.y *= -1f;
        startingPosition.y *= -1f;


        return (positionVector, scale, startingPosition);
    }

    /// <summary>
    /// Updates the <c>rectTransform</c> passed as an argument to the given <c>position</c> and <c>scale</c>.<br/>
    /// Also updates their size that's given in the inspector.
    /// </summary>
    private void ChangeButtonTransform(Vector2 position, Vector2 scale, RectTransform rectTransform)
    {
        rectTransform.sizeDelta = buttonBox.buttonSize;
        rectTransform.anchoredPosition = position;
        rectTransform.localScale = scale;
    }

    /// <summary>
    /// Return the maximum amount of rows in the button box for a given <c>buttonAmount</c>.
    /// </summary>
    private int GetMaxHeight(int buttonAmount)
    {
        return (int)Mathf.Ceil(Mathf.Sqrt((float)buttonAmount / (float)buttonBox.ratio));
    }

    void Start()
    {
        statsboxList = new List<UI_Statsbox>();
        buttonList = new List<UI_Button>();
    }

}
