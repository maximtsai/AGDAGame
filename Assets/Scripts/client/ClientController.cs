using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using DarkRift;

public class ClientController : MonoBehaviour
{
    public string IP = "127.0.0.1";

    void Start()
    {
        DarkRiftAPI.Connect(IP);
        DarkRiftAPI.SendMessageToServer(100, 255, null);
    }

    void Update()
    {
        if (DarkRiftAPI.isConnected)
        {
            DarkRiftAPI.Receive();
        }
    }

    void OnApplicationQuit()
    {
        if (DarkRiftAPI.isConnected)
        {
            DarkRiftAPI.Disconnect();
        }
    }
}
