using UnityEngine;
using TMPro;
public class UI_Statsbox : MonoBehaviour
{
    TextMeshProUGUI headerTMP;
    TextMeshProUGUI statsTMP;

    /// <summary>
    /// Enables the <c>Statsbox</c> object.
    /// </summary>
    public void Enable()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Disables the <c>Statsbox</c> object.
    /// </summary>
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets statsboxes name.
    /// </summary>
    public void SetName(string name)
    {
        gameObject.name = "Statsbox (" + name + ")";
    }

    /// <summary>
    /// Updates all of the text inside <c>Statsbox</c> with the stats given as arguments.
    /// </summary>
    public void UpdateBox(int speed, int agility, int resistance, string playerName)
    {
        string statsText = "";
        statsText += "Speed: " + speed.ToString("D3") + "\n";
        statsText += "Agility: " + agility.ToString("D3") + "\n";
        statsText += "Resistance: " + resistance.ToString("D3");

        statsTMP.text = statsText;
        headerTMP.text = playerName;
    }

    /// <summary>
    /// Initializes the <c>Statsbox</c> (basically Awake but for a prefab).
    /// </summary>
    public void Init()
    {
        //Could do null checks here but figured they wouldn't fix anything and would make debugging harder.
        headerTMP = gameObject.transform.Find("TMP - Name").GetComponent<TextMeshProUGUI>();
        statsTMP = gameObject.transform.Find("TMP - Stats").GetComponent<TextMeshProUGUI>();
    }

}
