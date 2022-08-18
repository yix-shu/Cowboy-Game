using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public enum BattleState { START, TURN, LOST, WON, TIE }

    /*
     TODO:
    - will want to fix the health updates so that it updates with the Shoot, Reload, Hold events and not after them because the HUD looks weird then.
    - make sure to check when the ammo is used up
     */
    public class BattleSystem : MonoBehaviour
    {
        public BattleState state;
        public Text dialogueText;

        public GameObject playerPrefab;
        public GameObject enemyPrefab;

        public Transform playerBattleLoc;
        public Transform enemyBattleLoc;

        Unit playerUnit;
        Level1NPC enemyUnit;

        public BattleHUD playerHUD;
        public BattleHUD enemyHUD;

        private string choice;

        public AudioSource source;

        // Start is called before the first frame update
        void Start()
        {
            AudioManager.instance.enableAudioSource(source);
            state = BattleState.START;
            StartCoroutine(SetupBattle());
        }

        IEnumerator SetupBattle()
        {
            GameObject playerGO = Instantiate(playerPrefab, playerBattleLoc);
            playerUnit = playerGO.GetComponent<Unit>();

            GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleLoc);
            enemyUnit = enemyGO.GetComponent<Level1NPC>();

            dialogueText.text = enemyUnit.unitName + " has challenged you";

            playerHUD.SetHUD(playerUnit);
            enemyHUD.SetHUD(enemyUnit);

            yield return new WaitForSeconds(3f); //a bit of break before the round begins

            state = BattleState.TURN;
            SimultaneousTurn();
        }
        //----------HANDLES HUD CHANGES
        public void updateHPHUD()
        {
            enemyHUD.updateHP(enemyUnit.currentHP);
            playerHUD.updateHP(playerUnit.currentHP);
        }

        //----------HANDLES PLAYER ACTIONS
        IEnumerator PlayerShoot()
        {
            if (playerUnit.reloaded)
            {
                playerUnit.reloaded = false;
                if (choice == "Reload")
                {
                    dialogueText.text = playerUnit.unitName + " shot " + enemyUnit.unitName + " while they were reloading!";
                    enemyHUD.updateBullets();
                    enemyUnit.reloaded = true;
                    //check if enemy reloaded during this turn
                    bool enemyIsDead = enemyUnit.TakeDamage(1);
                    updateHPHUD();
                    //enemyHUD.updateHP(enemyUnit.currentHP);

                    yield return new WaitForSeconds(3f);

                    //Check if the enemy has died
                    if (enemyIsDead)
                    {
                        //End battle
                        state = BattleState.WON;
                        EndBattle();
                    }
                    else
                    {
                        //New turn 
                        state = BattleState.TURN;
                        SimultaneousTurn();
                    }
                }
                else if (choice == "Shoot")
                {
                    dialogueText.text = "Everyone was shot and received injuries!";
                    enemyUnit.reloaded = false;
                    bool playerIsDead = playerUnit.TakeDamage(1);
                    bool enemyIsDead = enemyUnit.TakeDamage(1);
                    updateHPHUD();

                    //enemyHUD.updateHP(enemyUnit.currentHP);

                    yield return new WaitForSeconds(3f);

                    //Check if the enemy has died
                    if (enemyIsDead && playerIsDead)
                    {
                        state = BattleState.TIE;
                        EndBattle();
                    }
                    else if (enemyIsDead)
                    {
                        //End battle
                        state = BattleState.WON;
                        EndBattle();
                    }
                    else if (playerIsDead)
                    {
                        //End battle
                        state = BattleState.LOST;
                        EndBattle();
                    }
                    else
                    {
                        //New turn 
                        state = BattleState.TURN;
                        SimultaneousTurn();
                    }
                }
                else
                {
                    dialogueText.text = playerUnit.unitName + " shot but " + enemyUnit.unitName + " held and ended up blocking it!";
                }
            }
            else
            {
                dialogueText.text = "You cannot shoot without reloading" + ". As a result, " + playerUnit.unitName + " held.";
                yield return new WaitForSeconds(3f);
                StartCoroutine(PlayerHold());
            }


        }
        IEnumerator PlayerReload()
        {
            if (playerUnit.reloaded)
            {
                dialogueText.text = "You cannot reload more than once. As a result, " + playerUnit.unitName + " held.";
                yield return new WaitForSeconds(3f);
                StartCoroutine(PlayerHold());
            }
            else
            {
                if (choice == "Reload")
                {
                    dialogueText.text = "Everyone reloaded.";
                    enemyUnit.reloaded = true;
                    enemyHUD.updateBullets();
                }
                else if (choice == "Shoot")
                {
                    dialogueText.text = playerUnit.unitName + " reloaded while " + enemyUnit.unitName + " shot them!";
                    enemyUnit.reloaded = false;
                    bool playerIsDead = playerUnit.TakeDamage(1);
                    updateHPHUD();
                    yield return new WaitForSeconds(3f);

                    //Check if the enemy has died
                    if (playerIsDead)
                    {
                        //End battle
                        state = BattleState.LOST;
                        EndBattle();
                    }
                    else
                    {
                        //New turn 
                        state = BattleState.TURN;
                        SimultaneousTurn();
                    }
                }
                else
                {
                    dialogueText.text = playerUnit.unitName + " reloaded while " + enemyUnit.unitName + " held.";
                }
                //dialogueText.text = playerUnit.unitName + " reloads.";
                playerHUD.updateBullets();
                //check if enemy shot during this turn
                playerUnit.Reload();
            }
            yield return new WaitForSeconds(3f);

            state = BattleState.TURN;
            SimultaneousTurn();
        }
        IEnumerator PlayerHold()
        {
            if (choice == "Reload")
            {
                dialogueText.text = playerUnit.unitName + " held while " + enemyUnit.unitName + " reloaded.";
                enemyUnit.reloaded = true;
                enemyHUD.updateBullets();
            }
            else if (choice == "Shoot")
            {
                dialogueText.text = playerUnit.unitName + " held and ended up blocking " + enemyUnit.unitName + "'s shot!";
                enemyUnit.reloaded = false;
            }
            else
            {
                dialogueText.text = "Everyone passed/held.";
            }
            //check if enemy shot during this turn
            yield return new WaitForSeconds(3f);

            state = BattleState.TURN;
            SimultaneousTurn();
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
            if (state == BattleState.WON)
            {
                dialogueText.text = "You won the duel! You have gained 10 exp.";
                GameMaster.instance.player.exp += 10;
            }
            else if (state == BattleState.LOST)
            {
                dialogueText.text = "You have lost the duel. You have lost 10 exp.";
                GameMaster.instance.player.exp -= 10;
            }
            else if (state == BattleState.TIE)
            {
                dialogueText.text = "The duel was inconclusive. Your exp did not change.";
            }
            StartCoroutine(WaitBeforeMenu());
        }
        void SimultaneousTurn()
        {
            dialogueText.text = "Choose a move:"; //can change this to "CHOOSE" later
            choice = enemyUnit.enemyChoose(); //our automated NPC choice chooser
            print(choice);
            /*if (choice == "Reload")
            {
                enemyHUD.updateBullets();
            }*/
        }

        //----------HANDLES BUTTON EVENTS
        public void OnShootButton()
        {
            if (state != BattleState.TURN)
            {
                return;
            }
            else if (playerUnit.reloaded == false)
            {
                dialogueText.text = "Reload gun before shooting.";
                return;
            }
            StartCoroutine(PlayerShoot());
        }
        public void OnReloadButton()
        {
            if (state != BattleState.TURN)
            {
                return;
            }
            StartCoroutine(PlayerReload());
        }
        public void OnHoldButton()
        {
            if (state != BattleState.TURN)
            {
                return;
            }
            StartCoroutine(PlayerHold());
        }
    }
}