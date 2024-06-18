using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace MyGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        // Stairs ���� ����
        public GameObject stairPrefab;  // ����� ��� ������
        public Transform spawnPoint;    // ��� ���� ��ġ
        public float minXPosition = -3f;    // ����� ������ X �ּ� ��ġ
        public float maxXPosition = 3f; // ����� ������ X �ִ� ��ġ
        public float minDistance = 10f; // ��� �� �ּ� ����
        public float maxDistance = 13f; // ��� �� �ִ� ����
        public int poolSize = 30;   // ��� Ǯ ������
        private List<GameObject> stairsPool;  // ����� ������ Ǯ

        private Vector3 oldPosition;   // ���� ����� ��ġ
        public float moveSpeed = 0.5f; // ��� �̵� �ӵ�
        private GameObject player; // �÷��̾� ���� ������Ʈ

        // UI ���� ����
        public GameObject UI_GameOver; // ���� ���� UI
        public TextMeshProUGUI textMaxScore;   // �ְ� ���� UI
        public TextMeshProUGUI textNowScore;   // ���� ���� UI
        public TextMeshProUGUI textShowScore;  // ���� ǥ�� UI
        private int maxScore = 0;   // �ְ� ����
        private int nowScore = 0;   // ���� ����

        // �ð� ����� ���� ���� ���� ����
        public float scoreIncreaseInterval = 0.1f;

        void Start()
        {
            Instance = this;
            Init(); // �ʱ�ȭ
            StartCoroutine(SpawnInfiniteStairs()); // ���� ��� ����
            player = GameObject.FindGameObjectWithTag("Player"); // �÷��̾� ���� ������Ʈ Ž��

            // ������ �������� ������ ������Ű�� �ڷ�ƾ ����
            StartCoroutine(IncreaseScoreOverTime());
        }

        // ���� �Ŵ��� �ʱ�ȭ
        private void Init()
        {
            oldPosition = Vector3.zero;

            // ��� Ǯ �ʱ�ȭ
            stairsPool = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(stairPrefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                obj.transform.parent = transform;
                stairsPool.Add(obj);
            }

            // ���� �ʱ�ȭ
            nowScore = 0;
            if (textShowScore != null)
            {
                textShowScore.text = nowScore.ToString();
            }

            // ���� ���� UI �ʱ�ȭ
            if (UI_GameOver != null)
            {
                UI_GameOver.SetActive(false);
            }
        }

        // ���� ��� ����
        private IEnumerator SpawnInfiniteStairs()
        {
            while (true)
            {
                SpawnStair();
                yield return new WaitForSeconds(0.01f);
            }
        }

        // ��� ����
        private void SpawnStair()
        {
            GameObject newStair = GetAvailableStairFromPool();

            // ����� ��ġ ����
            newStair.transform.position = spawnPoint.position;
            newStair.SetActive(true);

            // ����� X ��ġ ���� ����
            float randomX = UnityEngine.Random.Range(minXPosition, maxXPosition);
            float randomY = UnityEngine.Random.Range(-23.59f, spawnPoint.position.y);
            newStair.transform.position = new Vector3(randomX, randomY, newStair.transform.position.z);

            if (oldPosition != Vector3.zero)
            {
                // ���� ��� ��ġ���� ������ ���� ������ ��ġ�� �̵�
                float distance = UnityEngine.Random.Range(minDistance, maxDistance);
                float newY = oldPosition.y - distance;
                newStair.transform.position = new Vector3(newStair.transform.position.x, newY, newStair.transform.position.z);
            }

            oldPosition = newStair.transform.position;

            // �÷��̾ ��ܺ��� ���� ������ �ٽ� Ǯ�� ��ȯ
            if (player != null && newStair.transform.position.y > player.transform.position.y)
            {
                ReturnStairToPool(newStair);
            }

            StartCoroutine(DeactivateStairAfterTime(newStair, 10.0f)); // ���� �ð��� ���� �� ��� ��Ȱ��ȭ
        }

        // Ǯ���� ��� ������ ��� ��������
        private GameObject GetAvailableStairFromPool()
        {
            foreach (GameObject stair in stairsPool)
            {
                if (!stair.activeInHierarchy)
                {
                    return stair;
                }
            }

            // �� ��� ����
            GameObject newStair = Instantiate(stairPrefab, Vector3.zero, Quaternion.identity);
            newStair.transform.parent = transform;
            stairsPool.Add(newStair);
            newStair.SetActive(false);
            return newStair;
        }

        // ����� Ǯ�� ��ȯ
        private void ReturnStairToPool(GameObject stair)
        {
            stair.SetActive(false);
            stair.transform.position = Vector3.zero;
        }

        // ���� �ð��� ���� �� ��� ��Ȱ��ȭ
        private IEnumerator DeactivateStairAfterTime(GameObject stair, float delay)
        {
            yield return new WaitForSeconds(delay);
            stair.SetActive(false);
            stair.transform.position = Vector3.zero;
        }

        // �÷��̾� ��� ó��
        public void PlayerDead()
        {
            Time.timeScale = 0f;
            GameOver();
        }

        // ���� �߰�
        public void AddScore(int scoreToAdd)
        {
            nowScore += scoreToAdd;
            if (textShowScore != null)
            {
                textShowScore.text = nowScore.ToString();
            }
            UnityEngine.Debug.Log("Score Updated: " + nowScore);
        }

        // �ð� ����� ���� ���� ����
        private IEnumerator IncreaseScoreOverTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(scoreIncreaseInterval);
                AddScore(15); // ���� �ð����� ���� ����
            }
        }

        // ���� ���� ��ȯ
        public int GetNowScore()
        {
            return nowScore;
        }

        // ���� ���� ó��
        public void GameOver()
        {
            StartCoroutine(ShowGameOver());
        }

        // ���� ���� UI ǥ��
        private IEnumerator ShowGameOver()
        {
            yield return new WaitForSecondsRealtime(1f);

            if (UI_GameOver != null)
            {
                UI_GameOver.SetActive(true);
            }

            if (nowScore > maxScore)
            {
                maxScore = nowScore;
            }

            if (textMaxScore != null)
            {
                textMaxScore.text = maxScore.ToString();
            }

            if (textNowScore != null)
            {
                textNowScore.text = nowScore.ToString();
            }
        }

        // ���� �����
        public void RestartGame(Vector3 startPosition)
        {
            Time.timeScale = 1f;
            Init(); // ���� �ʱ�ȭ

            // ĳ������ ��ġ�� ������ ��ġ�� ����
            if (player != null)
            {
                player.transform.position = startPosition;
            }
        }

        // ����ŸƮ ��ư Ŭ�� �̺�Ʈ �ڵ鷯
        public void RestartButtonOnClick()
        {
            Vector3 startPosition = new Vector3(-1.18f, 2.54f, 0f); // ĳ������ �ʱ� ��ġ
            RestartGame(startPosition);
        }
    }
}