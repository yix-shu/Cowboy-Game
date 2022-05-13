using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, TURN, LOST, WON}
public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleLoc;
    public Transform enemyBattleLoc;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        Instantiate(playerPrefab, playerBattleLoc);
        Instantiate(enemyPrefab, enemyBattleLoc);
    }
}
