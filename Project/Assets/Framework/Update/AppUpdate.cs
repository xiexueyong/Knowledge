using System.Collections;
using System.Collections.Generic;
using Framework.Asset;
using static GameController;

public class AppUpdate
{
    public static IEnumerator Update()
    {
        UpgradeInfo upgradeInfo = new UpgradeInfo();
       
        yield return AssetBundleManager.Inst.Upgrade(upgradeInfo);

        //TODO:下载进度条
        //测试start
        //UIAnnouncement uiAnnouncement_test = UIManager.Inst.ShowUI(UIModuleEnum.UIAnnouncement,null, "testtitle", "update contetnt ..", null, false) as UIAnnouncement;
        //while (!uiAnnouncement_test.isClosed)
        //{
        //    yield return null;
        //}
        //测试end


        //大版本更新
        //0：不更新，1：强制更新，2：非强制更新
        if (upgradeInfo.appUpdateType > 0 && !string.IsNullOrEmpty(upgradeInfo.appUrl))
        {
            //yield return gameLoading.mockLoginProgressEnd();
            //升级App
            if (!string.IsNullOrEmpty(upgradeInfo.noticeContent))
            {
                UIAnnouncement uiAnnouncement = UIManager.Inst.ShowUI(UIName.UIAnnouncement,false, upgradeInfo.noticeTitle, upgradeInfo.noticeContent, upgradeInfo.appUrl, upgradeInfo.appUpdateType != (int)UpdateType.Upgrade) as UIAnnouncement;
                while (!uiAnnouncement.isClosed)
                {
                    yield return null;
                }
            }
            yield break;
        }else if (upgradeInfo.assetUpdateType == (int)UpdateType.Upgrade && !string.IsNullOrEmpty(upgradeInfo.assetUrl))
        {
            UIGameLoading uiGameLoading = UIManager.Inst.ShowUI(UIName.UIGameLoading) as UIGameLoading;

            //下载资源
            ProgressInfo progressInfo = new ProgressInfo();
            uiGameLoading.progressInfo = progressInfo;
            //gameLoading.StartProgress(progressInfo);
            yield return AssetBundleManager.Inst.UpdateBaseAssetBundle(upgradeInfo.assetUrl, progressInfo, true);
            //UIManager.Inst.CloseUI(UIModuleEnum.UIGameLoading);
            //UIManager.Inst.ShowUI(UIModuleEnum.UIGameLogin);
            //重置已经初始化的Table和Res
            if (!progressInfo.fail && progressInfo.sucess)
            {
                Res.Inst.UnloadAll(false);
                Table.Clear();
                Res.Init();
                Table.Init();
            }
            UIManager.Inst.CloseUI(UIName.UIGameLoading);
            // //资源更新更告
             if (!string.IsNullOrEmpty(upgradeInfo.noticeContent))
            {
                UIAnnouncement uiAnnouncement = UIManager.Inst.ShowUI(UIName.UIAnnouncement, false, upgradeInfo.noticeTitle, upgradeInfo.noticeContent, "", false) as UIAnnouncement;
                while (!uiAnnouncement.isClosed)
                {
                    yield return null;
                }
            }
        }



    }

}
