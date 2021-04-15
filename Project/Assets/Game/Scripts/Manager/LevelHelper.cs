using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Tables;
using UnityEngine;

public class LevelHelper : D_MonoSingleton<LevelHelper>
{
    public bool anwserRight;
    public bool buyExplain;
    public int rightStreak;
    
    
    
    
    
    
    
    public static TableDegree getDegree(int level,out int levelBottom)
    {
        var a = Table.Degree.GetAll();
        a.Sort((x, y) =>
            {
                if (x.levelTop < y.levelTop)
                {
                    return -1;
                }
                if (x.levelTop == y.levelTop)
                {
                    return 0;
                }
                if (x.levelTop > y.levelTop)
                {
                    return 1;
                }
                return 0;
            }
        );

        levelBottom = 0;
        for (int i=0;i<a.Count;i++)
        {
            if (a[i].levelTop >= level)
            {
                return a[i];
            }

            levelBottom = a[i].levelTop+1;
        }
        return null;
    }
}
