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

        public string gas = "10.0";
        public string deposit = "0.1";

        public EConnectionActor actor;


        //--- Editor Settings

        public ulong nearGas;
        public UInt128 yoctoNearDeposit;
        public bool showExtraSettings = false;
        public int selectedRole = 0;
        public int selectedAction = 0;

        public async Task<dynamic> MintNft(string _contractAddress, string _tokenId, string _title, string _description, string _media, string _receiverId, EConnectionActor _actor, ulong? _gas = null, Nullable<UInt128> _deposit = null)
        {
            if (!IsNFTDataValid(_title, _media, _receiverId))
            {
                throw new Exception("Warning: Nft metadata is not valid, request will not be send.");
            }

            if (!ConnectionsManager.IsConnected(EConnectionActor.Player))
            {
                throw new Exception("Warning: Your near account is not connected.");
            }

            Connection connection = ConnectionsManager.GetConnectionInstance(_actor);

            dynamic args = new ExpandoObject();

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

        public bool IsNFTDataValid(string _title, string _media, string _receiverId)
        {
            if (_title == "") return false;
            if (_media == "") return false;
            if (_receiverId == "") return false;

            return true;
        }

        public bool IsComponentDataValid()
        {
            double _gas;
            if (!Double.TryParse(gas, out _gas)) return false;

            double _deposit;
            if (!Double.TryParse(deposit, out _deposit)) return false;

            return true;
        }

        public async Task<dynamic> MintNft()
        {
            if (!IsComponentDataValid())
            {
                throw new Exception("Warning: Component data is not valid. Change values or use CallContract(string _contractAddress, string _contractMethod, List<ContractArgument> _arguments, EConnectionActor _actor, ulong? _gas = null, Nullable<UInt128> _deposit = null) instead");
            }

            ulong nearGas = (ulong)UnitConverter.GetGasFormat(Convert.ToDouble(gas));
            UInt128 yoctoNearDeposit = (UInt128)UnitConverter.GetYoctoNearFormat(Convert.ToDouble(deposit));
            return await MintNft(contractAddress, tokenId, title, description, media, receiverId, actor, nearGas, yoctoNearDeposit);
        }
    }
}