using System;
using Framework;
using Framework.Storage;
using Newtonsoft.Json;
/***
 * Body数据
 * */
public class Body
{
    // 要调用的逻辑方法
    [JsonIgnore]
    public string method;
    public string name;
    [JsonIgnore]
    public string url;
    public string pf;
    public string lang;

    public string appVersion;
    public string gameUid;

    public Body(string method, string url = null)
    {
        this.method = method;
        this.name = method;
        this.pf = AssetBundleConfig.Inst.AppChannel.ToString();
        this.lang = LanguageTool.GetLangType().ToString();
        this.appVersion = DeviceHelper.GetAppVersion();

        if (StorageManager.Inst.Inited && StorageManager.Inst.GetStorage<StorageAccountInfo>() != null && !string.IsNullOrEmpty(this.gameUid = StorageManager.Inst.GetStorage<StorageAccountInfo>().Uid))
        {
            this.gameUid = StorageManager.Inst.GetStorage<StorageAccountInfo>().Uid.ToString();
        }

        if (string.IsNullOrEmpty(url))
        {
            switch (GameConfig.Inst.serverType)
            {
                case GameConfig.ServerType.Fotoable:
                    this.url = GameConfig.Inst.Fotoable_Gateway + method;
                    break;
                case GameConfig.ServerType.Local:
                    this.url = GameConfig.Inst.Local_Gateway + method;
                    break;
            }
        }
        else
        {
            this.url = url;
        }
    }
}