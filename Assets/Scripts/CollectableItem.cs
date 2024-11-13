using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public ItemType itemType;           //아이템 종류
    public string itemName;             //아이템 이름
    public float respawnTime = 30.0f;   //리스폰 시간
    public bool canCollect = true;      //수집 가능 여부(수집할 수 있는지 여부)

    //아이템을 수집하는 메서드, PlayerInventory를 통해 인벤토리에 추가

    public void CollectItem(PlayerInventory inventory)
    {
        //수집 가능 여부를 체크
        if (!canCollect) return;

       
        inventory.AddItem(itemType);            //아이템을 인벤토리에 추가

        if(FloatingTextManager.instance != null)
        {
            Vector3 textPosition = transform.position + Vector3.up * 0.5f;
            FloatingTextManager.instance.Show($" + {itemName}", textPosition);      //아이템의 위치보다 약간 위에 텍스트 생성
        }


        Debug.Log($"{itemName} 수집완료");      //아이템 수집완료 메세지 출력
        StartCoroutine(RespawnRoutine());
    }

    //아이템을 리스폰 처리
    private IEnumerator RespawnRoutine()
    {
        canCollect = false;                             //수집 불가능 상태로 변경
        GetComponent<MeshRenderer>().enabled = false;   //아이템의 MeshRenderer를 꺼서 보이지 않게
        GetComponent<MeshCollider>().enabled = false;

        yield return new WaitForSeconds(respawnTime);

        GetComponent<MeshRenderer>().enabled = true;    //아이템을 다시 보이게
        GetComponent<MeshCollider>().enabled = true;
        canCollect = true;                              //수집 불가능 상태로 변경


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
