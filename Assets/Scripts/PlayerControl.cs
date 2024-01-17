using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyTransition
{
    public class PlayerControl : MonoBehaviour
    {
        public float moveSpeed = 3.0f; // 이동 속도
        public float stepSize = 1.0f;  // 한 번에 이동할 거리
        private Vector3 targetPosition;
        private bool isMoving = false;
        private Animator animator;

        // 장면 전환 애니메이션 환경변수
        public TransitionSettings transition;
        // 장면 전환 딜레이
        public float loadDelay;

        // esc 키를 눌렀을 때 나타나는 종료 dialog
        public GameObject _quitDialog;
        // esc 키를 누른 상태인지를 확인하기 위한 flag
        private bool isQuitStatus;

        // 맵 상에서 적의 존재 유무를 관리하기 위한 전체 적 정보를 담은 리스트
        public GameObject[] allEnemies;

        void Start()
        {
            targetPosition = GameManager.Instance.GetPlayerLoc();
            transform.position = targetPosition;
            animator = GetComponent<Animator>();

            // quitDialog가 안 보이도록 설정
            _quitDialog.SetActive(false);
            isQuitStatus = false;

            HideDeadEnemies();
        }

        void Update()
        {
            if (isMoving)
            {
                MoveTowardsTarget();
            }
            else
            {
                CheckForInput();
            }
        }

        // 전투에서 죽은 적들을 맵에서 지우는 함수
        void HideDeadEnemies()
        {
            foreach (string deadEnemyID in GameManager.Instance.deadEnemyList)
            {
                foreach (GameObject enemy in allEnemies)
                {
                    if (enemy.GetComponent<Enemy>().enemyID == deadEnemyID)
                    {
                        enemy.SetActive(false);
                        break;
                    }
                } 
            }
        }

        void CheckForInput()
        {
            if (!isQuitStatus)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    TryMove(Vector3.up);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    TryMove(Vector3.down);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    TryMove(Vector3.left);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    TryMove(Vector3.right);
                }
                // esc키를 눌렀을 때 quit dialog가 나타나도록 설정
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _quitDialog.SetActive(true);
                    isQuitStatus = true;
                }
            }
            
            else
            {
                // Y를 입력하면 게임 종료
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    GameExit();
                }
                // N을 입력하면 다시 게임 화면으로 돌아가기
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    isQuitStatus = false;
                    _quitDialog.SetActive(false);
                }
            }
        }

        void GameExit()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        void TryMove(Vector3 direction)
        {
            UpdateAnimation(direction, false);

            float colliderHalfSize = GetComponent<Collider2D>().bounds.extents.x;
            Vector3 raycastStartPoint = transform.position + direction * colliderHalfSize;

            // Raycast 거리에 작은 값을 추가하여 조금 더 멀리 검사
            float adjustedStepSize = stepSize - 0.1f; // 예: 1.1

            int layerMask = 1 << LayerMask.NameToLayer("Player");
            layerMask = ~layerMask;

            RaycastHit2D hit = Physics2D.Raycast(raycastStartPoint, direction, adjustedStepSize, layerMask);
            if (hit.collider == null)
            {
                SetTargetPosition(direction);
                // Update the animation for movement
                UpdateAnimation(direction, true);
            }
            else // 뭔가에 부딪힘. enemy인 경우 battlescene call.
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    Debug.Log("Encountered Enemy: " + hit.collider.gameObject.name);

                    // 부딪혔을 때의 위치 정보를 저장
                    GameManager.Instance.UpdatePlayerLoc(targetPosition);

                    // Set the current enemy in GameManager
                    Enemy enemyData = hit.collider.gameObject.GetComponent<Enemy>();
                    if (enemyData != null)
                    {
                        GameManager.Instance.CurrentEnemyID = enemyData.enemyID;
                        Debug.Log("Enemy set in GameManager: " + enemyData.name);
                    }
                    else
                    {
                        Debug.Log("enemyData == null");
                    }

                    // 전투씬으로 전환
                    TransitionManager.Instance().Transition("BattleScene", transition, loadDelay);
                }
                else if (hit.collider.CompareTag("Box"))
                {
                    SceneManager.LoadScene("CompleteScene");
                }
                else
                {
                    float distance = Vector3.Distance(raycastStartPoint, hit.point);
                    Debug.Log("Distance to block: " + distance);
                    Debug.Log("Blocked by " + hit.collider.gameObject.name);
                }
            }
        }

        void SetTargetPosition(Vector3 direction)
        {
            targetPosition = transform.position + direction * stepSize;
            isMoving = true;
        }

        void UpdateAnimation(Vector3 direction, bool moving)
        {
            animator.SetBool("IsMoving", moving);
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);

            if (direction != Vector3.zero)
            {
                animator.SetFloat("LastMoveX", direction.x);
                animator.SetFloat("LastMoveY", direction.y);
            }
        }

        void MoveTowardsTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                transform.position = targetPosition;
                isMoving = false;
                UpdateAnimation(Vector3.zero, false); // Update animation to stop walking
            }
        }
    }
}
