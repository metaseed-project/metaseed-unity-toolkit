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

[CustomEditor(typeof(SimpleNFTPublisher))]
public class SimpleNFTPublisherEditor : Editor
{
    private SimpleNFTPublisher _target;

    void OnEnable()
    {
        _target = ((SimpleNFTPublisher)target);
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

            SerializedProperty titleProp = serializedObject.FindProperty("title");
            titleProp.stringValue = EditorGUILayout.TextField("Title: ", titleProp.stringValue);

            SerializedProperty descriptionProp = serializedObject.FindProperty("description");
            descriptionProp.stringValue = EditorGUILayout.TextField("Description: ", descriptionProp.stringValue);

            SerializedProperty tokenIdProp = serializedObject.FindProperty("tokenId");
            tokenIdProp.stringValue = EditorGUILayout.TextField("TokenId: ", tokenIdProp.stringValue);

            SerializedProperty mediaProp = serializedObject.FindProperty("media");
            mediaProp.stringValue = EditorGUILayout.TextField("Media: ", mediaProp.stringValue);

            EditorGUILayout.Space();

            SerializedProperty receiverIdProp = serializedObject.FindProperty("receiverId");
            receiverIdProp.stringValue = EditorGUILayout.TextField("Receiver Id: ", receiverIdProp.stringValue);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (!_target.IsComponentDataValid()) GUI.enabled = false;

            if (GUILayout.Button("Mint NFT"))
            {
                GUI.enabled = true;
                MintAndWaitForResult();
            }
            GUI.enabled = true;

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            DrawExtraSettings();
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

    public async void MintAndWaitForResult()
    {
        Debug.Log("Transaction is pending");
        dynamic result = await _target.MintNft();
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
