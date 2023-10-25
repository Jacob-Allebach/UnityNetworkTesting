using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldManager : MonoBehaviour
    {
        private static string joinCode = "";
        [SerializeField]
        private GameObject testRelay;

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons(testRelay);
            }
            else
            {
                StatusLabels();

                //SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        static void StartButtons(GameObject testRelay)
        {
            if (GUILayout.Button("Host"))
            {
                testRelay.GetComponent<TestRelay>().CreateRelay(true);
                ChangeJoinCode(testRelay.GetComponent<TestRelay>().GetJoinCode());
                //NetworkManager.Singleton.StartHost();
            }
            if (GUILayout.Button("Client"))
            {
                testRelay.GetComponent<TestRelay>().JoinRelay(joinCode);
                //NetworkManager.Singleton.StartClient();
            }
            if (GUILayout.Button("Server"))
            {
                testRelay.GetComponent<TestRelay>().CreateRelay(false);
                ChangeJoinCode(testRelay.GetComponent<TestRelay>().GetJoinCode());
                //NetworkManager.Singleton.StartServer();
            }
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
            GUILayout.Label("Join Code: " + joinCode);
        }

        public static void ChangeJoinCode(string newCode)
        {
            joinCode = newCode;
        }
    }
}