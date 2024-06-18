using UnityEngine;

namespace MyGame
{
    public class Item : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // �÷��̾�� �浹���� �� ������ ȹ���ϰ� ���� ����
                GameManager.Instance.AddScore(10);
                UnityEngine.Debug.Log("�������� ȹ���Ͽ����ϴ�. ���� ����: " + GameManager.Instance.GetNowScore());
                Destroy(gameObject); // ������ ����
            }
        }
    }
}
