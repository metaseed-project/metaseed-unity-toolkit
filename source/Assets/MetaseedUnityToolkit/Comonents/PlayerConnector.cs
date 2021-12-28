using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace MetaseedUnityToolkit
{
    public class PlayerConnector : MonoBehaviour
    {
        public async Task ConnectWalletByBrowser(EConnectionActor actor)
        {
            await WalletConnector.Connect("testnet", actor);
        }
    }
}