using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Tables;
using UnityEngine;

public class MailData
{
    public int Id;
    public int ServerId;

    //邮件照片
    public string Photo;

    //邮件标题
    public string Title;

    //邮件正文
    public string Content;

    //邮件奖励
    public List<Triangle<int, int, int>> Reward;

    //发件人
    public string Sender;
    public string Yours;

    public MailData()
    {
    }

    public MailData(int infoId)
    {
        Id = infoId;

        //TableMail tableMail = Table.Mail.Get(infoId);
        //if (tableMail != null)
        //{
        //    ServerId = -1;
        //    Photo = tableMail.Photo;
        //    Title = tableMail.Title;
        //    Content = tableMail.Content;
        //    Reward = tableMail.Reward;
        //    Sender = tableMail.Sender;
        //    Yours = tableMail.Yours;
        //}
        //else
        //{
        //    DebugUtil.LogError("not in table mail id: " + infoId);
        //}
    }

    public MailData(int serverId, string title, string content, string reward, string sender)
    {
        ServerId = serverId;
        Title = title;
        Content = content;
        Reward = ParasStr2Triangles(reward);

        Sender = sender;
    }

    private List<Triangle<int, int, int>> ParasStr2Triangles(string str)
    {
        List<Triangle<int, int, int>> triangleList = new List<Triangle<int, int, int>>();
        string[] arrSplit = str.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
        for (int index = 0; index < arrSplit.Length; index++)
        {
            string s = arrSplit[index];
            Triangle<int, int, int> triangle;
            try
            {
                string[] split = s.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
                switch (split.Length)
                {
//                    case 1:
//                        triangle = new Triangle<int, int, int>(int.Parse(split[0]));
//                        break;
                    case 2:
                        triangle = new Triangle<int, int, int>(int.Parse(split[0]), int.Parse(split[1]));
                        break;
                    case 3:
                        triangle = new Triangle<int, int, int>(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
                        break;
                    default:
                        triangle = null;
                        break;
                }
            }
            catch (Exception e)
            {
                DebugUtil.LogError(e.ToString());
                triangle = null;
            }

            if (triangle != null)
            {
                triangleList.Add(triangle);
            }
        }

        return triangleList;
    }
}
