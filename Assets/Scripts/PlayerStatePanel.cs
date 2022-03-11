using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatePanel : BasePanel
{
    [SerializeField]
    Text scoreValue;

    [SerializeField]
    Gage HPGage;

    public void SetScore(int value)
    {
        Debug.Log("SetScore value = " + value);
        scoreValue.text = value.ToString();
    }

    public void SetHP(float CurrenValue, float maxValue)
    {
        HPGage.SetHP(CurrenValue, maxValue);
    }
}
