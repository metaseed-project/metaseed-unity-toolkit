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

    public PlayerConnector playerConnector;
    public SimpleNFTPublisher simpleNFTPublisher;
    public ContractCaller contractCaller;
    public NearSender nearSender;

    public Button connectButton;
    public Button sendButton;
    public Button mintButton;
    public Button callButton;
    

    void Start()
    {
        // Finding Action Components if they are not drag'n'dropped on the script
        if (playerConnector == null) playerConnector = GameObject.Find("Actions/ConnectPlayer").GetComponent<PlayerConnector>();
        if (simpleNFTPublisher == null) simpleNFTPublisher = GameObject.Find("Actions/MintNFT").GetComponent<SimpleNFTPublisher>();
        if (contractCaller == null) contractCaller = GameObject.Find("Actions/CallContract").GetComponent<ContractCaller>();
        if (nearSender == null) nearSender = GameObject.Find("Actions/SendNear").GetComponent<NearSender>();

        Debug.Log($@"ConnectPlayer is found ({playerConnector != null})
                    MintNFT is found ({simpleNFTPublisher != null})
                    CallContract is found ({contractCaller != null})
                    SendNear is found ({nearSender != null})");


        connectButton = gameObject.GetComponent<Button>();
        connectButton.onClick.AddListener(OnConnectPlayer);

        sendButton = gameObject.GetComponent<Button>();
        sendButton.onClick.AddListener(OnSendNear);

        mintButton = gameObject.GetComponent<Button>();
        mintButton.onClick.AddListener(OnMintNFT);

        callButton = gameObject.GetComponent<Button>();
        callButton.onClick.AddListener(OnContractCall);
    }

    async void OnConnectPlayer()
    {
        await playerConnector.ConnectWalletByBrowser(EConnectionActor.Player);
        
        FetchAccountId();
        if (accountId != null)
            Debug.Log(accountId + " connected as a player successfully");
        else
            Debug.LogError("En error occured while connecting to " + accountId);
    }

    void FetchAccountId(){
        accountId = MetaseedUnityToolkit.PluginStorage.PlayerNearAccountId;
    }

    async void OnSendNear()
    {
        FetchAccountId();
        Debug.Log(accountId + " is sending NEAR");

        dynamic result = await nearSender.SendNear(accountId, UnitConverter.GetYoctoNearFormat(6.1), EConnectionActor.Player);
        Debug.Log("Blockchain has returned the result of sending near: " + JsonConvert.SerializeObject(result));
    }

    async void OnMintNFT()
    {
        FetchAccountId();
        Debug.Log(accountId + " is minting an NFT");

        string name = "Metaseed NFT";
        string description = "Welcome to Metaseed ecosystem!";
        string media = "https://gateway.ipfs.io/ipfs/QmcniBv7UQ4gGPQQW2BwbD4ZZHzN3o3tPuNLZCbBchd1zh";

        dynamic result = await simpleNFTPublisher.MintNftWithParameters("example-nft.testnet", name, description, media, accountId, EConnectionActor.Player);
        Debug.Log("Blockchain has returned the result of NFT minting: " + JsonConvert.SerializeObject(result));
    }

    async void OnContractCall()
    {   
        FetchAccountId();
        Debug.Log(accountId + " is calling a contract");

        // To call an example contract we
        // Need to provide the following arguments
        // {
        //     "receiver_id": "foo.testnet",
        //     "amount": "9999"
        // }

        // In C# we create them using List<ContractArgument>
        List<ContractArgument> arguments = new List<ContractArgument>();

        arguments.Add( new ContractArgument() { name = "receiver_id", value = accountId, type = "string"} );
        arguments.Add( new ContractArgument() { name = "amount", value = "6100", type = "string"} );

        dynamic result = await contractCaller.CallContractWithParameters("ft.examples.testnet", "ft_mint", arguments, EConnectionActor.Player);
        Debug.Log("Blockchain has returned the result of contract calling: " + JsonConvert.SerializeObject(result));
    }
}