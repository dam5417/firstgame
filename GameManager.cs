using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace MyGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        // Stairs 관련 변수
        public GameObject stairPrefab;  // 사용할 계단 프리팹
        public Transform spawnPoint;    // 계단 생성 위치
        public float minXPosition = -3f;    // 계단이 생성될 X 최소 위치
        public float maxXPosition = 3f; // 계단이 생성될 X 최대 위치
        public float minDistance = 10f; // 계단 간 최소 간격
        public float maxDistance = 13f; // 계단 간 최대 간격
        public int poolSize = 30;   // 계단 풀 사이즈
        private List<GameObject> stairsPool;  // 계단을 관리할 풀

        private Vector3 oldPosition;   // 이전 계단의 위치
        public float moveSpeed = 0.5f; // 계단 이동 속도
        private GameObject player; // 플레이어 게임 오브젝트

        // UI 관련 변수
        public GameObject UI_GameOver; // 게임 오버 UI
        public TextMeshProUGUI textMaxScore;   // 최고 점수 UI
        public TextMeshProUGUI textNowScore;   // 현재 점수 UI
        public TextMeshProUGUI textShowScore;  // 점수 표시 UI
        private int maxScore = 0;   // 최고 점수
        private int nowScore = 0;   // 현재 점수

        // 시간 경과에 따른 점수 증가 간격
        public float scoreIncreaseInterval = 0.1f;

        void Start()
        {
            Instance = this;
            Init(); // 초기화
            StartCoroutine(SpawnInfiniteStairs()); // 무한 계단 생성
            player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 게임 오브젝트 탐색

            // 일정한 간격으로 점수를 증가시키는 코루틴 시작
            StartCoroutine(IncreaseScoreOverTime());
        }

        // 게임 매니저 초기화
        private void Init()
        {
            oldPosition = Vector3.zero;

            // 계단 풀 초기화
            stairsPool = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(stairPrefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                obj.transform.parent = transform;
                stairsPool.Add(obj);
            }

            // 점수 초기화
            nowScore = 0;
            if (textShowScore != null)
            {
                textShowScore.text = nowScore.ToString();
            }

            // 게임 오버 UI 초기화
            if (UI_GameOver != null)
            {
                UI_GameOver.SetActive(false);
            }
        }

        // 무한 계단 생성
        private IEnumerator SpawnInfiniteStairs()
        {
            while (true)
            {
                SpawnStair();
                yield return new WaitForSeconds(0.01f);
            }
        }

        // 계단 생성
        private void SpawnStair()
        {
            GameObject newStair = GetAvailableStairFromPool();

            // 계단의 위치 설정
            newStair.transform.position = spawnPoint.position;
            newStair.SetActive(true);

            // 계단의 X 위치 랜덤 설정
            float randomX = UnityEngine.Random.Range(minXPosition, maxXPosition);
            float randomY = UnityEngine.Random.Range(-23.59f, spawnPoint.position.y);
            newStair.transform.position = new Vector3(randomX, randomY, newStair.transform.position.z);

            if (oldPosition != Vector3.zero)
            {
                // 이전 계단 위치에서 일정한 간격 떨어진 위치로 이동
                float distance = UnityEngine.Random.Range(minDistance, maxDistance);
                float newY = oldPosition.y - distance;
                newStair.transform.position = new Vector3(newStair.transform.position.x, newY, newStair.transform.position.z);
            }

            oldPosition = newStair.transform.position;

            // 플레이어가 계단보다 위에 있으면 다시 풀로 반환
            if (player != null && newStair.transform.position.y > player.transform.position.y)
            {
                ReturnStairToPool(newStair);
            }

            StartCoroutine(DeactivateStairAfterTime(newStair, 10.0f)); // 일정 시간이 지난 후 계단 비활성화
        }

        // 풀에서 사용 가능한 계단 가져오기
        private GameObject GetAvailableStairFromPool()
        {
            foreach (GameObject stair in stairsPool)
            {
                if (!stair.activeInHierarchy)
                {
                    return stair;
                }
            }

            // 새 계단 생성
            GameObject newStair = Instantiate(stairPrefab, Vector3.zero, Quaternion.identity);
            newStair.transform.parent = transform;
            stairsPool.Add(newStair);
            newStair.SetActive(false);
            return newStair;
        }

        // 계단을 풀로 반환
        private void ReturnStairToPool(GameObject stair)
        {
            stair.SetActive(false);
            stair.transform.position = Vector3.zero;
        }

        // 일정 시간이 지난 후 계단 비활성화
        private IEnumerator DeactivateStairAfterTime(GameObject stair, float delay)
        {
            yield return new WaitForSeconds(delay);
            stair.SetActive(false);
            stair.transform.position = Vector3.zero;
        }

        // 플레이어 사망 처리
        public void PlayerDead()
        {
            Time.timeScale = 0f;
            GameOver();
        }

        // 점수 추가
        public void AddScore(int scoreToAdd)
        {
            nowScore += scoreToAdd;
            if (textShowScore != null)
            {
                textShowScore.text = nowScore.ToString();
            }
            UnityEngine.Debug.Log("Score Updated: " + nowScore);
        }

        // 시간 경과에 따른 점수 증가
        private IEnumerator IncreaseScoreOverTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(scoreIncreaseInterval);
                AddScore(15); // 일정 시간마다 점수 증가
            }
        }

        // 현재 점수 반환
        public int GetNowScore()
        {
            return nowScore;
        }

        // 게임 오버 처리
        public void GameOver()
        {
            StartCoroutine(ShowGameOver());
        }

        // 게임 오버 UI 표시
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

        // 게임 재시작
        public void RestartGame(Vector3 startPosition)
        {
            Time.timeScale = 1f;
            Init(); // 게임 초기화

            // 캐릭터의 위치를 지정된 위치로 설정
            if (player != null)
            {
                player.transform.position = startPosition;
            }
        }

        // 리스타트 버튼 클릭 이벤트 핸들러
        public void RestartButtonOnClick()
        {
            Vector3 startPosition = new Vector3(-1.18f, 2.54f, 0f); // 캐릭터의 초기 위치
            RestartGame(startPosition);
        }
    }
}