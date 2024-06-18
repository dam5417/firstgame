using UnityEngine;

namespace MyGame
{
    public class Player : MonoBehaviour
    {
        private Rigidbody2D rb2D;
        public float initialMoveSpeed = 5f; // 초기 속도
        public float moveSpeed; // 현재 속도
        public bool isDIE = false; // 사망 여부

        void Start()
        {
            InitializePlayer(); // 플레이어 초기화
        }

        void Update()
        {
            HandleMovementInput(); // 움직임 입력 처리
        }

        // 플레이어 초기화
        void InitializePlayer()
        {
            rb2D = GetComponent<Rigidbody2D>();
            rb2D.gravityScale = 0.3f; // 중력 설정
            moveSpeed = initialMoveSpeed; // 초기 속도 설정
        }

        // 움직임 입력 처리
        void HandleMovementInput()
        {
            if (isDIE) // 사망 상태일 경우 움직이지 않음
                return;

            float horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput != 0)
            {
                Move(horizontalInput); // 움직임 적용
                Flip(horizontalInput); // 좌우 반전 적용
            }
        }

        // 플레이어 이동
        void Move(float horizontalInput)
        {
            Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb2D.velocity.y);
            rb2D.velocity = movement;
        }

        // 충돌 처리
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Stairs")) // 계단과 충동할 경우
            {
                UnityEngine.Debug.Log("게임 종료 - 계단에 부딪혀 사망");
                GameManager.Instance.PlayerDead(); // 플레이어 사망 처리
                Die(); // 사망 처리
            }
            else if (collision.CompareTag("Ground")) // 땅과 충돌할 경우
            {
                UnityEngine.Debug.Log("땅과 충돌");
            }
        }

        // 좌우 반전 처리
        void Flip(float horizontalInput)
        {
            GetComponent<SpriteRenderer>().flipX = horizontalInput < 0;
        }

        // 게임 재시작
        public void RestartGame(Vector3 startPosition)
        {
            Time.timeScale = 1f;
            if (gameObject != null)
            {
                transform.position = startPosition; // 시작 위치로 이동
                rb2D.velocity = new Vector2(moveSpeed, rb2D.velocity.y); // 초기 속도로 설정
                moveSpeed = initialMoveSpeed; // 초기 속도로 변경
            }
        }

        // 사망 처리
        public void Die()
        {
            isDIE = true;
        }
    }
}
