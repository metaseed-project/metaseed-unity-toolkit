using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using NearClient.Utilities;
using MetaseedUnityToolkit;

public class DemoControllerScript : MonoBehaviour
{
    private string accountId;
    private EConnectionActor actor = EConnectionActor.Player;

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

        dynamic result = await nearSender.SendNear(accountId, (UInt128)UnitConverter.GetYoctoNearFormat(0.01), actor);
        Debug.Log("Blockchain has returned the result of sending near: " + JsonConvert.SerializeObject(result));
    }

    async void OnMintNFT()
    {
        Debug.Log(accountId + " is minting an NFT");

        string name = "Metaseed NFT";
        string tokenId = "88";
        string description = "Welcome to Metaseed ecosystem!";
        string media = "https://gateway.ipfs.io/ipfs/QmcniBv7UQ4gGPQQW2BwbD4ZZHzN3o3tPuNLZCbBchd1zh";
        ulong nearGas = (ulong)UnitConverter.GetGasFormat(30);

        dynamic result = await simpleNFTPublisher.MintNftWithParameters("example-nft.testnet", tokenId, name, description, media, accountId, actor, nearGas);
        Debug.Log("Blockchain has returned the result of NFT minting: " + JsonConvert.SerializeObject(result));
    }

    async void OnContractCall()
    {
        Debug.Log(accountId + " is calling a contract");

        // To call an example contract we
        // Need to provide the following arguments
        // {
        //     "value": "4",
        // }

        // In C# we create them using List<ContractArgument>
        List<ContractArgument> arguments = new List<ContractArgument>();
        arguments.Add(new ContractArgument() { name = "value", value = "4", type = "i32" });

        ulong nearGas = (ulong)UnitConverter.GetGasFormat(10);
        UInt128 yoctoNearDeposit = (UInt128)UnitConverter.GetYoctoNearFormat(0);

        dynamic result = await contractCaller.CallContractWithParameters("testcounter.metaseed.testnet", "incrementCounter", arguments, actor, nearGas, yoctoNearDeposit);
        Debug.Log("Blockchain has returned the result of contract calling: " + JsonConvert.SerializeObject(result));
    }

    async void OnContractView()
    {
        Debug.Log(accountId + " is calling a contract");

        // To view an example contract we don't need an arguments, so the list is empty
        List<ContractArgument> arguments = new List<ContractArgument>();

        ulong nearGas = (ulong)UnitConverter.GetGasFormat(10);
        UInt128 yoctoNearDeposit = (UInt128)UnitConverter.GetYoctoNearFormat(0);

        dynamic result = await contractCaller.CallContractWithParameters("testcounter.metaseed.testnet", "getCounter", arguments, actor, nearGas, yoctoNearDeposit);
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
