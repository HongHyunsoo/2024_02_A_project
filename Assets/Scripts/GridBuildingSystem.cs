using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


//GridCell 클래스는 각 그리드 셀의 상태와 데이터를 관리합니다.
public class GridCell
{
    public Vector3Int Position; //셀의 그리드 내 위치
    public bool IsOccupied;     //셀이 건물로 차있는지 여부
    public GameObject Building; //셀에 배치된 건물 객체

    public GridCell(Vector3Int position)        //클레스 이름과 동일한 함수 (생성자) 클래스가 생성될 때 호출
    {
        Position = position;
        IsOccupied = false;
        Building = null;
    }
}

public class GridBuildingSystem : MonoBehaviour
{

    [SerializeField] private int width = 10;    //그리드의 가로 크기
    [SerializeField] private int height = 10;    //그리드의 세로 크기
    [SerializeField] float cellSize = 1.0f;      //각 셀의 크기
    [SerializeField] private GameObject cellPrefabs; //셀 프레팹
    [SerializeField] private GameObject buildingPrefabs; //빌딩 프레팹

    [SerializeField] private PlayerController playerController; //플레이어 컨트롤러 참조

    [SerializeField] private Grid grid;
    private GridCell[,] cells;          //GridCell 클래스를 2차원 배열로 선언
    private Camera firstPersonCamera;


   
   
   
    // Start is called before the first frame update
    void Start()
    {
        firstPersonCamera = playerController.firstPersonCamera;
        CreateGrid();
    }
    
    //그리드의 셀을 Gizmo로 표기하는 메서드

    private void CreateGrid()
    {
        grid.cellSize = new Vector3(cellSize, cellSize, cellSize);

        cells = new GridCell[width, height];

        Vector3 gridCenter = playerController.transform.position;       //플레이어의 위치를 받아와서
        gridCenter.y = 0;
        transform.position = gridCenter - new Vector3(width * cellSize / 2.0f, 0, height * cellSize / 2.0f);    //플레이어 정중앙 기준으로 만든ㄹ
    
        for (int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                Vector3Int cellPosition = new Vector3Int(x, 0, z);  //셀 위치
                Vector3 worldPosition = grid.GetCellCenterWorld(cellPosition);  //그리드 함수를 통해서 월드포지션 위치를 가져온다
                GameObject cellObject = Instantiate(cellPrefabs,worldPosition, cellPrefabs.transform.rotation);
                cellObject.transform.SetParent(transform);

                cells[x, z] = new GridCell(cellPosition);
            }
        }
    
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 lookPosition = GetLookPosition();
        if (lookPosition != Vector3.zero)               //보고 있는 좌표가 있는지 검사
        {
            Vector3Int gridPosition = grid.WorldToCell(lookPosition);       //그리드 월드 포지션 확인
            if (isValidGridPosition(gridPosition))      //위치가 유효한지 확인
            {
                HightlightCell(gridPosition);
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceBuilding(gridPosition);
                }
                if(Input.GetMouseButtonDown(1))
                {
                    RemoveBuilding(gridPosition);
                }
            }
        }
    }
    //그리드 셀에 건물을 배치하는 메서드
    private void PlaceBuilding(Vector3Int gridPosition)
    {
        GridCell cell = cells[gridPosition.x, gridPosition.z];
        if (!cell.IsOccupied)
        {
            Vector3 worldPosition = grid.GetCellCenterWorld(gridPosition);
            GameObject building = Instantiate(buildingPrefabs, worldPosition, Quaternion.identity);//건물을 생성
            cell.IsOccupied = true;             //건물 확인 값
            cell.Building = building;           //Cell 에 놓인 빌딩
        }
    }

    //그리드 셀에서 건물을 제거하는 메서드
    private void RemoveBuilding(Vector3Int gridPosition)
    {
        GridCell cell = cells[gridPosition.x, gridPosition.z];
        if (cell.IsOccupied)
        {
            Destroy(cell.Building);         //해당 Cell 건물 제거
            cell.IsOccupied = false;        //건물 확인 값
            cell.Building = null;           //Cell 에 놓인 빌딩
        }
    }

    //선택된 셀을 하이라이팅
    private void HightlightCell(Vector3Int gridPosition)
    {
        for (int x = 0; x < width; x++)   //Cell 을 돌면서
        {
            for (int z = 0; z < height; z++)
            {
                //건물 없으면 하양
                GameObject cellObject = cells[x, z].Building != null ? cells[x, z].Building : transform.GetChild(x * height + z).gameObject;
                cellObject.GetComponent<Renderer>().material.color = Color.white;
            }
        }

        //특정 셀에 건물이 잇으면 빨간색 아니면 초록색
        GridCell cell = cells[gridPosition.x , gridPosition.z];
        GameObject highlightObject = cell.Building != null ? cell.Building : transform.GetChild(gridPosition.x * height + gridPosition.z).gameObject;
        highlightObject.GetComponent<Renderer>().material.color = cell.IsOccupied ? Color.red : Color.green;
    }

    //그리드 포지션이 유효한지 확인하는 메서드
    private bool isValidGridPosition(Vector3Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < width &&
            gridPosition.z >= 0 && gridPosition.z < height;
    }

    //플레이어가 보고 있는 위치를 계산하는 메서드
    private Vector3 GetLookPosition()
    {
        if (playerController.isFirstPerson)     //만약 1인칭 모드 일 경우
        {
            Ray ray = new Ray(firstPersonCamera.transform.position, firstPersonCamera.transform.forward);   //카메라 앞 방향으로 ray발사
            if(Physics.Raycast(ray, out RaycastHit hitInfo, 5.0f))
            {
                Debug.DrawRay(ray.origin,ray.direction * hitInfo.distance, Color.red );
                return hitInfo.point;
            }
            else
            {
                Debug.DrawRay(ray.origin,ray.direction * 5.0f, Color.white);
            }
        }
        else
        {
            Vector3 charactorPosition = playerController.transform.position;
            Vector3 charactorForward = playerController.transform.forward;
            Vector3 rayOrigin = charactorPosition + Vector3.up * 1.5f + charactorForward * 0.5f;
            Vector3 rayDirection = (charactorForward - Vector3.up).normalized;

            Ray ray = new Ray (rayOrigin,rayDirection);
            if(Physics.Raycast(ray,out RaycastHit hitInfo, 5.0f))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.blue);
                return hitInfo.point;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 5.0f, Color.white);       //Ray정보를 얻어준다
            }
        }

        return Vector3.zero;
    }
    private void OnDrawGizmos()
    { 
        Gizmos.color = Color.blue;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 cellCenter = grid.GetCellCenterWorld(new Vector3Int(x, 0, z));
                Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, 0.1f, cellSize));
            }
        }

    }
}
