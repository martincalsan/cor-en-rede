using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    static readonly Color[] Colors =
    {
        Color.red, Color.blue, Color.green,
        Color.yellow, new Color(1f, 0.5f, 0f), Color.magenta
    };

    readonly NetworkVariable<Color> _color =
        new(writePerm: NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            transform.position = new Vector3(
                Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
            _color.Value = Colors[Random.Range(0, Colors.Length)];
        }

        _color.OnValueChanged += (_, current) =>
            GetComponent<Renderer>().material.color = current;

        GetComponent<Renderer>().material.color = _color.Value;
    }

    void OnGUI()
    {
        if (!IsOwner) return;
        if (GUILayout.Button("Change Color")) RequestColorChangeServerRpc();
    }

    [ServerRpc]
    void RequestColorChangeServerRpc()
    {
        _color.Value = Colors[Random.Range(0, Colors.Length)];
    }
}