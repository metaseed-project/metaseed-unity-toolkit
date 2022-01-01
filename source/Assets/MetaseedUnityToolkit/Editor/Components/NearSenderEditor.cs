using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Linq;
using System;
using Newtonsoft.Json;
using System.Dynamic;
using UnityEngine.UI;
using NearClient.Utilities;
using MetaseedUnityToolkit;

[CustomEditor(typeof(NearSender))]
public class NearSenderEditor : Editor
{
    private NearSender _target;

    void OnEnable()
    {
        _target = ((NearSender)target);
    }

    int selectedAction = 0;
    public override void OnInspectorGUI()
    {
        bool isSelectedRoleConnected = IsSelectedRoleConnected();
        if (isSelectedRoleConnected)
        {
            EditorGUILayout.Space();

            _target.receiverId = EditorGUILayout.TextField("Receiver address: ", _target.receiverId);

            _target.deposit = EditorGUILayout.TextField("Amount: ", _target.deposit.ToString());

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (!_target.IsComponentDataValid()) GUI.enabled = false;

            if (GUILayout.Button("Send"))
            {
                GUI.enabled = true;
                SendAndWaitForResult();
            }

            GUI.enabled = true;

            EditorGUILayout.Space();
        }
        else
        {
            EditorGUILayout.Space();

            if (selectedRole == 0)
            {
                GUILayout.Label("You should connect player account first");
                GUILayout.Label("Drag the 'Player Connect' component somewhere in your scene and press connect.", EditorStyles.miniLabel);
            }
            else if (selectedRole == 1)
            {
                GUILayout.Label("You should connect developer account first");
                GUILayout.Label("Open Near > Developer Account and press connect", EditorStyles.miniLabel);
            }

        }
    }

    public async void SendAndWaitForResult()
    {
        Debug.Log("Transaction is pending");
        dynamic result = await _target.SendNear();
        Debug.Log(JsonConvert.SerializeObject(result));
    }

    private int selectedRole = 0;
    private bool IsSelectedRoleConnected()
    {
        string[] options = new string[] { "Player", "Developer" };
        selectedRole = EditorGUILayout.Popup("Choose your role:", selectedRole, options);
        if (selectedRole == 0) _target.actor = EConnectionActor.Player;
        else if (selectedRole == 1) _target.actor = EConnectionActor.Developer;

        return ConnectionsManager.IsConnected(_target.actor);
    }
}
