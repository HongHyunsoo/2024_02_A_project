using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//������ ���� ����

public enum ItemType
{
    Crystal,        //ũ����Ż 
    Plant,          //�Ĺ�
    Bush,           //��Ǯ
    Tree,           //����
    VegetableStew, //��ä ��Ʃ (��� ȸ����)
    FruitSalad,    //���� ������ (��� ȸ����)
    RepairKit      //���� ŰƮ (���ֺ� ������)
}
public class ItenDetector : MonoBehaviour
{
    public float checkRadius = 3.0f;        //������ ���� ����
    public Vector3 lastPosition;            //�÷��̾��� ������ ��ġ
    public float moveThresthold = 0.1f;             //�̵� ���� �Ӱ谪
    public CollectableItem currentNearbyItem;       //���� ������ �ִ� ���� ������ ������




    void Start()
    {
        lastPosition = transform.position;      //���� �� ���� ��ġ�� ������ ��ġ�� ����
        CheckForItems();                        //�ʱ� ������ üũ ����
    }

    void Update()
    {
        //�÷��̾ ���� �Ÿ� �̻� �̵��Ͽ����� üũ
        if (Vector3.Distance(lastPosition, transform.position) > moveThresthold)
        {
            CheckForItems();
            lastPosition = transform.position;
        }

        //����� �������� �ְ� EŰ�� ������ �� ������ ����
        if (currentNearbyItem != null && Input.GetKeyDown(KeyCode.E))
        {
            currentNearbyItem.CollectItem(GetComponent<PlayerInventory>());
        }

       
    }
    //�ֺ��� ���� ������ �������� �����ϴ� �Լ�

    private void CheckForItems()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);      //���� ���� ���� ��� �ݶ��̴��� ã�ƿ�

        float closestDistance = float.MaxValue;     //���� ����� �Ÿ��� �ʱⰪ
        CollectableItem closestItem = null;         //���� ����� ������ �ʱⰪ

        foreach (Collider collider in hitColliders)
        {
            CollectableItem item = collider.GetComponent<CollectableItem>();
            if (item != null && item.canCollect)
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = item;
                }
            }
        }
        if (closestItem != currentNearbyItem)       //���� ����� �������� ����Ǿ��� �� �޼��� ǥ��
        {
            currentNearbyItem = closestItem;        //���� ����� ������ ������Ʈ
            if (currentNearbyItem != null)
            {
                Debug.Log($" [E] Ű�� ���� {currentNearbyItem.itemName} �����ϼ���");     //���ο� ������ ���� �޼��� ǥ��
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, checkRadius);

    }
}
