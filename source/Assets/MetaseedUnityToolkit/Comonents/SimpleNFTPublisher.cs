using System;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using NearClient.Utilities;

namespace MetaseedUnityToolkit
{
    public class SimpleNFTPublisher : MonoBehaviour
    {
        [System.NonSerialized]
        public string contractAddress = "example-nft.testnet";

        public string title;
        public string description;
        public string media;
        public string tokenId = "0";
        public string receiverId;

        public double gas = 10.0;
        public double deposit = 0.1;

        public EConnectionActor actor;


        //--- Editor Settings

        public ulong nearGas;
        public UInt128 yoctoNearDeposit;
        public bool showExtraSettings = false;
        public int selectedRole = 0;
        public int selectedAction = 0;

        public bool IsNFTDataValid()
        {
            if (title == "") return false;

            if (media == "") return false;

            //TODO: check if valid address
            if (receiverId == "") return false;

            return true;
        }

        public async Task<dynamic> MintNftWithParameters(string _contractAddress, string _tokenId, string _title, string _description, string _media, string _receiverId, EConnectionActor _actor, ulong? _gas = null, Nullable<UInt128> _deposit = null)
        {
            if (!IsNFTDataValid())
            {
                Debug.LogError("Warning: Nft metadata is not valid, request will not be send.");
                return new ExpandoObject();
            }

            if (!ConnectionsManager.IsConnected(EConnectionActor.Player))
            {
                Debug.LogError("Warning: Your near account is not connected.");
                return new ExpandoObject();
            }

            Connection connection = ConnectionsManager.GetConnectionInstance(_actor);

            dynamic args = new ExpandoObject();

            //TODO: get token_id dynamically
            args.token_id = _tokenId;
            args.title = _title;
            args.receiver_id = _receiverId;

            args.token_metadata = new ExpandoObject();
            args.token_metadata.title = _title;
            args.token_metadata.description = _description;
            args.token_metadata.media = _media;
            args.token_metadata.copies = 1;

            _gas = _gas ?? UnitConverter.GetGasFormat(15.0);
            _deposit = _deposit ?? UnitConverter.GetYoctoNearFormat(0.1);

            return await connection.CallMethod(_contractAddress, "nft_mint", args, _gas, _deposit);
        }
    }
}