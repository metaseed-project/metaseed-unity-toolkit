using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Linq;
using System;
using MetaseedUnityToolkit;

[CustomEditor(typeof(PlayerConnector))]
public class PlayerConnectorEditor : Editor
{
    private PlayerConnector _target;

    void OnEnable()
    {
        _target = ((PlayerConnector)target);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        if (!ConnectionsManager.IsConnected(EConnectionActor.Player))
        {

            EditorGUILayout.Space();

            if (GUILayout.Button("Connect Player Wallet"))
            {
                _target.ConnectWalletByBrowser(EConnectionActor.Player);
            }
        }
        else
        {
            GUILayout.Label("Player wallet: " + PluginStorage.PlayerNearAccountId, EditorStyles.label);
        }
    }
}
