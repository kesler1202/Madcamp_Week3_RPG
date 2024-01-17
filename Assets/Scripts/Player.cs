using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using CommonElement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int level;
    public int currentExp;

    public int maxHP;
    public int curHP;

    public List<Skill> skillList = new List<Skill>(3);

    public Text battleHpText;
    public Text mapHpText;
    //애니메이터 사용 위한 선언
    private Animator animator; 

    // Start is called before the first frame update
    void Start()
    {
        //애니메이터 선언
        animator = GetComponent<Animator>();
        curHP = GameManager.Instance.playerCurrentHP;
        UpdateBattleHPDisplay();
        UpdateMapHPDisplay(); 
    }

    // 플레이어가 피격당했을 때 체력을 깎는 함수. 체력이 0 이하가 되면 false 리턴
    public bool PlayerHit(int damage)
    {
        curHP -= damage;
        if (curHP <= 0)
        {   
            curHP = 0;
            UpdateBattleHPDisplay();
            BattleMessage.Instance.Open("Player died");
        }
        else{
            UpdateBattleHPDisplay();
            BattleMessage.Instance.Open("Player damaged! Current HP is " + curHP);
            return true;
        }
        return false;
    }

    // 플레이어가 체력을 회복하는 함수. 단, 최대 체력 이상으로는 회복 불가
    public void PlayerHeal(int healRate)
    {
        curHP += healRate;
        // 최대체력을 초과해서 회복할 경우 최대체력 수준으로 조정
        if (maxHP < curHP)
        {
            curHP = maxHP;
        }
        UpdateBattleHPDisplay();
        BattleMessage.Instance.Open("Player healed!");
    }

    // BattleManager에게 현재 플레이어의 스킬 목록을 넘기는 함수
    public List<Skill> GetSkillList()
    {
        return skillList;
    }

    private void UpdateBattleHPDisplay()
    {
        if (battleHpText != null)
        {
            if (curHP < 0)
            {
                battleHpText.text = "HP: " + 0 + "/" + maxHP;
            }
            else
            {
                battleHpText.text = "HP: " + curHP + "/" + maxHP;
                GameManager.Instance.UpdatePlayerHP(curHP);
            }
        }
    }
    public void BAStart()
    {
        //기본공격 애니메이션 활성화
        animator.SetBool("usingBA", true);
    }
    public void BAEnd()
    {
        //기본공격 애니메이션 비활성화
        animator.SetBool("usingBA", false);
    }
    public void SwordStart()
    {
        animator.SetBool("usingSword", true);
    }
    public void SwordEnd()
    {
        animator.SetBool("usingSword", false);
    }

    public void HitStart()
    {
        animator.SetBool("getHit", true);
    }
    public void HitEnd()
    {
        animator.SetBool("getHit", false);
    }
    public void Dead()
    {
        animator.SetBool("isDead", true);
    }


    private void UpdateMapHPDisplay()
    {
        if (mapHpText != null)
        {
            GameManager.Instance.UpdatePlayerHP(curHP);
            mapHpText.text = "HP: " + GameManager.Instance.playerCurrentHP + "/" + maxHP;
        }
        else
        {
            Debug.Log("mapHpText is null");
        }
    }

    // 적이 죽었을 때 플레이어의 경험치를 획득하는 함수. 기준점 이상의 경험치를 얻었을 경우 레벨업
    // 현재 비활성화
    /*
    public void GetExp(int enemyExp)
    {
        currentExp += enemyExp;
        if (currentExp >= 20)
        {
            LevelUp();
        }
    }
    */

    // 레벨 1 증가
    // 현재 비활성화
    /*
    public void LevelUp()
    {
        // 레벨업 시 공격력, 체력 증가 & 체력 전체 회복
        level += 1;
        maxHP += 10;
        curHP = maxHP;
        // 레벨업에 사용한 경험치만큼 현재 경험치 감소
        currentExp -= 20;
        BattleMessage.Instance.Open("Level up! Current level is " + level);
    }
    */
}