using Framework.Storage;
using Framework.Tables;

public class PayUserLayer : D_MonoSingleton<PayUserLayer>
{
    /// <summary>
    /// 获取分层名称
    /// </summary>
    /// <returns></returns>
    public string GetLayerName()
    {
        string layerName = "FreeUser";
        //var payLayerData = GetCurrentLayer();
        //if (null != payLayerData) {
        //    layerName = payLayerData.ShowName;
        //}

        return layerName;
    }

    /// <summary>
    /// 获取分层ID
    /// </summary>
    /// <returns></returns>
    public int GetLayerId()
    {
        int id = -1;
        //var payLayerData = GetCurrentLayer();
        //if (null != payLayerData) {
        //    id = payLayerData.Id;
        //}

        return id;
    }

    /// <summary>
    /// 是否所在层
    /// </summary>
    /// <param name="layerId"></param>
    /// <returns></returns>
    public bool IsInLayer(int layerId)
    {
        return false;
        //var data = GetCurrentLayer();
        //return data != null && data.Id == layerId;
    }

    public bool IsInLayer(int[] layerList)
    {
        bool isInLayer = false;
        foreach (var layerId in layerList) {
            if (IsInLayer(layerId)) {
                isInLayer = true;
//                Debug.LogError("所在层级:"+layerId);
                break;
            }
        }

        return isInLayer;
    }

    /// <summary>
    /// 获取分层数据
    /// </summary>
    /// <returns></returns>
    //TablePayLayer GetCurrentLayer()
    //{
    //    TablePayLayer data = null;
    //    //int payCount = StorageManager.Inst.GetStorage<StorageUserInfo>().PayCount;
    //    //foreach (var payLayerData in Table.PayLayer.GetAll()) {
    //    //    if (payLayerData.MinValue <= payCount && payLayerData.MaxValue > payCount) {
    //    //        data = payLayerData;
    //    //        break;
    //    //    }
    //    //}

    //    return data;
    //}
}