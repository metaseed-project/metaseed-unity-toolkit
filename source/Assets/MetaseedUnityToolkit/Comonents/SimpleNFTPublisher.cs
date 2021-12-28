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
        public string contractAddress;

        public string title;
        public string description;
        public string media;
        public string tokenId;
        public string receiverId;

        public ulong? gas;
        public Nullable<UInt128> deposit;

        public EConnectionActor actor;

        public bool IsNFTDataValid()
        {
            if (title == "") return false;

            if (media == "") return false;

            //TODO: check if valid address
            if (receiverId == "") return false;

            return true;
        }

        public async Task<dynamic> MintNftWithParameters(string _contractAddress, string _title, string _description, string _media, string _receiverId, EConnectionActor _actor, ulong? _gas = null, Nullable<UInt128> _deposit = null)
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
            args.token_id = tokenId;
            args.title = _title;
            args.receiver_id = _receiverId;

            args.token_metadata = new ExpandoObject();
            args.token_metadata.title = _title;
            args.token_metadata.description = _description;
            args.token_metadata.media = _media;
            args.token_metadata.copies = 1;

            _gas = _gas ?? UnitConverter.GetGasFormat(15.0);
            _deposit = _deposit ?? UnitConverter.GetYoctoNearFormat(0.1);

            //TODO: calculate gas dynamically 300000000000000;
            return await connection.CallMethod(_contractAddress, "nft_mint", args, _gas, _deposit);
        }
    }
}