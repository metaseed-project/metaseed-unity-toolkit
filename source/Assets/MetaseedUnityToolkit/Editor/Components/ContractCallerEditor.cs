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
        bool isSelectedRoleConnected = IsSelectedRoleConnected();
        if (isSelectedRoleConnected)
        {
            EditorGUILayout.Space();

            _target.contractAddress = EditorGUILayout.TextField("Contract address: ", _target.contractAddress);

            _target.contractMethod = EditorGUILayout.TextField("Method: ", _target.contractMethod);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            string[] options = new string[] { "Call", "View" };
            _target.selectedAction = EditorGUILayout.Popup("Choose action type:", _target.selectedAction, options);

            EditorGUILayout.Space();

            listProperty.arraySize = EditorGUILayout.IntField("Arguments", listProperty.arraySize);

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                var dialogue = listProperty.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(dialogue, new GUIContent("Argument "), true);
            }

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            if (!_target.IsCallDataValid()) GUI.enabled = false;

            if (_target.selectedAction == 0)
            {
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
    }

    public async void CallAndWaitForResult()
    {
        Debug.Log("Transaction is pending");
        dynamic result = await _target.CallContractWithParameters(_target.contractAddress, _target.contractMethod, _target.arguments, _target.actor, _target.nearGas, _target.yoctoNearDeposit);
        Debug.Log(JsonConvert.SerializeObject(result));
    }

    public async void ViewAndWaitForResult()
    {
        Debug.Log("Transaction is pending");
        dynamic result = await _target.ViewContractWithParameters(_target.contractAddress, _target.contractMethod, _target.arguments, _target.actor);
        Debug.Log(JsonConvert.SerializeObject(result));
    }


    private bool IsSelectedRoleConnected()
    {
        string[] options = new string[] { "Player", "Developer" };
        _target.selectedRole = EditorGUILayout.Popup("Choose your role:", _target.selectedRole, options);
        if (_target.selectedRole == 0) _target.actor = EConnectionActor.Player;
        else if (_target.selectedRole == 1) _target.actor = EConnectionActor.Developer;

        return ConnectionsManager.IsConnected(_target.actor);
    }

    void DrawExtraSettings()
    {
        _target.showExtraSettings = EditorGUILayout.Toggle("Settings", _target.showExtraSettings);
        if (_target.showExtraSettings)
        {
            _target.gas = Convert.ToDouble(EditorGUILayout.TextField("TGas: ", _target.gas.ToString()));
            _target.nearGas = (ulong)UnitConverter.GetGasFormat(_target.gas);

            _target.deposit = Convert.ToDouble(EditorGUILayout.TextField("Deposit: ", _target.deposit.ToString()));
            _target.yoctoNearDeposit = (UInt128)UnitConverter.GetYoctoNearFormat(_target.deposit);
        }
    }
}
