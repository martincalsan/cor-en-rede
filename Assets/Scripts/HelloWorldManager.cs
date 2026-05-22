using Unity.Netcode;
using UnityEngine;

public class HelloWorldManager : MonoBehaviour
{
    void OnGUI()
    {
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
            return;

        if (GUILayout.Button("Host"))   NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
    }
}