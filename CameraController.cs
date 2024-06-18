using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    // 플레이어와 카메라 사이의 오프셋(거리 차이)
    private Vector3 offset;

    void Start()
    {
        //초기 오프셋 계산
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // 카메라를 플레이어 위치에 이동시키고 오프셋 더함
        transform.position = player.position + offset;
    }
}
