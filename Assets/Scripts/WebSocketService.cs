using UnityEngine;
using NativeWebSocket;
using System.Diagnostics;

namespace Assets.Scripts
{
    public class WebSocketService : MonoBehaviour
    {
        public static WebSocketService instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        //create a notification system

        //these are the commands that will be communicating with the AWS server
        public const string RequestStartOp = "1";
        public const string PlayingOp = "11";

        public const string ShootOp = "5";
        public const string ReloadOp = "7";
        public const string HoldOp = "9";

        public const string YouDiedOp = "44";
        public const string YouWonOp = "88";
        public const string TieOp = "66";

        private bool intentionalClose = false;
        private WebSocket _websocket;
        public MultiplayerBattleSystem mpBattleSystem;
        private string current_uuid;
        private string _webSocketDns = "wss://awgez7g3yg.execute-api.us-east-1.amazonaws.com/demo";

        // Establishes the connection's lifecycle callbacks.
        private void SetupWebsocketCallbacks()
        {
            _websocket.OnOpen += () =>
            {
                Debug.Log("Connection open!");
                intentionalClose = false;
                EventMessage startRequest = new EventMessage("OnMessage", RequestStartOp);
                current_uuid = startRequest.uuid;
                SendWebSocketMessage(JsonUtility.ToJson(startRequest));
            };

            _websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };

            _websocket.OnClose += (e) =>
            {
                Debug.Log("Connection closed!");

                // only do this if someone quit the game session, and not for a game ending event
                if (!intentionalClose)
                {
                    Debug.Log("Disconnected.");
                }
            };

            _websocket.OnMessage += (bytes) =>
            {
                Debug.Log("OnMessage!");
                string message = System.Text.Encoding.UTF8.GetString(bytes);
                Debug.Log(message.ToString());

                ProcessReceivedMessage(message);
            };
        }

        // Connects to the websocket
        async public void FindMatch()
        {
            // waiting for messages
            await _websocket.Connect();
        }

        private void ProcessReceivedMessage(string message)
        {
            //Debug.Log(message);

            EventMessage gameMessage = JsonUtility.FromJson<EventMessage>(message);
            // Debug.Log(JsonUtility.ToJson(gameMessage, true));
            Debug.Log(gameMessage.uuid);

            if (gameMessage.opcode == PlayingOp)
            {
                Debug.Log("Playing!");
                //change game text to indicate player found
                mpBattleSystem.websocketReady = true;
            }
            else if (gameMessage.opcode == ShootOp && gameMessage.uuid != current_uuid)
            {
                Debug.Log(gameMessage.message);
                mpBattleSystem.updateEnemyAction(BattleAction.SHOOT);
            }
            else if (gameMessage.opcode == ReloadOp && gameMessage.uuid != current_uuid)
            {
                Debug.Log(gameMessage.message);
                mpBattleSystem.updateEnemyAction(BattleAction.RELOAD);
            }
            else if (gameMessage.opcode == HoldOp && gameMessage.uuid != current_uuid)
            {
                Debug.Log(gameMessage.message);
                mpBattleSystem.updateEnemyAction(BattleAction.HOLD);
            }

            else if (gameMessage.opcode == YouWonOp && gameMessage.uuid != current_uuid)
            {
                Debug.Log("You won!");
                //change game text to indicate client won
                QuitGame();
            }
            else if (gameMessage.opcode == YouDiedOp && gameMessage.uuid != current_uuid)
            {
                Debug.Log("You lost!");
                //change game text to indicate client lost
                QuitGame();
            }
            else if (gameMessage.opcode == TieOp && gameMessage.uuid != current_uuid)
            {
                Debug.Log("You tied!");
                //change game text to indicate client lost
                QuitGame();
            }
        }

        public async void SendWebSocketMessage(string message)
        {
            if (_websocket.State == WebSocketState.Open)
            {
                // Sending plain text
                await _websocket.SendText(message);
            }
        }

        public void Hold()
        {
            EventMessage holdMessage = new EventMessage("OnMessage", HoldOp);
            SendWebSocketMessage(JsonUtility.ToJson(holdMessage));
        }

        public async void QuitGame()
        {
            intentionalClose = true;
            //change game message to indicate match ended
            await _websocket.Close();
            Destroy(this);
        }

        private async void OnApplicationQuit()
        {
            await _websocket.Close();
        }

        void Start()
        {
            Debug.Log("Websocket start");
            intentionalClose = false;

            _websocket = new WebSocket(_webSocketDns);
            Debug.Log("1");
            SetupWebsocketCallbacks();
            Debug.Log("2");
            FindMatch();
        }

        void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            _websocket.DispatchMessageQueue();
#endif
        }

        public void init() { }

        protected WebSocketService() { }
    }
}
