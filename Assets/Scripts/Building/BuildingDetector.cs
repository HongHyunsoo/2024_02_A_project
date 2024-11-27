using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BuildingDetector : MonoBehaviour
{
    public float checkRadius = 3.0f;        //������ ���� ����
    public Vector3 lastPosition;            //�÷��̾��� ������ ��ġ
    public float moveThresthold = 0.1f;             //�̵� ���� �Ӱ谪
    public ConstructibleBuilding currentNearbyBuilding;       //���� ������ �ִ� �ǹ�
    public BuildingCraftor currentBuildingCrafter;



    void Start()
    {
        lastPosition = transform.position;      //���� �� ���� ��ġ�� ������ ��ġ�� ����
        CheckForBuilding();                        //�ʱ� �ǹ� üũ ����
    }

    // Update is called once per frame
    void Update()
    {
        //�÷��̾ ���� �Ÿ� �̻� �̵��Ͽ����� üũ
        if (Vector3.Distance(lastPosition, transform.position) > moveThresthold)
        {
            CheckForBuilding();                 //�̵� �� �ǹ� üũ
            lastPosition = transform.position;  //���� ��ġ�� ������ ��ġ�� ������Ʈ
        }

        //����� �ǹ��� �ְ� FŰ�� ������ �� �ǹ� �Ǽ�
        if (currentNearbyBuilding != null && Input.GetKeyDown(KeyCode.F))
        {
            if (!currentNearbyBuilding.isConstructed)
            {
                currentNearbyBuilding.StartConstriction(GetComponent<PlayerInventory>());       //  player Inventory �����Ͽ� �ǹ� �Ǽ�
            }
           else if(currentBuildingCrafter != null)  //ũ�����Ͱ� ���� ���
            {
                Debug.Log($"{currentNearbyBuilding.buildingName}�� ���� �޴� ����");
                CraftingUIManager.Instance?.ShowUI(currentBuildingCrafter);     //UI �г��� ����
            }
        }


    }

    private void CheckForBuilding()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);      //���� ���� ���� ��� �ݶ��̴��� ã�ƿ�

        float closestDistance = float.MaxValue;     //���� ����� �Ÿ��� �ʱⰪ
        ConstructibleBuilding closestBuilding = null;         //���� ����� �ǹ� �ʱⰪ
        BuildingCraftor closesCrafter = null;

        foreach (Collider collider in hitColliders)
        {
            ConstructibleBuilding building = collider.GetComponent<ConstructibleBuilding>();
            if (building != null) //��� �ǹ� ������ ����
            {
                float distance = Vector3.Distance(transform.position, building.transform.position);
                if (distance < closestDistance)     //�� ����� �ǹ� �߰� �� ������Ʈ
                {
                    closestDistance = distance;
                    closestBuilding = building;
                    closesCrafter = building.GetComponent<BuildingCraftor>();       //���⼭ ũ������ ��������
                }
            }
        }
        if (closestBuilding != currentNearbyBuilding)       //���� ����� �ǹ��� ����Ǿ��� �� �޼��� ǥ��
        {
            currentNearbyBuilding = closestBuilding;        //���� ����� �ǹ� ������Ʈ
            currentBuildingCrafter = closesCrafter;

            if (currentNearbyBuilding != null && !currentNearbyBuilding.isConstructed)
            {
                if (FloatingTextManager.instance != null)
                {
                    Vector3 textPosition = transform.position + Vector3.up * 0.5f;
                    FloatingTextManager.instance.Show
                        ($"{currentNearbyBuilding.buildingName} �Ǽ� (���� {currentNearbyBuilding.requiredTree}�� �ʿ�)", 
                        currentNearbyBuilding.transform.position + Vector3.up);
                }
            }
        }
    }
}
