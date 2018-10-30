using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class waveText : MonoBehaviour
{
    public Text waveNumber;

    void Start()
    {
        waveNumber = GetComponent<Text>();
    }

    public void updateWaveNum()
    {
        waveNumber.text = GameController.waveNum.ToString();
    }
}
