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
    public class ContractCaller : MonoBehaviour
    {
        [SerializeField]
        public List<ContractArgument> arguments = new List<ContractArgument>();

        [System.NonSerialized]
        public string contractAddress;

        public string contractMethod;

        public ulong? gas;
        public Nullable<UInt128> deposit;

        public EConnectionActor actor;

        public bool IsCallDataValid()
        {
            if (contractAddress == "") return false;
            if (contractMethod == "") return false;

            foreach (ContractArgument p in arguments)
            {
                if (p.name == "") return false;
                if (p.value == "") return false;
            }
            return true;
        }

        public async Task<dynamic> CallContractWithParameters(string _contractAddress, string _contractMethod, List<ContractArgument> _arguments, EConnectionActor _actor, ulong? _gas = null, Nullable<UInt128> _deposit = null)
        {
            if (!IsCallDataValid())
            {
                Debug.LogError("Warning: Call metadata is not valid, request will not be send.");
                return new ExpandoObject();
            }

            if (!ConnectionsManager.IsConnected(_actor))
            {
                Debug.LogError("Warning: Your near account is not connected.");
                return new ExpandoObject();
            }

            Connection connection = ConnectionsManager.GetConnectionInstance(_actor);

            dynamic args = new ExpandoObject();

            foreach (ContractArgument a in _arguments)
            {
                args[a.name] = a.value;
            }

            //TODO: gas calculation up to Anton

            return await connection.CallMethod(_contractAddress, _contractMethod, args, _gas, _deposit);
        }

        public async Task<dynamic> ViewContractWithParameters(string _contractAddress, string _contractMethod, List<ContractArgument> _arguments, EConnectionActor _actor)
        {
            if (!IsCallDataValid())
            {
                Debug.LogError("Warning: Call metadata is not valid, request will not be send.");
                return new ExpandoObject();
            }

            if (!ConnectionsManager.IsConnected(_actor))
            {
                Debug.LogError("Warning: Your near account is not connected.");
                return new ExpandoObject();
            }

            Connection connection = ConnectionsManager.GetConnectionInstance(_actor);

            dynamic args = new ExpandoObject();

            foreach (ContractArgument a in _arguments)
            {
                args[a.name] = a.value;
            }

            //TODO: gas calculation up to Anton

            return await connection.ViewMethod(_contractAddress, _contractMethod, args);
        } 

        public static UInt128 GetNearFormat(double amount)
        {
            UInt128 p = new UInt128(amount * 1000000000);
            UInt128.Create(out var lp, 1000000000000000);
            var res = p * lp;
            return res;
        }
    }

    [System.Serializable]
    public class ContractArgument
    {
        public string name = "";
        public string value;
    }
}