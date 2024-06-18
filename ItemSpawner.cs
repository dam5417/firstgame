using UnityEngine;
using System.Collections;

namespace MyGame
{
    public class ItemSpawner : MonoBehaviour
    {
        public GameObject itemPrefab; // 아이템 프리팹
        public float spawnRadius = 20f; // 스폰 반경
        public float spawnInterval = 2f; // 스폰 간격
        public int maxItems = 10; // 최대 아이템 개수
        public float maxYPosition = -24f; // 최대 Y 위치
        private GameObject player; // 플레이어 오브젝트
        private int itemSpawnCount = 0; // 스폰된 아이템 개수

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(SpawnItemRoutine());
        }

        // 일정 간격으로 아이템 스폰
        private IEnumerator SpawnItemRoutine()
        {
            while (itemSpawnCount < maxItems)
            {
                SpawnItem();
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void SpawnItem()
        {
            // 플레이어가 없거나 최대 아이템 개수 초과하면 스폰 안함.
            if (player == null || itemSpawnCount >= maxItems) return;

            // 스폰 위치 계산
            Vector3 spawnPosition = transform.position + Vector3.down * UnityEngine.Random.Range(10f, 20f);
            Vector3 randomPosition = spawnPosition + UnityEngine.Random.insideUnitSphere * spawnRadius;
            randomPosition.y = Mathf.Clamp(randomPosition.y, float.MinValue, maxYPosition);

            // 아이템 생성
            Instantiate(itemPrefab, randomPosition, Quaternion.identity);
            itemSpawnCount++;

            // 최대 아이템 개수에 도달하면 스폰 루틴 중지
            if (itemSpawnCount >= maxItems)
            {
                StopCoroutine(SpawnItemRoutine());
            }
        }
    }
}