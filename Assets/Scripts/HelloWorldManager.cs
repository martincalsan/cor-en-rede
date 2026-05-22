using Unity.Netcode;
using UnityEngine;

public class HelloWorldManager : MonoBehaviour
{
    void OnGUI()
    {
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
            return;

        if (GUILayout.Button("Host"))
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = ApproveConnection;
            NetworkManager.Singleton.StartHost();
        }
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
    }

    void ApproveConnection(
        NetworkManager.ConnectionApprovalRequest req,
        NetworkManager.ConnectionApprovalResponse res)
    {
        res.Approved = NetworkManager.Singleton.ConnectedClientsIds.Count < 6;
        res.CreatePlayerObject = res.Approved;
    }
}