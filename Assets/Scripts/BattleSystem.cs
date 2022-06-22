using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, TURN, LOST, WON}
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

    // Start is called before the first frame update
    void Start()
    {
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

    //----------HANDLES PLAYER ACTIONS
    IEnumerator PlayerShoot()
    {
        playerUnit.reloaded = false;
        dialogueText.text = playerUnit.unitName + " shoots " + enemyUnit.unitName;

        //check if enemy reloaded during this turn
        bool enemyIsDead = enemyUnit.TakeDamage(1);

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
            yield return new WaitForSeconds(3f);

            state = BattleState.TURN;
            SimultaneousTurn();
        }

        //Change state based on what has occurred

    }
    IEnumerator PlayerReload()
    {
        dialogueText.text = playerUnit.unitName + " reloads.";
        playerHUD.updateBullets();
        //check if enemy shot during this turn
        playerUnit.Reload();
        yield return new WaitForSeconds(3f);

        state = BattleState.TURN;
        SimultaneousTurn();
    }
    IEnumerator PlayerHold()
    {
        if (choice == "Reload")
        {
            dialogueText.text = playerUnit.unitName + " held while " + enemyUnit.unitName + " reloaded.";
        } else if (choice == "Shoot"){
            dialogueText.text = playerUnit.unitName + " held and ended up blocking " + enemyUnit.unitName + "'s shot!";
        } else
        {
            dialogueText.text = "Everyone passed/held.";
        }
        //check if enemy shot during this turn
        yield return new WaitForSeconds(3f);

        state = BattleState.TURN;
        SimultaneousTurn();
    }

    //----------HANDLES STATES
    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the duel!";
        } else if (state == BattleState.LOST)
        {
            dialogueText.text = "You have lost the duel.";
        }
    }
    void SimultaneousTurn()
    {
        dialogueText.text = "Choose a move:"; //can change this to "CHOOSE" later
        choice = enemyUnit.enemyChoose(); //our automated NPC choice chooser
        print(choice);
        if (choice == "Reload")
        {
            enemyHUD.updateBullets();
        }
    }

    //----------HANDLES BUTTON EVENTS
    public void OnShootButton()
    {
        if (state != BattleState.TURN) 
        {
            return;
        } else if (playerUnit.reloaded == false)
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
