using System.Collections.Generic;

namespace FFF.Scripts.Framework.Data.DataFramework
{
    public class ChangedDataInLevel : IBaseInfo
    {
        public void Initialize()
        {
        }

        public void Clear()
        {
            PreLevelUsedGoods.Clear();
            InLevelUsedGoods.Clear();
            BuyLifeCount = 0;
            WatchAdCount = 0;
            WheelGotLifeCount = 0;
        }
        
        // 本关卡前置道具使用了啥
        public Dictionary<int, int> PreLevelUsedGoods { get; } = new Dictionary<int, int>();

        /// <summary>
        /// 使用关卡前置道具
        /// </summary>
        /// <param name="goodsId">道具ID</param>
        public void AddPreLevelGoodsUseCount(int goodsId)
        {
            if (PreLevelUsedGoods.ContainsKey(goodsId))
            {
                PreLevelUsedGoods[goodsId]++;   
            }
            else
            {
                PreLevelUsedGoods.Add(goodsId, 1);
            }
        }

        // 本关卡内道具使用了啥
        public Dictionary<int, int> InLevelUsedGoods { get; } = new Dictionary<int, int>();

        /// <summary>
        /// 使用关卡内道具
        /// </summary>
        /// <param name="goodsId">道具ID</param>
        public void AddInLevelGoodsUseCount(int goodsId)
        {
            if (InLevelUsedGoods.ContainsKey(goodsId))
            {
                InLevelUsedGoods[goodsId]++;    
            }
            else
            {
                InLevelUsedGoods.Add(goodsId, 1);
            }
            
        }
        /// <summary>
        /// 取消使用关卡内道具
        /// </summary>
        /// <param name="goodsId">道具ID</param>
        public void ReduceInLevelGoodsUseCount(int goodsId)
        {
            InLevelUsedGoods[goodsId]--;
            if (InLevelUsedGoods[goodsId] == 0)
            {
                InLevelUsedGoods.Remove(goodsId);
            }
        }

        // 金币买步数的次数
        public int BuyLifeCount { get; private set; }

        public void AddBuyLifeCount()
        {
            BuyLifeCount++;
        }

        // 看广告次数
        public int WatchAdCount { get; private set; }

        public void AddWatchAdCount()
        {
            WatchAdCount++;
        }

        // 转盘抽中步数的次数
        public int WheelGotLifeCount { get; private set; }

        public void AddWheelGotLifeCount()
        {
            WheelGotLifeCount++;
        }
    }
}