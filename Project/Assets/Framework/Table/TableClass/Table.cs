using System;
using System.Collections.Generic;
using Framework.Tables;
using UnityEngine;
public class Table {
    public static bool Inited;
    
    public static void Init()
    {
        Inited = true;
        if (GameConfig.Inst.DebugEnable && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor))
        {
			Wheels.Init();
			GoodsId.Init();
			Goods.Init();
			GoodsBundle.Init();
			Store.Init();
			Inventory.Init();
			Sound.Init();
			Sounds.Init();
			Question.Init();
			Degree.Init();
			AdChannelPostfix.Init();
			DailyTarget.Init();
			Language.Init();
			GameConst.Init();

        }
    }

    public static void Clear(){
		Wheels.Clear();
		GoodsId.Clear();
		Goods.Clear();
		GoodsBundle.Clear();
		Store.Clear();
		Inventory.Clear();
		Sound.Clear();
		Sounds.Clear();
		Question.Clear();
		Degree.Clear();
		AdChannelPostfix.Clear();
		DailyTarget.Clear();
		Language.Clear();
		GameConst.Clear();

    }
	public static  TableWheels Wheels = new  TableWheels();

	public static  TableGoodsId GoodsId = new  TableGoodsId();

	public static  TableGoods Goods = new  TableGoods();

	public static  TableGoodsBundle GoodsBundle = new  TableGoodsBundle();

	public static  TableStore Store = new  TableStore();

	public static  TableInventory Inventory = new  TableInventory();

	public static  TableSound Sound = new  TableSound();

	public static  TableSounds Sounds = new  TableSounds();

	public static  TableQuestion Question = new  TableQuestion();

	public static  TableDegree Degree = new  TableDegree();

	public static  TableAdChannelPostfix AdChannelPostfix = new  TableAdChannelPostfix();

	public static  TableDailyTarget DailyTarget = new  TableDailyTarget();

	public static  TableLanguage Language = new  TableLanguage();

	public static  TableGameConst GameConst = new  TableGameConst();



    public static bool string2Bool (string str) {
        return str == "1" || str == "TRUE" || str == "true";
    }

    public static int[] string2ArrayInt (string str) {
        string[] temp = string2ArrayString (str);
        int[] result = new int[temp.Length];
        for (int i = 0; i < temp.Length; i++) {
            int f1;
            bool r = int.TryParse (temp[i], out f1);
            if (!r)
                f1 = 0;
            result[i] = f1;
        }
        return result;

    }
    public static Int2 string2Int2 (string str) {
        Int2 result = new Int2 (0, 0);
        if (!string.IsNullOrEmpty (str)) {
            string[] values = str.Split (':');
            if (values.Length == 2) {
                int f1, f2;
                if (int.TryParse (values[0], out f1))
                    result.field1 = f1;
                if (int.TryParse (values[1], out f2))
                    result.field2 = f2;
            }
        }
        return result;
    }
    public static float[] string2ArrayFloat (string str) {
        string[] temp = string2ArrayString (str);
        float[] result = new float[temp.Length];
        for (int i = 0; i < temp.Length; i++) {
            float f1;
            bool r = float.TryParse (temp[i], out f1);
            if (!r)
                f1 = 0f;
            result[i] = f1;
        }
        return result;
    }
    public static string[] string2ArrayString (string str) {
        if (string.IsNullOrEmpty (str)) {
            return new string[0];
        } else {
            return str.Split (',');
        }
    }

    public static Dictionary<int, int> string2Dic_int_int(string str)
    {
        Dictionary<int, int> result = new Dictionary<int, int>();
        if (!string.IsNullOrEmpty(str))
        {
            string[] pairs = str.Split(',');
            for (int i = 0; i < pairs.Length; i++)
            {
                string[] pairItems = pairs[i].Split(':');
                if (pairItems.Length >= 2)
                    result[int.Parse(pairItems[0])] = int.Parse(pairItems[1]);
                else
                    DebugUtil.LogError("Table.string2Dic_int_int parse error ,string format invalid: " + str);

            }
            return result;
        }
        return result;
    }
    public static Dictionary<string, string> string2Dic_string_string(string str)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(str))
        {
            string[] pairs = str.Split(',');
            for (int i = 0; i < pairs.Length; i++)
            {
                string[] pairItems = pairs[i].Split(':');
                if (pairItems.Length >= 2 && !string.IsNullOrEmpty(pairItems[0]) && !string.IsNullOrEmpty(pairItems[1]))
                    result[pairItems[0]] = pairItems[1];
                else
                    DebugUtil.LogError("Table.string2Dic_string_string parse error ,string format invalid: " + str);
            }
            return result;
        }
        return result;
    }
    public static Dictionary<string, int> string2Dic_string_int(string str)
    {
	    Dictionary<string, int> result = new Dictionary<string, int>();
	    if (!string.IsNullOrEmpty(str))
	    {
		    string[] pairs = str.Split(',');
		    for (int i = 0; i < pairs.Length; i++)
		    {
			    string[] pairItems = pairs[i].Split(':');
			    if (pairItems.Length >= 2 && !string.IsNullOrEmpty(pairItems[0]) && !string.IsNullOrEmpty(pairItems[1]))
			    {
				    int a;
				    if(int.TryParse(pairItems[1],out a))
						result[pairItems[0]] = a;
			    }
			    else
				    DebugUtil.LogError("Table.string2Dic_string_string parse error ,string format invalid: " + str);
		    }
		    return result;
	    }
	    return result;
    }

    
    public static Triangle<int,int,int> string2Triangle_int_int_int(string str)
    {
        Triangle<int, int, int> result = new Triangle<int, int, int>();
        if (!string.IsNullOrEmpty(str))
        {
            string[] values = str.Split(':');
            for (int i = 0; i < values.Length; i++)
            {
                try
                {
                    switch (i)
                    {
                        case 0:
                            result.V1 = int.Parse(values[0]);
                            break;
                        case 1:
                            result.V2 = int.Parse(values[1]);
                            break;
                        case 2:
                            result.V3 = int.Parse(values[2]);
                            break;
                    }
                }catch(Exception e)
                {
                    DebugUtil.LogError("Table.string2Triangle_int_int_int parse error ,string format invalid: " + str);
                }
            }
            return result;
        }
        return null;
    }
    public static List<Triangle<int, int, int>> string2List_Triangle_int_int_int(string str)
    {
        List<Triangle<int, int, int>> result = new List<Triangle<int, int, int>>();
        if (!string.IsNullOrEmpty(str))
        {
            string[] pairs = str.Split(',');
            for (int i = 0; i < pairs.Length; i++)
            {
                try
                {
                    result.Add(Table.string2Triangle_int_int_int(pairs[i]));
                }
                catch (Exception e)
                {
                    DebugUtil.LogError("Table.string2List_Triangle_int_int_int parse error ,string format invalid: " + str);
                }
            }
            return result;
        }
        return result;
    }

    //public static Int2[] string2ArrayInt2(string str)
    //{
    //    if (string.IsNullOrEmpty(str))
    //    {
    //        return new Int2[0];
    //    }
    //    else
    //    {
    //        string[] pairs = str.Split('|');
    //        Int2[] result = new Int2[pairs.Length];
    //        for (int i = 0; i < pairs.Length; i++)
    //        {
    //            string[] pairItems = pairs[i].Split(',');
    //            result[i] = new Int2(int.Parse(pairItems[0]), int.Parse(pairItems[1]));
    //        }
    //        return result;
    //    }
    //}
}