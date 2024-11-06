using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //���� ������ ������ �����ϴ� ����
    public int crystalCount = 0;        //ũ����Ż ����
    public int plantCount = 0;          //�Ĺ� ����
    public int bushCount = 0;           //��Ǯ ����
    public int treeCount = 0;           //���� ����
    
    //�������� �߰��ϴ� �Լ�, ������ ������ ���� �ش� �������� ������ ���� ��Ŵ
    public void AddItem(ItemType itemType)
    {
        //������ ������ ���� �ٸ� ���� ����
        switch(itemType)
        {
            case ItemType.Crystal:
                crystalCount++;     //ũ����Ż ���� ����
                Debug.Log($"ũ����Ż ȹ�� ! ���� ���� : {crystalCount}");  //���� ũ����Ż ���� �Է�
                break;
            case ItemType.Plant:
                plantCount++;     //�Ĺ� ���� ����
                Debug.Log($"�Ĺ� ȹ�� ! ���� ���� : {plantCount}");  //���� �Ĺ� ���� �Է�
                break;
            case ItemType.Bush:
                bushCount++;     //��Ǯ ���� ����
                Debug.Log($"��Ǯ ȹ�� ! ���� ���� : {bushCount}");  //���� ��Ǯ ���� �Է�
                break;
            case ItemType.Tree:
                treeCount++;     //���� ���� ����
                Debug.Log($"���� ȹ�� ! ���� ���� : {treeCount}");  //���� ���� ���� �Է�
                break;
        }
    }
    

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            ShowInventory();
        }
    }

    private void ShowInventory()
    {
        Debug.Log("========�κ��丮========");
        Debug.Log($"ũ����Ż: {crystalCount}��");        //ũ����Ż ���� ���
        Debug.Log($"�Ĺ�: {plantCount}��");              //�Ĺ� ���� ���
        Debug.Log($"��Ǯ: {bushCount}��");               //��Ǯ ���� ���
        Debug.Log($"����: {treeCount}��");               //���� ���� ���
        Debug.Log("========================");
    }
}
