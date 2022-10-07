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
                //indicate in dialogue text that there was an error
                mpBattleSystem.EndBattle();
            };

            _websocket.OnClose += (e) =>
            {
                Debug.Log("Connection closed!");

                if (!intentionalClose) //if player closes a game session
                {
                    Debug.Log("Disconnected.");
                    //indicate in dialogue text that there was an error
                    mpBattleSystem.EndBattle();
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

        //Connects to the websocket to find match for 2 players
        async public void FindMatch()
        {
            // waiting for messages
            await _websocket.Connect();
        }

        private void ProcessReceivedMessage(string message)
        {
            //Debug.Log(message);

            EventMessage gameMessage = JsonUtility.FromJson<EventMessage>(message);
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

            else if (gameMessage.opcode == YouWonOp)
            {
                Debug.Log("You won!");
                //change game text to indicate client won
                QuitGame();
            }
            else if (gameMessage.opcode == YouDiedOp)
            {
                Debug.Log("You lost!");
                //change game text to indicate client lost
                QuitGame();
            }
            else if (gameMessage.opcode == TieOp)
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
                await _websocket.SendText(message); //sends message to websocket for OnMessage to process
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
