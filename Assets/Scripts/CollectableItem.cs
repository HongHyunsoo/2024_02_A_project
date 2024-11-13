using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public ItemType itemType;           //������ ����
    public string itemName;             //������ �̸�
    public float respawnTime = 30.0f;   //������ �ð�
    public bool canCollect = true;      //���� ���� ����(������ �� �ִ��� ����)

    //�������� �����ϴ� �޼���, PlayerInventory�� ���� �κ��丮�� �߰�

    public void CollectItem(PlayerInventory inventory)
    {
        //���� ���� ���θ� üũ
        if (!canCollect) return;

       
        inventory.AddItem(itemType);            //�������� �κ��丮�� �߰�

        if(FloatingTextManager.instance != null)
        {
            Vector3 textPosition = transform.position + Vector3.up * 0.5f;
            FloatingTextManager.instance.Show($" + {itemName}", textPosition);      //�������� ��ġ���� �ణ ���� �ؽ�Ʈ ����
        }


        Debug.Log($"{itemName} �����Ϸ�");      //������ �����Ϸ� �޼��� ���
        StartCoroutine(RespawnRoutine());
    }

    //�������� ������ ó��
    private IEnumerator RespawnRoutine()
    {
        canCollect = false;                             //���� �Ұ��� ���·� ����
        GetComponent<MeshRenderer>().enabled = false;   //�������� MeshRenderer�� ���� ������ �ʰ�
        GetComponent<MeshCollider>().enabled = false;

        yield return new WaitForSeconds(respawnTime);

        GetComponent<MeshRenderer>().enabled = true;    //�������� �ٽ� ���̰�
        GetComponent<MeshCollider>().enabled = true;
        canCollect = true;                              //���� �Ұ��� ���·� ����


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
