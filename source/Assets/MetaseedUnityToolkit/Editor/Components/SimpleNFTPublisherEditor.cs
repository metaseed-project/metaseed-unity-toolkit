using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Linq;
using System;
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
        bool isSelectedRoleConnected = IsSelectedRoleConnected();
        if (isSelectedRoleConnected)
        {

            EditorGUILayout.Space();

            _target.contractAddress = EditorGUILayout.TextField("Contract address: ", _target.contractAddress);

            _target.title = EditorGUILayout.TextField("Title: ", _target.title);

            _target.description = EditorGUILayout.TextField("Descrition: ", _target.description);

            _target.tokenId = EditorGUILayout.TextField("TokenId: ", _target.tokenId);

            _target.media = EditorGUILayout.TextField("Media: ", _target.media);

            EditorGUILayout.Space();

            _target.receiverId = EditorGUILayout.TextField("Receiver Id: ", _target.receiverId);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (!_target.IsNFTDataValid()) GUI.enabled = false;

            if (GUILayout.Button("Mint NFT"))
            {
                GUI.enabled = true;
                _target.MintNftWithParameters(_target.contractAddress, _target.title, _target.description, _target.media, _target.receiverId, _target.actor);
            }
            GUI.enabled = true;

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            DrawExtraSettings();
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
