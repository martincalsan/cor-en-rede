using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    static readonly Color[] Colors =
    {
        Color.red, Color.blue, Color.green,
        Color.yellow, new Color(1f, 0.5f, 0f), Color.magenta
    };

    static readonly HashSet<Color> UsedColors = new();

    readonly NetworkVariable<Color> _color =
        new(writePerm: NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            transform.position = new Vector3(
                Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
            _color.Value = PickUniqueColor(Color.clear);
            UsedColors.Add(_color.Value);
        }

        _color.OnValueChanged += (_, current) =>
            GetComponent<Renderer>().material.color = current;

        GetComponent<Renderer>().material.color = _color.Value;
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer) UsedColors.Remove(_color.Value);
    }

    void OnGUI()
    {
        if (!IsOwner) return;
        if (GUILayout.Button("Change Color")) RequestColorChangeServerRpc();
    }

    [ServerRpc]
    void RequestColorChangeServerRpc()
    {
        Color newColor = PickUniqueColor(_color.Value);
        UsedColors.Remove(_color.Value);
        UsedColors.Add(newColor);
        _color.Value = newColor;
    }

    Color PickUniqueColor(Color current)
    {
        var free = new List<Color>();
        foreach (var c in Colors)
            if (!UsedColors.Contains(c) && c != current)
                free.Add(c);

        return free.Count > 0 ? free[Random.Range(0, free.Count)] : current;
    }
}