using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndShiftManager : MonoBehaviour
{
    [SerializeField] private Text _soulsEarnedShift;
    [SerializeField] private Text _soulsEarnedTotal;
    [SerializeField] private Text _header;

    public void SetValues(int currentday, int soulsEarned, int totalSouls)
    {
        _header.text = "end of day: " + currentday.ToString();
        _soulsEarnedShift.text = "souls earned during shift: " + soulsEarned.ToString();
        _soulsEarnedTotal.text = "total souls earned: " + totalSouls.ToString();
    }

    public void QuitGame()
    {
        Application.OpenURL("https://itch.io/jam/micro-jam-015/rate/2745568");
    }
}
