using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using NearClient.Utilities;
using MetaseedUnityToolkit;

public class SendNear : MonoBehaviour
{
    NearSender ns = new NearSender();
    void Start()
    {
        Button sendButton = gameObject.GetComponent<Button>();
        sendButton.onClick.AddListener(OnClick);
    }

    async void OnClick()
    {
        dynamic result = await ns.SendNear("metaseed.testnet", (UInt128)UnitConverter.GetYoctoNearFormat(6.1), EConnectionActor.Player);
        Debug.LogError(JsonConvert.SerializeObject(result));
    }
}
