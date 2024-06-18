using UnityEngine;

namespace MyGame
{
    public class BackGroundCamera : MonoBehaviour
    {
        // 카메라가 따라갈 플레이어
        public Transform player;

        void LateUpdate()
        {
            if (player != null)
            {
                // 플레이어 위치에 따라 카메라 위치 업데이트
                Vector3 newPosition = player.position;
                newPosition.z = transform.position.z; // 카메라 Z축 위치 유지
                transform.position = newPosition; // 카메라 위치 업데이트
            }
        }
    }
}
