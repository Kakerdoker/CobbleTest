using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Button : MonoBehaviour
{
    /// <summary>Player selected by the button upon clicking it.</summary>
    Player selectingPlayer;
    /// <summary>Stores a reference to the built-in Unity Button component.</summary>
    Button buttonComponent;
    /// <summary>Stores a reference to the built-in Unity TextMeshPro component.</summary>
    TextMeshProUGUI buttonTMP;
    /// <summary>Stores a reference to the UI_Manager script.</summary>
    UI_Manager uiManager;

    /// <summary>
    /// Initializes the Script (basically the constructor).<br/>
    /// The <c>Player</c> passed as an argument will be chosen as the next leader upon clicking the Button.
    /// </summary>
    public void Init(Player player)
    {
        //I would prefer to do most of this in the inspector but since UI_Button is used by a prefab I have to do it this way.

        buttonComponent = gameObject.GetComponent<Button>();
        buttonTMP = buttonComponent.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        buttonComponent.onClick.AddListener(Click);

        uiManager = GameObject.Find("UI Manager").GetComponent<UI_Manager>();

        selectingPlayer = player;
        buttonTMP.text = player.gameObject.name;
    }
    public void Click()
    {
        uiManager.ChangeSelectedPlayer(selectingPlayer);
    }




}
