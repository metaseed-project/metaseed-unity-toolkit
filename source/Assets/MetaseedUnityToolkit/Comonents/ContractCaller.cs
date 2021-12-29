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
        public string contractAddress = "testcounter.metaseed.testnet";

        public string contractMethod = "incrementCounter";

        public double gas = 10.0;
        public double deposit = 0;

        public EConnectionActor actor;

        //--- Editor Settings

        public ulong nearGas;
        public UInt128 yoctoNearDeposit;
        public bool showExtraSettings = false;
        public int selectedRole = 0;
        public int selectedAction = 0;

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
            return await connection.CallMethod(_contractAddress, _contractMethod, ConstructArguments(_arguments), _gas, _deposit);
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
            return await connection.ViewMethod(_contractAddress, _contractMethod, ConstructArguments(_arguments));
        }

        public dynamic ConstructArguments(List<ContractArgument> _arguments)
        {
            dynamic args = new ExpandoObject();

            foreach (ContractArgument a in _arguments)
            {
                if (a.type == "i32") ((IDictionary<String, object>)args)[a.name] = Int32.Parse(a.value);
                else if (a.type == "i64") ((IDictionary<String, object>)args)[a.name] = Int64.Parse(a.value);
                else if (a.type == "string") ((IDictionary<String, object>)args)[a.name] = a.value.ToString();
            }
            return args;
        }
    }



    [System.Serializable]
    public class ContractArgument
    {
        public string name = "";
        public string value;

        public string type = "i32";
    }
}