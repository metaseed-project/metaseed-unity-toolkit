using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace MetaseedUnityToolkit
{
    public class PlayerConnector : MonoBehaviour
    {
        public async Task ConnectWalletByBrowserAsync()
        {
            await WalletConnector.Connect("testnet", EConnectionActor.Player);
        }

        public void ConnectWalletByBrowser()
        {
            WalletConnector.Connect("testnet", EConnectionActor.Player);
        }

        public void DisconnectWallet()
        {
            ConnectionsManager.Disconnect(EConnectionActor.Player);
        }

        public bool IsPlayerConnected()
        {
            return ConnectionsManager.IsConnected(EConnectionActor.Player);
        }

        public string GetPlayerAccountId()
        {
            return PluginStorage.PlayerNearAccountId;
        }
    }
}