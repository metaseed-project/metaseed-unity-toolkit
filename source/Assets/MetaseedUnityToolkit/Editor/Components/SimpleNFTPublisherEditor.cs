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
    }

    public async void MintAndWaitForResult()
    {
        Debug.Log("Transaction is pending");
        dynamic result = await _target.MintNftWithParameters(_target.contractAddress, _target.tokenId, _target.title, _target.description, _target.media, _target.receiverId, _target.actor, _target.nearGas, _target.yoctoNearDeposit);
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
