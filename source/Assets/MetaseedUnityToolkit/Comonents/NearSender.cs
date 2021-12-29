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

        public double deposit = 0f;

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
                Debug.LogError("Warning: Transaction metadata is not valid, request will not be send.");
                return new ExpandoObject();
            }

            if (!ConnectionsManager.IsConnected(_actor))
            {
                Debug.LogError("Warning: Your near account is not connected.");
                return new ExpandoObject();
            }

            Connection connection = ConnectionsManager.GetConnectionInstance(_actor);
            return await connection.SendMoney(_receiverId, _deposit);
        }
    }
}