using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using NearClient.Utilities;
using MetaseedUnityToolkit;

public class DemoControllerScriptVisual : MonoBehaviour
{
    private string accountId;

    public PlayerConnector playerConnector;
    public SimpleNFTPublisher simpleNFTPublisher;
    public ContractCaller contractCaller;
    public NearSender nearSender;

    public Button connectButton;
    public Button sendButton;
    public Button mintButton;
    public Button callButton;
    public Button viewButton;

    void Awake()
    {
        // Finding Action Components if they are not drag'n'dropped on the script
        if (playerConnector == null) playerConnector = GameObject.Find("Actions/ConnectPlayer").GetComponent<PlayerConnector>();
        if (simpleNFTPublisher == null) simpleNFTPublisher = GameObject.Find("Actions/MintNFT").GetComponent<SimpleNFTPublisher>();
        if (contractCaller == null) contractCaller = GameObject.Find("Actions/CallContract").GetComponent<ContractCaller>();
        if (nearSender == null) nearSender = GameObject.Find("Actions/SendNear").GetComponent<NearSender>();

        if (playerConnector.IsPlayerConnected()) accountId = playerConnector.GetPlayerAccountId();
        ChangeUI(playerConnector.IsPlayerConnected());
    }
    void Start()
    {
        Debug.Log($@"ConnectPlayer is found ({playerConnector != null}) | MintNFT is found ({simpleNFTPublisher != null}) | CallContract is found ({contractCaller != null}) | SendNear is found ({nearSender != null})");

        connectButton.onClick.AddListener(OnConnectPlayer);
        sendButton.onClick.AddListener(OnSendNear);
        mintButton.onClick.AddListener(OnMintNFT);
        callButton.onClick.AddListener(OnContractCall);
        viewButton.onClick.AddListener(OnContractView);
    }

    async void OnConnectPlayer()
    {
        if (!playerConnector.IsPlayerConnected())
        {
            await playerConnector.ConnectWalletByBrowserAsync();
            accountId = playerConnector.GetPlayerAccountId();
        }
        else
        {
            playerConnector.DisconnectWallet();
            accountId = null;
        }
        ChangeUI(playerConnector.IsPlayerConnected());
    }

    async void OnSendNear()
    {
        Debug.Log(accountId + " is sending NEAR");
        dynamic result = await nearSender.SendNear();
        Debug.Log("Blockchain has returned the result of sending near: " + JsonConvert.SerializeObject(result));
    }

    async void OnMintNFT()
    {
        Debug.Log(accountId + " is minting an NFT");
        dynamic result = await simpleNFTPublisher.MintNft();
        Debug.Log("Blockchain has returned the result of NFT minting: " + JsonConvert.SerializeObject(result));
    }

    async void OnContractCall()
    {
        Debug.Log(accountId + " is calling a contract");
        dynamic result = await contractCaller.CallContract();
        Debug.Log("Blockchain has returned the result of contract calling: " + JsonConvert.SerializeObject(result));
    }

    async void OnContractView()
    {
        Debug.Log(accountId + " is calling a contract");
        dynamic result = await contractCaller.ViewContract();
        Debug.Log("Blockchain has returned the result of contract calling: " + JsonConvert.SerializeObject(result));
    }


    private void ChangeUI(bool connected)
    {
        if (connected)
        {
            connectButton.GetComponentInChildren<Text>().text = "Disconnect: " + accountId;
            sendButton.gameObject.SetActive(true);
            mintButton.gameObject.SetActive(true);
            callButton.gameObject.SetActive(true);
            viewButton.gameObject.SetActive(true);
        }
        else
        {
            connectButton.GetComponentInChildren<Text>().text = "Connect Player";
            sendButton.gameObject.SetActive(false);
            mintButton.gameObject.SetActive(false);
            callButton.gameObject.SetActive(false);
            viewButton.gameObject.SetActive(false);
        }
    }
}
