using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MetaseedUnityToolkit;

public class Login : MonoBehaviour
{
    PlayerConnector pc = new PlayerConnector();
    void Start()
    {
        Button loginButton = gameObject.GetComponent<Button>();
        loginButton.onClick.AddListener(OnClick);
    }

    async void OnClick()
    {
        await pc.ConnectWalletByBrowser(EConnectionActor.Player);
    }
}
