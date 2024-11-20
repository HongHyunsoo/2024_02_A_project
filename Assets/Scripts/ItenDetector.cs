using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//아이템 종류 정리

public enum ItemType
{
    Crystal,        //크리스탈 
    Plant,          //식물
    Bush,           //수풀
    Tree,           //나무
    VegetableStew, //야채 스튜 (허기 회복용)
    FruitSalad,    //과일 샐러드 (허기 회복용)
    RepairKit      //수리 키트 (우주복 수리용)
}
public class ItenDetector : MonoBehaviour
{
    public float checkRadius = 3.0f;        //아이템 감지 범위
    public Vector3 lastPosition;            //플레이어의 마지막 위치
    public float moveThresthold = 0.1f;             //이동 감지 임계값
    public CollectableItem currentNearbyItem;       //가장 가까이 있는 수집 가능한 아이템




    void Start()
    {
        lastPosition = transform.position;      //시작 시 현재 위치를 마지막 위치로 설정
        CheckForItems();                        //초기 아이템 체크 수행
    }

    void Update()
    {
        //플레이어가 일정 거리 이상 이동하였는지 체크
        if (Vector3.Distance(lastPosition, transform.position) > moveThresthold)
        {
            CheckForItems();
            lastPosition = transform.position;
        }

        //가까운 아이템이 있고 E키를 눌렀을 때 아이템 수집
        if (currentNearbyItem != null && Input.GetKeyDown(KeyCode.E))
        {
            currentNearbyItem.CollectItem(GetComponent<PlayerInventory>());
        }

       
    }
    //주변에 수집 가능한 아이템을 감지하는 함수

    private void CheckForItems()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);      //감지 범위 내의 모든 콜라이더를 찾아옴

        float closestDistance = float.MaxValue;     //가장 가까운 거리의 초기값
        CollectableItem closestItem = null;         //가장 가까운 아이템 초기값

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
        if (closestItem != currentNearbyItem)       //가장 가까운 아이템이 변경되었을 때 메세지 표시
        {
            currentNearbyItem = closestItem;        //가장 가까운 아이템 업데이트
            if (currentNearbyItem != null)
            {
                Debug.Log($" [E] 키를 눌러 {currentNearbyItem.itemName} 수집하세요");     //새로운 아이템 수집 메세지 표시
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, checkRadius);

    }
}
