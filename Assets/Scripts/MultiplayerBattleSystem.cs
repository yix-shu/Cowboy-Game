using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

namespace Assets.Scripts
{
    public enum MultiplayerBattleState { START, TURN, WAIT, LOST, WON, TIE }
    public enum BattleAction { SHOOT, RELOAD, HOLD, NONE }
    /*
     TODO:
    - will want to fix the health updates so that it updates with the Shoot, Reload, Hold events and not after them because the HUD looks weird then.
    - make sure to check when the ammo is used up
     */
    public class MultiplayerBattleSystem : MonoBehaviour
    {
        public MultiplayerBattleState state;

        public UnityEvent onEnemyInput;

        public bool started = false;
        public bool websocketReady = true;

        public Text dialogueText;

        GameObject playerPrefab;
        GameObject enemyPrefab;

        public Transform playerBattleLoc;
        public Transform enemyBattleLoc;

        GameObject playerGO;
        GameObject enemyGO;

        Unit playerUnit;
        Level1NPC enemyUnit;

        public BattleHUD playerHUD;
        public BattleHUD enemyHUD;

        private BattleAction yourAction;
        public BattleAction enemyAction;
        private bool reloaded = false;

        public AudioSource source;

        // Start is called before the first frame update
        void Start()
        {
            playerPrefab = OutfitManager.instance.playerOutfit;
            enemyPrefab = OutfitManager.instance.outfits[Random.Range(0, 5)];

            AudioManager.instance.enableAudioSource(source);



        }
        void Update()
        {
            if (started == false && websocketReady)
            {
                state = MultiplayerBattleState.START;
                StartCoroutine(SetupBattle());
                started = true;
                websocketReady = false;
            }

        }

        IEnumerator SetupBattle()
        {
            playerGO = Instantiate(playerPrefab, playerBattleLoc);
            playerGO.AddComponent<Unit>();
            playerUnit = playerGO.GetComponent<Unit>();
            playerUnit.defaultProperties();

            enemyGO = Instantiate(enemyPrefab, enemyBattleLoc);
            enemyGO.AddComponent<Level1NPC>();
            enemyGO.transform.localScale = new Vector3(-1, 1, 1);
            enemyUnit = enemyGO.GetComponent<Level1NPC>();
            enemyUnit.defaultProperties();

            dialogueText.text = enemyUnit.unitName + " has challenged you";

            playerHUD.SetHUD(playerUnit);
            enemyHUD.SetHUD(enemyUnit);

            yield return new WaitForSeconds(3f); //a bit of break before the round begins

            state = MultiplayerBattleState.TURN;
            SimultaneousTurn();
        }
        //----------HANDLES HUD CHANGES
        public void updateHPHUD()
        {
            enemyHUD.updateHP(enemyUnit.currentHP);
            playerHUD.updateHP(playerUnit.currentHP);
        }

        //----------HANDLES PLAYER ACTIONS
        IEnumerator CheckAction()
        {
            state = MultiplayerBattleState.WAIT;
            //check player1 and player2's actions from server
            //if player1 shot and player2 reload
            //if player2 shot and player1 reload

            if (enemyAction == BattleAction.NONE)
            {
                Debug.Log("Wait for other player to pick");
            }
            else
            {
                Debug.Log("Everyone has chosen.");
                if (enemyAction == BattleAction.SHOOT)
                {
                    if (yourAction == BattleAction.RELOAD)
                    {
                        //lose one health
                    }
                    else if (yourAction == BattleAction.SHOOT)
                    {
                        //both lose one health 
                    }
                    else
                    {
                        //you blocked enemy's shot
                    }
                }
                else if (enemyAction == BattleAction.RELOAD)
                {
                    //update enemy's reload
                    if (yourAction == BattleAction.RELOAD)
                    {
                        //both reloaded
                        //update your reload
                    }
                    else if (yourAction == BattleAction.SHOOT)
                    {
                        //update enemy's health 
                        //
                    }
                    else
                    {
                        //enemy reloaded while you held/passed
                    }
                }

                //check your health or (bullets and not reloaded)
                if (playerUnit.currentHP == 0 || (playerUnit.ammoLeft == 0 && !playerUnit.reloaded))
                {
                    EventMessage eventMessage = new EventMessage("OnMessage", WebSocketService.YouDiedOp);
                    //display that you lost 
                    WebSocketService.instance.SendWebSocketMessage(JsonUtility.ToJson(eventMessage));
                    state = MultiplayerBattleState.LOST;
                    EndBattle();
                }
                else if (enemyUnit.currentHP == 0 || (enemyUnit.ammoLeft == 0 && !enemyUnit.reloaded))
                {
                    EventMessage eventMessage = new EventMessage("OnMessage", WebSocketService.YouWonOp);
                    //display that you won
                    WebSocketService.instance.SendWebSocketMessage(JsonUtility.ToJson(eventMessage));
                    state = MultiplayerBattleState.WON;
                    EndBattle();
                }
                //check enemy's health or (bullets and not reloaded)

                yield return new WaitForSeconds(3f);
                state = MultiplayerBattleState.TURN;
                clearEnemyAction();
                SimultaneousTurn();
            }
        }

        IEnumerator WaitBeforeMenu()
        {
            SaveSystem.SavePlayer(GameMaster.instance.player);
            yield return new WaitForSeconds(3f);
            UIController.switchScene(-1);

        }

        //----------HANDLES STATES
        void EndBattle()
        {
            AudioManager.instance.disableAudioSource(source);
            if (state == MultiplayerBattleState.WON)
            {
                dialogueText.text = "You won the duel! You have gained 10 exp.";
                GameMaster.instance.player.exp += 10;
            }
            else if (state == MultiplayerBattleState.LOST)
            {
                dialogueText.text = "You have lost the duel. You have lost 10 exp.";
                GameMaster.instance.player.exp -= 10;
            }
            else if (state == MultiplayerBattleState.TIE)
            {
                dialogueText.text = "The duel was inconclusive. Your exp did not change.";
            }
            StartCoroutine(WaitBeforeMenu());
        }
        void SimultaneousTurn()
        {
            dialogueText.text = "Choose a move:"; //can change this to "CHOOSE" later
            //wait for enemy to choose
        }

        //----------HANDLES BUTTON EVENTS
        public void OnShootButton()
        {
            if (state != MultiplayerBattleState.TURN)
            {
                return;
            }
            else if (playerUnit.reloaded == false)
            {
                dialogueText.text = "Reload gun before shooting.";
                //make it an automatic Hold
                OnHoldButton();
            }
            EventMessage shootMessage = new EventMessage("OnMessage", WebSocketService.ShootOp);
            WebSocketService.instance.SendWebSocketMessage(JsonUtility.ToJson(shootMessage));
            StartCoroutine(CheckAction());
        }
        public void OnReloadButton()
        {
            if (state != MultiplayerBattleState.TURN)
            {
                return;
            }
            EventMessage reloadMessage = new EventMessage("OnMessage", WebSocketService.ReloadOp);
            WebSocketService.instance.SendWebSocketMessage(JsonUtility.ToJson(reloadMessage));
            StartCoroutine(CheckAction());
        }
        public void OnHoldButton()
        {
            if (state != MultiplayerBattleState.TURN)
            {
                return;
            }
            EventMessage holdMessage = new EventMessage("OnMessage", WebSocketService.HoldOp);
            WebSocketService.instance.SendWebSocketMessage(JsonUtility.ToJson(holdMessage));
            StartCoroutine(CheckAction());
        }

        //enemy actions
        public void clearEnemyAction()
        {
            enemyAction = BattleAction.NONE;
        }

        public void updateEnemyAction(BattleAction action)
        {
            enemyAction = action;
        }
    }
}