using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonElement;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Property property;
    public string enemyID;
    public string enemyName;
    public int exp;

    public int maxHP;
    public int curHP;
    public Text enemyHpText;
    public EnemyDatabase enemyDatabase;

    public int atk;
    private Animator animator; 

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateEnemyHPDisplay();
    }

    public void BattleSceneHP(int enemyMaxHP)
    {
        curHP = enemyMaxHP;
        maxHP = enemyMaxHP;
    }

    // 적이 피격당했을 때 체력을 깎는 함수. 체력이 0 이하가 되면 false 리턴
    public bool EnemyHit(int damage)
    {
        curHP -= damage;
        if (curHP <= 0)
        {
            curHP = 0;
            UpdateEnemyHPDisplay();
            BattleMessage.Instance.Open("Enemy died");
            // 적 죽음
            return false;
        }
        else
        {
            UpdateEnemyHPDisplay();
            BattleMessage.Instance.Open("Enemy damaged! Current HP is " + curHP);
            return true;
        }
    }

    private void UpdateEnemyHPDisplay()
    {
        if (enemyHpText != null)
        {
            if (curHP < 0)
            {
                enemyHpText.text = "HP: " + 0 + "/" + maxHP;
            }
            else
            {
                enemyHpText.text = "HP: " + curHP + "/" + maxHP;
            }   
        }
    }

    public int GetEnemyAtk(EnemyData enemyData)
    {
        return enemyData.atk;
    }

    public Property GetEnemyProperty(EnemyData enemyData)
    {
        return enemyData.property;
    }

    public void AttackStart()
    {
        //기본공격 애니메이션 활성화
        animator.SetBool("isAttack", true);
    }
    public void AttackEnd()
    {
        //기본공격 애니메이션 비활성화
        animator.SetBool("isAttack", false);
    }

    public void HitStart()
    {
        Debug.Log(enemyID);
        animator.SetBool("isHit", true);
    }
    public void HitEnd()
    {
        animator.SetBool("isHit", false);
    }
    public void Dead()
    {
        animator.SetBool("isDead", true);
    }
}
