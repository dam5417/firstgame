using UnityEngine;

namespace MyGame
{
    public class Item : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // 플레이어와 충돌했을 때 아이템 획득하고 점수 증가
                GameManager.Instance.AddScore(10);
                UnityEngine.Debug.Log("아이템을 획득하였습니다. 현재 점수: " + GameManager.Instance.GetNowScore());
                Destroy(gameObject); // 아이템 제거
            }
        }
    }
}
