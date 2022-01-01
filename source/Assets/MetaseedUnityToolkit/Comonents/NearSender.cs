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
    public class NearSender : MonoBehaviour
    {
        [System.NonSerialized]
        public string receiverId = "metaseed.testnet";

        public string deposit = "0";

        public EConnectionActor actor;

        public bool IsCallDataValid(String _receiverId, Nullable<UInt128> _deposit)
        {
            if (_receiverId == "") return false;
            if (_deposit <= 0) return false;

            return true;
        }

        public async Task<dynamic> SendNear(String _receiverId, UInt128 _deposit, EConnectionActor _actor)
        {
            if (!IsCallDataValid(_receiverId, _deposit))
            {
                throw new Exception("Warning: Transaction metadata is not valid, request will not be send.");
            }

            if (!ConnectionsManager.IsConnected(_actor))
            {
                throw new Exception("Warning: Your near account is not connected.");
            }

            Connection connection = ConnectionsManager.GetConnectionInstance(_actor);
            return await connection.SendMoney(_receiverId, _deposit);
        }

        public bool IsComponentDataValid()
        {
            double _deposit;
            if (!Double.TryParse(deposit, out _deposit)) return false;
            return true;
        }

        public async Task<dynamic> SendNear()
        {
            if (!IsComponentDataValid())
            {
                throw new Exception("Warning: Component data is not valid. Change values or use SendNear(String _receiverId, UInt128 _deposit, EConnectionActor _actor) instead");
            }

            UInt128 yoctoNearDeposit = (UInt128)UnitConverter.GetYoctoNearFormat(Convert.ToDouble(deposit));
            return await SendNear(receiverId, yoctoNearDeposit, actor);
        }
    }
}