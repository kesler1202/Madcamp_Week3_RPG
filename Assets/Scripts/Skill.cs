using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonElement;

public class Skill : MonoBehaviour
{
    public string skillName;
    // 스킬의 속성
    public Property property;
    // 스킬의 종류
    public SkillType skillType;
    // 스킬 자체 공격력
    public int atk;
    // 스킬 명중률. 백분율로 계산
    public int accuracy;
    // 스킬의 쿨타임 및 현 시점으로부터 몇 턴 뒤에 재사용 가능한지 여부를 알려주는 변수
    // ex) maxCooldown이 0이면 매 턴 사용 가능, 1이면 1회 사용 후 1턴 쉬어야 함.
    public int maxCooldown, curCooldown;
    // 스킬에 달려 있는 화상 데미지 계수 (불속성 한정)
    public int flameRate;
    // 스킬에 달려 있는 체력 회복 계수 (풀속성 한정)
    public int healRate;
    // 스킬에 달려 있는 쿨타임 감소 계수 (물속성 한정)
    public int cooldownReduceRate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // 스킬의 명중률에 따른 명중 여부 계산
    public bool IsHit()
    {
        int randomValue = Random.Range(1, 101);

        return randomValue <= accuracy;
    }

    // 스킬을 사용 가능한 남은 턴수를 리턴
    public int GetCurrentCooldown()
    {
        return curCooldown;
    }

    // 스킬 사용 후 쿨타임 초기화
    public void ResetCooldown()
    {
        curCooldown = maxCooldown;
    }

    // 턴 지난 후 스킬의 쿨타임을 1씩 줄임
    public void ReduceCooldown()
    {
        curCooldown -= 1;
    }

    public void ReduceCooldown(int reduceRate)
    {
        curCooldown -= reduceRate;
    }

    public int GetSkillAtk()
    {
        return atk;
    }

    public Property GetSkillProperty()
    {
        return property;
    }

    public SkillType GetSkillType()
    {
        return skillType;
    }

    public int GetFlameRate()
    {
        return flameRate;
    }

    public int GetHealRate()
    {
        return healRate;
    }

    public int GetCooldownReduceRate()
    {
        return cooldownReduceRate;
    }
}
