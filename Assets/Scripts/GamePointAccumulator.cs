using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePointAccumulator //: MonoBehaviour
{
    int gamePoint = 0;

    // Start is called before the first frame update
    public int GamePoint
    {
        get
        {
            return gamePoint;
        }
    }

    // Update is called once per frame
    public void Accumulate(int value)
    {
        gamePoint += value;
        Debug.Log("Game Point : " + gamePoint);
        PlayerStatePanel playerStatePanel = PanelManager.GetPanel(typeof(PlayerStatePanel)) as PlayerStatePanel;
        playerStatePanel.SetScore(gamePoint);
    }

    public void Reset()
    {
        gamePoint = 0;
    }
}
