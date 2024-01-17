using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CommonElement;

public class BattleManager : MonoBehaviour
{
    // 전체 적 정보를 담은 리스트.
    public GameObject[] allEnemies;

    public enum State
    {
        start, playerTurn, enemyTurn, win, lose
    }

    public enum SkillButtonState
    {
        inactive, // 기본 상태 (스킬 설명 출력 x)
        selected // 버튼이 눌렸으나 아직 행동이 실행되지 않은 상태 (스킬 설명 출력)
    }

    public enum KeyboardState
    {
        inactive,
        basic,
        skill1,
        skill2,
        skill3
    }

    [SerializeField]
    public Player player;
    [SerializeField]
    // 화면 상에서의 적 객체를 다루는 변수
    public Enemy enemy;
    // 적에 대한 정보를 담고 있는 데이터베이스
    public EnemyDatabase enemyDatabase;
    // 전투의 현재 상태를 나타내는 변수
    public State state;

    // 스킬버튼과 각 스킬버튼의 상태 관리
    public List<Button> skillButton = new List<Button>(3);
    private List<SkillButtonState> skillButtonStates = new List<SkillButtonState>(3);
    // 플레이어가 가지고 올 스킬들을 담는 리스트
    private List<Skill> skillList = new List<Skill>(3);
    
    // 기본공격버튼과 버튼의 상태 관리
    public Button basicAttackButton;
    private SkillButtonState basicAttackButtonState;

    // 적의 속성과 공격력 정보
    private Property enemyProperty;
    private int enemyAtk;

    // 상태 이상과 관련된 변수 선언
    int flameRemainTurns;
    int flameRate;

    // 키보드 입력으로 인한 현재 선택된 버튼의 상태
    private KeyboardState keyboardState;

    private void Awake()
    {
        // 현재 부딪힌 적만 보여주고 나머지 적들의 모습을 숨기기
        string selectedEnemyID = GameManager.Instance.CurrentEnemyID;
        HideEnemies(selectedEnemyID);

        enemy = GetCurrentEnemy(selectedEnemyID);

        // 전투 시작 알림
        state = State.start;
        // 현재 선택된 버튼 상태를 inactive로 설정
        keyboardState = KeyboardState.inactive;
        SetUpBattle();
        BattleStart();
    }

    // 부딪힌 적을 제외한 나머지 적들을 숨기기
    void HideEnemies(string selectedEnemyID)
    {
        foreach (GameObject enemy in allEnemies)
        {
            if (enemy != null)
            {
                if (enemy.GetComponent<Enemy>().enemyID != selectedEnemyID)
                {
                    enemy.SetActive(false);
                }
            }
        }
    }

    Enemy GetCurrentEnemy(string selectedEnemyID)
    {
        foreach (GameObject enemy in allEnemies)
        {
            if (enemy != null)
            {
                if (enemy.GetComponent<Enemy>().enemyID == selectedEnemyID)
                {
                    return enemy.GetComponent<Enemy>();
                }
            }
        }
        return null;
    }

    // 전투 초기 설정. 플레이어와 적을 지정한 뒤 공격력과 타입 정보 가져오기
    private void SetUpBattle()
    {
        // player와 enemy 오브젝트 설정
        // 이 부분을 필요에 따라 수정
        player = FindObjectOfType<Player>();

        // player와 enemy 오브젝트가 없으면 디버그 로그 출력
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }

        string enemyID = GameManager.Instance.CurrentEnemyID;
        Debug.Log("Searching for enemy with ID: " + enemyID);

        if (enemyDatabase == null)
        {
            Debug.LogError("EnemyDatabase is not set on " + gameObject.name);
            return;
        }

        EnemyData enemyData = enemyDatabase.GetEnemyDataById(enemyID);
        enemy.BattleSceneHP(enemyData.maxHP);
        if (enemyData == null)
        {
            Debug.LogError("No enemy found with ID: " + enemyID);
            return;
        }

        // 플레이어와 적의 정보 가져오기
        skillList = player.GetSkillList();

        enemyProperty = enemy.GetEnemyProperty(enemyData);
        enemyAtk = enemy.GetEnemyAtk(enemyData);

        // 초기 상태로 모든 버튼을 설정
        for (int i = 0; i < 3; i++)
        {
            skillButtonStates.Add(SkillButtonState.inactive);
        }
    }

    // 전투 시작
    public void BattleStart()
    {
        // 플레이어 턴으로 초기 설정. 스피드 속성을 넣는다면 스피드 값에 따라 누가 선공을 잡을지 바꿀 수 있을 듯.
        state = State.playerTurn;
    }

    private void Update()
    {
        // 현재 출력 중인 메시지가 있거나 플레이어의 턴이 아니라면 키보드 입력이 되지 않도록 막기
        if (BattleMessage.Instance.IsDisplayingMessage() || !basicAttackButton.interactable)
        {
            return;
        }
        // 아래 방향키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 기본공격
            if (keyboardState == KeyboardState.inactive)
            {
                keyboardState = KeyboardState.basic;
                SelectBasicAttackButton();
            }
            // 기본공격 버튼에서 아래 키를 눌렀으므로 1번 스킬 선택
            else if (keyboardState == KeyboardState.basic)
            {
                keyboardState = KeyboardState.skill1;
                SelectSkillButton(0);
                
            }
        }
        // 오른쪽 방향키를 눌렀을 때
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // 2번 스킬
            if (keyboardState == KeyboardState.skill1)
            {
                keyboardState = KeyboardState.skill2;
                SelectSkillButton(1);
            }
            // 3번 스킬
            else if (keyboardState == KeyboardState.skill2)
            {
                keyboardState = KeyboardState.skill3;
                SelectSkillButton(2);
            }
        }
        // 왼쪽 방향키를 눌렀을 때
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // 2번 스킬
            if (keyboardState == KeyboardState.skill3)
            {
                keyboardState = KeyboardState.skill2;
                SelectSkillButton(1);
            }
            // 1번 스킬
            else if (keyboardState == KeyboardState.skill2)
            {
                keyboardState = KeyboardState.skill1;
                SelectSkillButton(0);
            }
        }
        // 위쪽 방향키를 눌렀을 때
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 기본공격
            if (keyboardState == KeyboardState.skill1 || keyboardState == KeyboardState.skill2 || keyboardState == KeyboardState.skill3)
            {
                keyboardState = KeyboardState.basic;
                SelectBasicAttackButton();
            }
        }
        // 버튼이 선택된 상태에서 엔터키를 눌렀을 때 버튼에 해당하는 행동 수행
        // 현재 쿨타임 상태일 경우 스킬 사용 불가 문구 출력
        // 모든 엔터 버튼은 basicAttackButton이 활성화되어있을 때에만 상호작용 가능
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            switch (keyboardState)
            {
                case KeyboardState.basic:
                    StartCoroutine(PlayerBasicAttackAct());
                    break;
                case KeyboardState.skill1:
                    if (!skillButton[0].interactable) {
                        BattleMessage.Instance.Open("Cannot use this skill..");
                        break;
                    }
                    StartCoroutine(PlayerSkillAct(0));
                    break;
                case KeyboardState.skill2:
                    if (!skillButton[1].interactable) {
                        BattleMessage.Instance.Open("Cannot use this skill..");
                        break;
                    }
                    StartCoroutine(PlayerSkillAct(1));
                    break;
                case KeyboardState.skill3:
                    if (!skillButton[2].interactable) {
                        BattleMessage.Instance.Open("Cannot use this skill..");
                        break;
                    }
                    StartCoroutine(PlayerSkillAct(2));
                    break;
                default:
                    break;
            }
        }
    }

    // 기본공격 버튼을 선택했을 때 문구 출력
    private void SelectBasicAttackButton()
    {
        // 버튼의 상태를 selected로 변경
        basicAttackButton.Select();

        // 기본공격 설명 표시
        BattleMessage.Instance.Open("Basic attack damage: 3~6");
    }

    // 스킬 버튼을 선택했을 때 문구 출력
    private void SelectSkillButton(int skillNum)
    {
        // 버튼의 상태를 selected로 변경
        skillButton[skillNum].Select();

        // 스킬 설명 표시
        if (skillButton[skillNum].interactable)
        {
            BattleMessage.Instance.Open($"Skill property: {skillList[skillNum].GetSkillProperty()}\nDamage: {skillList[skillNum].GetSkillAtk()}");
        }
        else
        {
            BattleMessage.Instance.Open($"Skill property: {skillList[skillNum].GetSkillProperty()}\nDamage: {skillList[skillNum].GetSkillAtk()}\nCooldown: {skillList[skillNum].GetCurrentCooldown()+1} turns");
        }
    }

    // 기본공격 수행
    IEnumerator PlayerBasicAttackAct()
    {
        // 플레이어 턴이 될 때까지 모든 버튼 비활성화
        for (int i = 0; i < 3; i++)
        {
            skillButton[i].interactable = false;
        }
        basicAttackButton.interactable = false;
        player.BAStart();
        enemy.HitStart();
        BattleMessage.Instance.Open("Attack!");

        yield return new WaitForSeconds(1.5f);
        player.BAEnd();

        // 적이 죽었을 경우 전투 종료
        // 기본 공격의 데미지는 3~5 사이의 랜덤한 숫자임
        if (!enemy.EnemyHit(Random.Range(3, 6)))
        {
            enemy.Dead();
            yield return new WaitForSeconds(1.5f);

            state = State.win;
            EndBattle();
        }
        // 적이 죽지 않았을 경우 스킬 쿨타임 초기화
        else
        {
            enemy.HitEnd();
            yield return new WaitForSeconds(1.5f);
            // 적의 턴으로 넘어가기
            if (state == State.playerTurn)
            {
                state = State.enemyTurn;
                EnemyTurn();
            }
        }
    }

    // 해당 스킬에 따른 플레이어의 공격 수행
    IEnumerator PlayerSkillAct(int skillNum)
    {
        // 플레이어 턴이 될 때까지 모든 버튼 비활성화
        for (int i = 0; i < 3; i++)
        {
            skillButton[i].interactable = false;
        }
        basicAttackButton.interactable = false;

        // 스킬의 종류가 attack일 경우
        if (skillList[skillNum].GetSkillType() == SkillType.attack)
        {
            player.SwordStart();
            enemy.HitStart();
            BattleMessage.Instance.Open("Skill use!");
            yield return new WaitForSeconds(1.5f);
            player.SwordEnd();

            // 공격이 명중했을 경우 - 총 데미지 계산 후 적에게 적용
            if (skillList[skillNum].IsHit())
            {
                // 적이 죽었을 경우 전투 종료
                if (!enemy.EnemyHit(CalculateDamage(skillList[skillNum].GetSkillProperty(), enemyProperty, skillList[skillNum].GetSkillAtk())))
                {
                    enemy.Dead();
                    yield return new WaitForSeconds(1.5f);

                    // 예외적으로 풀타입의 경우 적이 죽어도 체력 회복이 가능하도록 설계
                    if (skillList[skillNum].GetSkillProperty() == Property.grass)
                    {
                        player.PlayerHeal(skillList[skillNum].GetHealRate());
                        yield return new WaitForSeconds(1.5f);
                    }
                    
                    state = State.win;
                    EndBattle();
                    
                    yield break;
                }
                // 적이 죽지 않았을 경우 사용한 스킬 쿨타임 리셋
                else
                {
                    enemy.HitEnd();
                    yield return new WaitForSeconds(1.5f);

                    skillList[skillNum].ResetCooldown();
                }

                // 스킬 속성에 따라 부가효과 적용
                // 불속성 스킬 - 화상 적용
                if (skillList[skillNum].GetSkillProperty() == Property.fire)
                {
                    if (flameRemainTurns == 0) {
                        // 화상 지속 턴수 초기화 (현재 화상 지속 턴수: 3)
                        flameRemainTurns = 3;
                        flameRate = skillList[skillNum].GetFlameRate();

                        BattleMessage.Instance.Open("Enemy started to burn!");
                        yield return new WaitForSeconds(1.5f);
                    }
                    else {
                        BattleMessage.Instance.Open("Enemy already burned.");
                        yield return new WaitForSeconds(1.5f);
                    }
                }
                // 풀속성 스킬 - 체력 회복
                else if (skillList[skillNum].GetSkillProperty() == Property.grass)
                {
                    player.PlayerHeal(skillList[skillNum].GetHealRate());
                    yield return new WaitForSeconds(1.5f);
                }
                // 물속성 스킬 - 스킬 쿨타임 감소
                else if (skillList[skillNum].GetSkillProperty() == Property.water)
                {
                    BattleMessage.Instance.Open("Cooltime reduced!");
                    yield return new WaitForSeconds(1.5f);
                    
                    for (int i = 0; i < 3; i++)
                    {
                        // 현재 남은 쿨타임이 물 속성 스킬의 쿨타임 감소 계수보다 클 경우
                        if (skillList[i].GetCurrentCooldown() >= skillList[skillNum].GetCooldownReduceRate())
                        {
                            skillList[i].ReduceCooldown(skillList[skillNum].GetCooldownReduceRate());
                        }
                        // 현재 남은 쿨타임이 물 속성 스킬의 쿨타임 감소 계수보다 작을 경우 -> 쿨타임을 0으로 설정
                        else
                        {
                            skillList[i].ReduceCooldown(skillList[i].GetCurrentCooldown());
                        }
                    }
                }
            }
            // 스킬이 명중하지 않았을 경우
            else
            {
                BattleMessage.Instance.Open("Missed..");
                yield return new WaitForSeconds(1.5f);

                skillList[skillNum].ResetCooldown();
            }
        }
        // 스킬의 종류가 buff일 경우
        else if (skillList[skillNum].GetSkillType() == SkillType.buff)
        {
            
        }
        // 스킬의 종류가 heal일 경우
        else if (skillList[skillNum].GetSkillType() == SkillType.heal)
        {
            
        }

        // 적의 턴으로 넘어가기
        if (state == State.playerTurn)
        {
            state = State.enemyTurn;
            EnemyTurn();
        }
    }

    // 적의 턴 종료 전 상태 이상이 있을 경우 상태이상 부여
    IEnumerator EnemyDebuff()
    {
        // 화상 상태일 경우 적에게 화상 데미지 부여
        if (flameRemainTurns > 0) {
            BattleMessage.Instance.Open("Enemy is burning..");
            yield return new WaitForSeconds(1.5f);

            if (!enemy.EnemyHit(flameRate))
            {
                yield return new WaitForSeconds(1.5f);

                state = State.win;
                EndBattle();
            }
            yield return new WaitForSeconds(1.5f);

            flameRemainTurns -= 1;
            if (flameRemainTurns == 0)
            {
                BattleMessage.Instance.Open("Enemy burn ended.");
                yield return new WaitForSeconds(1.5f);
            }
        }

        // 상태이상 적용 후 플레이어에게 턴 넘기기
        if (state == State.enemyTurn)
        {
            state = State.playerTurn;

            // 스킬 쿨타임 관리
            for (int i = 0; i < 3; i++)
            {
                // 스킬의 쿨타임이 0이라면 스킬버튼 활성화
                if (skillList[i].GetCurrentCooldown() == 0)
                {
                    skillButton[i].interactable = true;
                }
                // 0이 아니라면 쿨타임 1 줄이기
                else
                {
                    skillList[i].ReduceCooldown();
                }
            }

            BattleMessage.Instance.Open("Player's turn");
            // 키보드 위치상태를 inactive으로 설정
            keyboardState = KeyboardState.inactive;
            // 기본공격 버튼 활성화
            basicAttackButton.interactable = true;
        }
    }

    // 적 턴
    private void EnemyTurn()
    {
        // 적 턴에서 적이 할 수 있는 행동들

        // 공격
        StartCoroutine(EnemyAttack());
    }

    // 적의 공격 수행
    IEnumerator EnemyAttack()
    {
        BattleMessage.Instance.Open("Enemy attack!");
        enemy.AttackStart();
        yield return new WaitForSeconds(0.7f);
        enemy.AttackEnd();

        // 총 데미지 계산 후 적에게 적용
        // 적의 공격은 속성을 고려하지 않고 적의 공격력만 고려해서 계산
        // 플레이어가 죽었을 경우 전투 종료
        if (!player.PlayerHit(enemyAtk))
        {
            player.Dead();
            yield return new WaitForSeconds(1.0f);
            state = State.lose;
            EndBattle();
        }
        // 플레이어가 죽지 않았을 경우 플레이어에게 턴이 넘어감
        else{
            player.HitStart();
            yield return new WaitForSeconds(1.0f);
            player.HitEnd();
            yield return new WaitForSeconds(1.0f);
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(EnemyDebuff());
        }
    }

    // 데미지를 계산하는 함수. 상성 관계의 속성에 따라 데미지 값이 바뀜
    int CalculateDamage(Property propertyAttack, Property propertyDefence, int atk)
    {
        int damage = atk;
        // 상대방의 속성에 강점을 가지는 속성일 경우 1.5배로 데미지 계산
        if ((propertyAttack == Property.water && propertyDefence == Property.fire) || (propertyAttack == Property.fire && propertyDefence == Property.grass) || (propertyAttack == Property.grass && propertyDefence == Property.water)) {
            damage = (int)(damage * 1.5);
        }
        // 상대방의 속성에 약점을 가지는 속성일 경우 0.5배로 데미지 계산
        else if ((propertyAttack == Property.water && propertyDefence == Property.grass) || (propertyAttack == Property.fire && propertyDefence == Property.water) || (propertyAttack == Property.grass && propertyDefence == Property.fire)) {
            damage *= (int)(damage * 0.5);
        }
        
        return damage;
    }

    // 전투 종료
    void EndBattle()
    {
        GameManager.Instance.AddDeadEnemyID(GameManager.Instance.CurrentEnemyID);

        StartCoroutine(DisplayEndBattleMessage());
    }

    // 전투 종료 메시지를 표시하는 코루틴
    private IEnumerator DisplayEndBattleMessage()
    {
        BattleMessage.Instance.Open("Battle ends");
        yield return new WaitForSeconds(1.5f);

        // 현재 메시지가 표시 중이라면 대기
        while (BattleMessage.Instance.IsDisplayingMessage())
        {
            yield return null;
        }

        Debug.Log(state);
        if (state == State.lose)
        {
            Debug.Log(state);
            SceneManager.LoadScene("EndScene");
        }
        else
        {
           SceneManager.LoadScene("MapScene"); 
        }
    }
}