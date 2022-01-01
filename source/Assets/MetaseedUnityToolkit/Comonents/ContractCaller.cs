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

        public string gas = "10.0";
        public string deposit = "0";
        public EConnectionActor actor;

        //--- Editor Settings

        public ulong nearGas;
        public UInt128 yoctoNearDeposit;
        public bool showExtraSettings = false;
        public int selectedRole = 0;
        public int selectedAction = 0;

        public async Task<dynamic> CallContract(string _contractAddress, string _contractMethod, ExpandoObject _arguments, EConnectionActor _actor, ulong? _gas = null, Nullable<UInt128> _deposit = null)
        {
            if (!IsCallDataValid(_contractAddress, _contractMethod))
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
            return await connection.CallMethod(_contractAddress, _contractMethod, _arguments, _gas, _deposit);
        }

        public bool IsCallDataValid(string _contractAddress, string _contractMethod)
        {
            if (_contractAddress == "") return false;
            if (_contractMethod == "") return false;

            return true;
        }

        public bool IsComponentDataValid()
        {
            double _gas;
            if (!Double.TryParse(gas, out _gas)) return false;

            double _deposit;
            if (!Double.TryParse(deposit, out _deposit)) return false;

            foreach (ContractArgument p in arguments)
            {
                if (p.name == "") return false;
                if (p.value == "") return false;
                // check for type
            }
            return true;
        }

        public async Task<dynamic> CallContract()
        {
            if (!IsComponentDataValid())
            {
                throw new Exception("Warning: Component data is not valid. Change values or use CallContract(string _contractAddress, string _contractMethod, List<ContractArgument> _arguments, EConnectionActor _actor, ulong? _gas = null, Nullable<UInt128> _deposit = null) instead");
            }

            ulong nearGas = (ulong)UnitConverter.GetGasFormat(Convert.ToDouble(gas));
            UInt128 yoctoNearDeposit = (UInt128)UnitConverter.GetYoctoNearFormat(Convert.ToDouble(deposit));
            return await CallContract(contractAddress, contractMethod, ConstructArguments(arguments), actor, nearGas, yoctoNearDeposit);
        }

        public async Task<dynamic> ViewContract(string _contractAddress, string _contractMethod, ExpandoObject _arguments, EConnectionActor _actor)
        {
            if (!IsViewDataValid(_contractAddress, _contractMethod))
            {
                throw new Exception("Warning: Call metadata is not valid, request will not be send.");
            }

            if (!ConnectionsManager.IsConnected(_actor))
            {
                throw new Exception("Warning: Your near account is not connected.");
            }

            Connection connection = ConnectionsManager.GetConnectionInstance(_actor);
            return await connection.ViewMethod(_contractAddress, _contractMethod, _arguments);
        }

        public bool IsViewDataValid(string _contractAddress, string _contractMethod)
        {
            if (_contractAddress == "") return false;
            if (_contractMethod == "") return false;

            return true;
        }

        public async Task<dynamic> ViewContract()
        {
            return await ViewContract(contractAddress, contractMethod, ConstructArguments(arguments), actor);
        }

        public dynamic ConstructArguments(List<ContractArgument> _arguments)
        {
            dynamic args = new ExpandoObject();

            foreach (ContractArgument a in _arguments)
            {
                if (a.type == "i32") ((IDictionary<String, object>)args)[a.name] = Int32.Parse(a.value);
                else if (a.type == "i64") ((IDictionary<String, object>)args)[a.name] = Int64.Parse(a.value);
                else if (a.type == "ui32") ((IDictionary<String, object>)args)[a.name] = UInt32.Parse(a.value);
                else if (a.type == "ui64") ((IDictionary<String, object>)args)[a.name] = UInt64.Parse(a.value);
                else if (a.type == "ui128") ((IDictionary<String, object>)args)[a.name] = UInt128.Parse(a.value);
                else if (a.type == "string") ((IDictionary<String, object>)args)[a.name] = a.value.ToString();
                else ((IDictionary<String, object>)args)[a.name] = a.value.ToString();
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