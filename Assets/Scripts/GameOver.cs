using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    void Start()
    {
        // 플레이어의 초기 위치 재조정
        GameManager.Instance.UpdatePlayerLoc(new Vector3(-7.5f,-4.5f,0));
        // 플레이어 체력 복구
        GameManager.Instance.playerCurrentHP = 30;
        // 적 리스폰
        GameManager.Instance.RespawnEnemies();
    }

    void Update()
    {
        // Check if any key is pressed
        if (Input.anyKeyDown)
        {
            // Load 'MapScene'
            SceneManager.LoadScene("SceneBetwScene");
        }
    }
}
