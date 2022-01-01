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


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        bool isSelectedRoleConnected = IsSelectedRoleConnected();
        if (isSelectedRoleConnected)
        {
            EditorGUILayout.Space();

            SerializedProperty contractAddressProp = serializedObject.FindProperty("contractAddress");
            contractAddressProp.stringValue = EditorGUILayout.TextField("Contract address: ", contractAddressProp.stringValue);

            SerializedProperty contractMethodProp = serializedObject.FindProperty("contractMethod");
            contractMethodProp.stringValue = EditorGUILayout.TextField("Method: ", contractMethodProp.stringValue);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            string[] options = new string[] { "Call", "View" };
            SerializedProperty selectedActionProp = serializedObject.FindProperty("selectedAction");
            selectedActionProp.intValue = EditorGUILayout.Popup("Choose action type:", selectedActionProp.intValue, options);

            EditorGUILayout.Space();

            listProperty.arraySize = EditorGUILayout.IntField("Arguments", listProperty.arraySize);

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                var dialogue = listProperty.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(dialogue, new GUIContent("Argument " + (i + 1)), true);
            }

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            if (_target.selectedAction == 0)
            {
                if (!_target.IsComponentDataValid()) GUI.enabled = false;

                if (GUILayout.Button("Call contract"))
                {
                    GUI.enabled = true;
                    CallAndWaitForResult();
                }
            }
            else if (_target.selectedAction == 1)
            {
                if (GUILayout.Button("View contract"))
                {
                    GUI.enabled = true;
                    ViewAndWaitForResult();
                }

            }
            GUI.enabled = true;

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (_target.selectedAction == 0)
            {
                DrawExtraSettings();
            }
        }
        else
        {
            EditorGUILayout.Space();

            if (_target.selectedRole == 0)
            {
                GUILayout.Label("You should connect player account first");
                GUILayout.Label("Drag the 'Player Connect' component somewhere in your scene and press connect.", EditorStyles.miniLabel);
            }
            else if (_target.selectedRole == 1)
            {
                GUILayout.Label("You should connect developer account first");
                GUILayout.Label("Open Near > Developer Account and press connect", EditorStyles.miniLabel);
            }

        }

        serializedObject.ApplyModifiedProperties();
    }

    public async void CallAndWaitForResult()
    {
        Debug.Log("Transaction is pending");
        dynamic result = await _target.CallContract();
        Debug.Log(JsonConvert.SerializeObject(result));
    }

    public async void ViewAndWaitForResult()
    {
        Debug.Log("Transaction is pending");
        dynamic result = await _target.ViewContract();
        Debug.Log(JsonConvert.SerializeObject(result));
    }


    private bool IsSelectedRoleConnected()
    {
        string[] options = new string[] { "Player", "Developer" };

        SerializedProperty selectedRoleProp = serializedObject.FindProperty("selectedRole");
        selectedRoleProp.intValue = EditorGUILayout.Popup("Choose your role:", selectedRoleProp.intValue, options);

        if (selectedRoleProp.intValue == 0) _target.actor = EConnectionActor.Player;
        else if (selectedRoleProp.intValue == 1) _target.actor = EConnectionActor.Developer;

        return ConnectionsManager.IsConnected(_target.actor);
    }

    void DrawExtraSettings()
    {
        _target.showExtraSettings = EditorGUILayout.Toggle("Settings", _target.showExtraSettings);
        if (_target.showExtraSettings)
        {
            SerializedProperty gasProp = serializedObject.FindProperty("gas");
            gasProp.stringValue = EditorGUILayout.TextField("TGas: ", gasProp.stringValue);

            SerializedProperty depositProp = serializedObject.FindProperty("deposit");
            depositProp.stringValue = EditorGUILayout.TextField("Deposit: ", depositProp.stringValue);
        }
    }
}
