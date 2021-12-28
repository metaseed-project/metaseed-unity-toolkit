using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using System;
using Newtonsoft.Json;
using System.Dynamic;
using System.IO;
using MetaseedUnityToolkit;

public class NearAccountWindow : EditorWindow
{
    [MenuItem("Near/Developer Account")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(NearAccountWindow));
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        GUILayout.Label("Developer Wallet", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (!ConnectionsManager.IsConnected(EConnectionActor.Developer))
        {

            EditorGUILayout.Space();

            if (GUILayout.Button("Connect Wallet"))
            {
                WalletConnector.Connect("testnet", EConnectionActor.Developer);
            }
        }
        else
        {
            GUILayout.Label("wallet: " + PluginStorage.GameDeveloperNearAccountId, EditorStyles.label);

            EditorGUILayout.Space();

            if (GUILayout.Button("Disconnect"))
            {
                ConnectionsManager.Disconnect(EConnectionActor.Developer);
            }
        }
    }
}
