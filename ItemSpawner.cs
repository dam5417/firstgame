using UnityEngine;
using System.Collections;

namespace MyGame
{
    public class ItemSpawner : MonoBehaviour
    {
        public GameObject itemPrefab; // ������ ������
        public float spawnRadius = 20f; // ���� �ݰ�
        public float spawnInterval = 2f; // ���� ����
        public int maxItems = 10; // �ִ� ������ ����
        public float maxYPosition = -24f; // �ִ� Y ��ġ
        private GameObject player; // �÷��̾� ������Ʈ
        private int itemSpawnCount = 0; // ������ ������ ����

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(SpawnItemRoutine());
        }

        // ���� �������� ������ ����
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
            // �÷��̾ ���ų� �ִ� ������ ���� �ʰ��ϸ� ���� ����.
            if (player == null || itemSpawnCount >= maxItems) return;

            // ���� ��ġ ���
            Vector3 spawnPosition = transform.position + Vector3.down * UnityEngine.Random.Range(10f, 20f);
            Vector3 randomPosition = spawnPosition + UnityEngine.Random.insideUnitSphere * spawnRadius;
            randomPosition.y = Mathf.Clamp(randomPosition.y, float.MinValue, maxYPosition);

            // ������ ����
            Instantiate(itemPrefab, randomPosition, Quaternion.identity);
            itemSpawnCount++;

            // �ִ� ������ ������ �����ϸ� ���� ��ƾ ����
            if (itemSpawnCount >= maxItems)
            {
                StopCoroutine(SpawnItemRoutine());
            }
        }
    }
}