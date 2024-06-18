using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    // �÷��̾�� ī�޶� ������ ������(�Ÿ� ����)
    private Vector3 offset;

    void Start()
    {
        //�ʱ� ������ ���
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // ī�޶� �÷��̾� ��ġ�� �̵���Ű�� ������ ����
        transform.position = player.position + offset;
    }
}
