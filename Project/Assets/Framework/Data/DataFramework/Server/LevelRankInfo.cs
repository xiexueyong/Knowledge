using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace FFF.Scripts.Framework.Data.DataFramework.Server
{
    public class LevelRankInfo
    {
        public int SelfRank;
        public int SelfScore;
        public List<UserLevelRankItem> UserLevelRankList;

        public LevelRankInfo()
        {
            UserLevelRankList = new List<UserLevelRankItem>();
            SelfRank = -1;
            SelfScore = 0;
        }

        /// <summary>
        /// 获取关卡好友排行数据
        /// </summary>
        /// <param name="level">关卡等级</param>
        /// <param name="successCallback">获取成功回调</param>
        public void GetFriendsLevelRankData(int level, int score, Action successCallback = null)
        {
            SelfRank = -1;
            
            GetFriendsLevelRankBody body = new GetFriendsLevelRankBody(URLConfig.GetFriendsLevelRank);
            body.level = level;
            body.score = score;
            HttpRequestTool.SendMessage(
                body
                , (x) =>
                {
                    Debug.Log("GetFriendsLevelRankData Sucess :" + x);
                    UserLevelRankList = ParseLevelRankInfo(x);
                    if (successCallback != null)
                    {
                        successCallback();
                    }
                }
                , (x) =>
                {
                    Debug.LogError("GetFriendsLevelRankData Fail :" + x);
                }
            );
        }
        
        List<UserLevelRankItem> ParseLevelRankInfo(string response)
        {
            var levelRankList = new List<UserLevelRankItem>();
            try
            {
                if (string.IsNullOrEmpty(response))
                {
                    return levelRankList;
                }
                JSONNode res = JSONNode.Parse(response);
                JSONArray rankJsonInfos = res["rankInfo"] as JSONArray;
                JSONNode selfInfo = res["selfInfo"];
                if (rankJsonInfos != null && rankJsonInfos.Count > 0)
                {
                    foreach (var item in rankJsonInfos)
                    {
                        levelRankList.Add(new UserLevelRankItem(item));
                    }
                }
                SelfRank = selfInfo["rank"];
                SelfScore = selfInfo["score"];
            }
            catch(Exception e)
            {
                return levelRankList;
            }
            return levelRankList;
        }
    }

    /// <summary>
    /// 关卡等级排行item
    /// </summary>
    public class UserLevelRankItem
    {
        public UserLevelRankItem(JSONNode node)
        {
            if (node["isSelf"] == 1)
            {
                UserName = DataManager.Inst.userInfo.NickName;
            }
            else
            {
                UserName = node["UserName"];   
            }
            UserLevelRank = node["UserLevelRank"];
            UserLevelScore = node["UserLevelScore"];
        }
        
        public string UserName;
        public int UserLevelRank;
        public int UserLevelScore;
    }
}