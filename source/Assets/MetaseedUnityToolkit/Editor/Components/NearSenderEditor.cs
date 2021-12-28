using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Linq;
using System;
using System.Dynamic;
using UnityEngine.UI;
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

            string _deposit = UnitConverter.GetNearFormat(_target.deposit).ToString();
            _deposit = EditorGUILayout.TextField("Amount: ", _deposit).Replace('.', ',');
            _target.deposit = UnitConverter.GetYoctoNearFormat( Convert.ToDouble(_deposit) );

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (!_target.IsCallDataValid(_target.receiverId, _target.deposit)) GUI.enabled = false;

            if (GUILayout.Button("Send"))
            {
                GUI.enabled = true;
                _target.SendNear(_target.receiverId, _target.deposit, _target.actor);
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
