using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Linq;
using System;
using MetaseedUnityToolkit;

[CustomEditor(typeof(ContractCaller))]
public class ContractCallerEditor : Editor
{
    private ContractCaller _target;

    private SerializedProperty listProperty;

    void OnEnable()
    {
        _target = ((ContractCaller)target);
        listProperty = serializedObject.FindProperty("arguments");
    }

    int selectedAction = 0;
    public override void OnInspectorGUI()
    {
        bool isSelectedRoleConnected = IsSelectedRoleConnected();
        if (isSelectedRoleConnected)
        {
            EditorGUILayout.Space();

            _target.contractAddress = EditorGUILayout.TextField("Contract address: ", _target.contractAddress);

            _target.contractMethod = EditorGUILayout.TextField("Method: ", _target.contractMethod);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            string[] options = new string[] { "Call", "View" };
            selectedAction = EditorGUILayout.Popup("Choose action type:", selectedAction, options);

            EditorGUILayout.Space();

            // Ofcourse you also want to change the list size here
            listProperty.arraySize = EditorGUILayout.IntField("Arguments", listProperty.arraySize);

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                var dialogue = listProperty.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(dialogue, new GUIContent("Argument "), true);
            }

            // Note: You also forgot to add this
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            if (!_target.IsCallDataValid()) GUI.enabled = false;

            if (selectedAction == 0)
            { 
                if (GUILayout.Button("Call contract"))
                {
                    GUI.enabled = true;
                    _target.CallContractWithParameters(_target.contractAddress, _target.contractMethod, _target.arguments, _target.actor, _target.gas, _target.deposit);
                }
            }
            else if (selectedAction == 1)
            {
                if (GUILayout.Button("View contract"))
                {
                    GUI.enabled = true;
                    _target.ViewContractWithParameters(_target.contractAddress, _target.contractMethod, _target.arguments,  _target.actor);
                }

            }
            GUI.enabled = true;

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (selectedAction == 0)
            {
                DrawExtraSettings();
            }
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


    bool showExtraSettings = false;
    void DrawExtraSettings()
    {
        showExtraSettings = EditorGUILayout.Toggle("Settings", showExtraSettings);
        if (showExtraSettings)
        {
            string tGasField = UnitConverter.GetTGasFormat(_target.gas).ToString();
            tGasField = EditorGUILayout.TextField("TGas: ", tGasField).Replace('.', ',');
            _target.gas = UnitConverter.GetGasFormat( Convert.ToDouble(tGasField) );

            string depositNearField = UnitConverter.GetNearFormat(_target.deposit).ToString();
            depositNearField = EditorGUILayout.TextField("Deposit: ", depositNearField).Replace('.', ',');
            _target.deposit = UnitConverter.GetYoctoNearFormat( Convert.ToDouble(depositNearField) );
        }
        else
        {
            _target.gas = UnitConverter.GetGasFormat(10.0);
            _target.deposit = UnitConverter.GetYoctoNearFormat(0.1);
        }
    }
}
