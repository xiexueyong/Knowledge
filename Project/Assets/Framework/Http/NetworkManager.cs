using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : D_MonoSingleton<NetworkManager>
{

    enum NetworkStatus
    {
        OFFLINE,
        WIFI,
        CARRIER_NETWORK
    };

    NetworkStatus networkStatus = NetworkStatus.WIFI;
    float networkTimer = 0;
    float heartBeatTimer = 0;
    public bool HasNetwork
    {
        get
        {
            return networkStatus != NetworkStatus.OFFLINE;
        }
    }

    void Update()
    {
        networkTimer += Time.deltaTime;
        if (networkTimer > 1.0f)
        {
            networkTimer = 0.0f;
            CheckNetwork();
        }

        heartBeatTimer += Time.deltaTime;
        if (heartBeatTimer > 30.0f)
        {
            heartBeatTimer = 0.0f;
            HeartBeat();
        }

    }

    void CheckNetwork()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                networkStatus = NetworkStatus.OFFLINE;
                break;
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                networkStatus = NetworkStatus.WIFI;
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                networkStatus = NetworkStatus.CARRIER_NETWORK;
                break;
        }
    }


    void HeartBeat()
    {
       
    }




}
