using UnityEngine;
using TMPro;
public class UI_Statsbox : MonoBehaviour
{
    TextMeshProUGUI headerTMP;
    TextMeshProUGUI statsTMP;

    public void Enable()
    {
        gameObject.SetActive(true);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void UpdateBox(int speed, int agility, int resistance, string playerName)
    {
        string statsText = "";
        statsText += "Speed: " + speed.ToString("D3") + "\n";
        statsText += "Agility: " + agility.ToString("D3") + "\n";
        statsText += "Resistance: " + resistance.ToString("D3");

        statsTMP.text = statsText;
        headerTMP.text = playerName;
    }

    public void Init()
    {
        //Could do null checks here but figured they wouldn't fix anything and would make debugging harder.
        headerTMP = gameObject.transform.Find("TMP - Name").GetComponent<TextMeshProUGUI>();
        statsTMP = gameObject.transform.Find("TMP - Stats").GetComponent<TextMeshProUGUI>();
    }

}
