using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string CurrentEnemyID { get; set; }

    public int playerCurrentHP = 30;

    public Vector3 playerLoc = new Vector3(-7.5f,-4.5f,0);

    public List<string> deadEnemyList = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UpdatePlayerHP(int newHP)
    {
        playerCurrentHP = newHP;
    }

    // 전투씬 전환 시 현재 플에이어의 위치값을 저장
    public void UpdatePlayerLoc(Vector3 currentLoc)
    {
        playerLoc = currentLoc;
    }

    // 맵 화면 로드 시 초기 플레이어의 위치값을 가져오는 함수
    public Vector3 GetPlayerLoc()
    {
        return playerLoc;
    }

    // 적을 죽였을 때 죽은 적의 정보를 가지고 오는 함수
    public void AddDeadEnemyID(string deadEnemyID)
    {
        deadEnemyList.Add(deadEnemyID);
    }

    // 게임오버 시 적을 리스폰하는 함수
    public void RespawnEnemies()
    {
        deadEnemyList.Clear();
    }
}