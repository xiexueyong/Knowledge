using System.Collections.Generic;
using System.Linq;
using EventUtil;
using Framework.Storage;
using UnityEngine;

public class MailLogic
{
    private StorageUserInfo _storageUserInfo;

    public void Init()
    {
        _storageUserInfo = StorageManager.Inst.GetStorage<StorageUserInfo>();
        //EventDispatcher.AddEventListener<string, string>(StorageAccountInfo.StorageAccountInfo_Change_AppVersion, OnAppVersionChange);
    }
    public void Refreshe()
    {
        _storageUserInfo = StorageManager.Inst.GetStorage<StorageUserInfo>();
    }
   

   
    private void DelServerMailCallback(List<int> deletedServerMails)
    {
        Debug.Log(" remove server mail call back deletedServerMails.count" + deletedServerMails.Count);
        foreach (int deletedServerMail in deletedServerMails)
        {
            Debug.Log("remove read server mail storage:" + deletedServerMail);
            //_storageUserInfo.ReadServerMail.Remove(deletedServerMail);
        }
    }

  
}
