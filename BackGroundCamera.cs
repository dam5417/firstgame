using UnityEngine;

namespace MyGame
{
    public class BackGroundCamera : MonoBehaviour
    {
        // ī�޶� ���� �÷��̾�
        public Transform player;

        void LateUpdate()
        {
            if (player != null)
            {
                // �÷��̾� ��ġ�� ���� ī�޶� ��ġ ������Ʈ
                Vector3 newPosition = player.position;
                newPosition.z = transform.position.z; // ī�޶� Z�� ��ġ ����
                transform.position = newPosition; // ī�޶� ��ġ ������Ʈ
            }
        }
    }
}
