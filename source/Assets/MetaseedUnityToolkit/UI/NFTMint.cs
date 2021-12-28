using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using MetaseedUnityToolkit;

public class NFTMint : MonoBehaviour
{
    SimpleNFTPublisher simpleNFTPublisher = new SimpleNFTPublisher();
    void Start()
    {
        Button mintButton = gameObject.GetComponent<Button>();
        mintButton.onClick.AddListener(OnClick);
    }

    async void OnClick()
    {
        string accountId = PluginStorage.PlayerNearAccountId;
        Debug.LogError("NFT Mint" + accountId);
        dynamic result = await simpleNFTPublisher.MintNftWithParameters("tonyraven.testnet", "Test", "Test description", "https://bafybeiezwejfrmigg2mefe5egfysnbrjr4iqstgleh7nwgh4vjgdpqe7oe.ipfs.dweb.link/", accountId, EConnectionActor.Player);
        Debug.LogError(JsonConvert.SerializeObject(result));
    }
}
