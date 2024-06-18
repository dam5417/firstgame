using UnityEngine;

namespace MyGame
{
    public class Player : MonoBehaviour
    {
        private Rigidbody2D rb2D;
        public float initialMoveSpeed = 5f; // �ʱ� �ӵ�
        public float moveSpeed; // ���� �ӵ�
        public bool isDIE = false; // ��� ����

        void Start()
        {
            InitializePlayer(); // �÷��̾� �ʱ�ȭ
        }

        void Update()
        {
            HandleMovementInput(); // ������ �Է� ó��
        }

        // �÷��̾� �ʱ�ȭ
        void InitializePlayer()
        {
            rb2D = GetComponent<Rigidbody2D>();
            rb2D.gravityScale = 0.3f; // �߷� ����
            moveSpeed = initialMoveSpeed; // �ʱ� �ӵ� ����
        }

        // ������ �Է� ó��
        void HandleMovementInput()
        {
            if (isDIE) // ��� ������ ��� �������� ����
                return;

            float horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput != 0)
            {
                Move(horizontalInput); // ������ ����
                Flip(horizontalInput); // �¿� ���� ����
            }
        }

        // �÷��̾� �̵�
        void Move(float horizontalInput)
        {
            Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb2D.velocity.y);
            rb2D.velocity = movement;
        }

        // �浹 ó��
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Stairs")) // ��ܰ� �浿�� ���
            {
                UnityEngine.Debug.Log("���� ���� - ��ܿ� �ε��� ���");
                GameManager.Instance.PlayerDead(); // �÷��̾� ��� ó��
                Die(); // ��� ó��
            }
            else if (collision.CompareTag("Ground")) // ���� �浹�� ���
            {
                UnityEngine.Debug.Log("���� �浹");
            }
        }

        // �¿� ���� ó��
        void Flip(float horizontalInput)
        {
            GetComponent<SpriteRenderer>().flipX = horizontalInput < 0;
        }

        // ���� �����
        public void RestartGame(Vector3 startPosition)
        {
            Time.timeScale = 1f;
            if (gameObject != null)
            {
                transform.position = startPosition; // ���� ��ġ�� �̵�
                rb2D.velocity = new Vector2(moveSpeed, rb2D.velocity.y); // �ʱ� �ӵ��� ����
                moveSpeed = initialMoveSpeed; // �ʱ� �ӵ��� ����
            }
        }

        // ��� ó��
        public void Die()
        {
            isDIE = true;
        }
    }
}
