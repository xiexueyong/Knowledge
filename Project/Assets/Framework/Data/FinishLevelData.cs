using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class FinishLevelData : MonoBehaviour
{
    public int level;
    public int coinNum;
    public int flowerNum;

    public FinishLevelData() { }

    public FinishLevelData(int level, int coinNum, int flowerNum)
    {
        this.level = level;
        this.coinNum = coinNum;
        this.flowerNum = flowerNum;
    }
}
