using System;
using Framework.Storage;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : IBaseInfo
{
    public void Initialize()
    {
    }

    public StorageList<int> AlreadyGuide
    {
        get { return StorageManager.Inst.GetStorage<StorageUserInfo>().AlreadyGuide; }
    }

    public void SaveGuide(int groupId)
    {
        if (!AlreadyGuide.Contains(groupId))
        {
            AlreadyGuide.Add(groupId);
        }
    }

    //购买过的商品
    public StorageList<string> Purchased
    {
        get { return StorageManager.Inst.GetStorage<StorageUserInfo>().Purchased; }
    }
    
    //头像ID
    public int HeadIconID
    {
        get { return StorageManager.Inst.GetStorage<StorageUserInfo>().HeadIconID; }
        set { StorageManager.Inst.GetStorage<StorageUserInfo>().HeadIconID = value; }
    }

    //昵称
    public string NickName
    {
        get { return StorageManager.Inst.GetStorage<StorageUserInfo>().NickName; }
        set { StorageManager.Inst.GetStorage<StorageUserInfo>().NickName = value; }
    }

    //等级
    public int Level
    {
        get
        {
            int lv = StorageManager.Inst.GetStorage<StorageUserInfo>().Level;
            if (lv <= 0)
            {
                Level = 1;
            }

            return StorageManager.Inst.GetStorage<StorageUserInfo>().Level;
        }
        set { StorageManager.Inst.GetStorage<StorageUserInfo>().Level = value; }
    }
    
    //金币
    public int Coins
    {
        get { return StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[Table.GoodsId.Coin]; }
    }
    public void AddCoin(int coin)
    {
        SetGoodsCount(Table.GoodsId.Coin, Coins+ coin);
    }
    
    //体力
    public int Energy
    {
        get { return StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[Table.GoodsId.Energy]; }
        set {
            int oldEnergy = Energy;
            if (oldEnergy >= MLife.LifeMaxCount && value < MLife.LifeMaxCount)
            {
                MLifeManager.Inst.StartCountDown();
            }
            int newEnergy = Mathf.Clamp(value, 0, MLife.LifeMaxCount);
            StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[Table.GoodsId.Energy] = newEnergy;

        }
    }

    public void ChangeEnergy(int count)
    {
        if (!MLifeManager.Inst.InfiniteLife())
        {
            Energy += count;
        }
    }

    private Dictionary<int, int> _newGoods = new Dictionary<int, int>();
    public Dictionary<int, int> NewGoods
    {
        set
        {
            _newGoods = value;
        }
        get
        {
            return _newGoods;
        }
    }

    public int GetGoodsCount(int goodsId)
    {
        return StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[goodsId];
    }

    public void SetGoodsCount(int goodsId, int count)
    {
        if (Table.GoodsId.InfiniteEnergy1H == goodsId)
        {
            MLifeManager.Inst.life.AddInfiniteLife(count * 3600);
            StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[goodsId] = 0;
        }
        else if (Table.GoodsId.InfiniteEnergy6H == goodsId)
        {
            MLifeManager.Inst.life.AddInfiniteLife(count * 3600 * 6);
            StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[goodsId] = 0;
        }
        else if (Table.GoodsId.InfiniteEnergy12H == goodsId)
        {
            MLifeManager.Inst.life.AddInfiniteLife(count * 3600 * 12);
            StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[goodsId] = 0;
        }
        else if (Table.GoodsId.InfiniteEnergy24H == goodsId)
        {
            MLifeManager.Inst.life.AddInfiniteLife(count * 3600 * 24);
            StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[goodsId] = 0;
        }
        else
        {
            // TableBI.Goods(eventName.ToString(), goodsId, count, 
            //     count - StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[goodsId]);
            StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[goodsId] = count;
        }
    }
    
    public void ChangeGoodsCount(int goodsId, int count)
    {
        int oldCount = GetGoodsCount(goodsId);
        // TableBI.Goods(eventName.ToString(), goodsId, oldCount + count, count);
        SetGoodsCount(goodsId, oldCount+ count);
        if (_newGoods.ContainsKey(goodsId))
        {
            _newGoods[goodsId] += count;
        }
        else
        {
            _newGoods.Add(goodsId, count);
        }
    }
}