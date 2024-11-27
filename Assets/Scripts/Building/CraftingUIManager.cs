using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingUIManager : MonoBehaviour
{
    public static CraftingUIManager Instance { get; private set; }  //싱글톤 인스턴스

    [Header("UI References")]
    public GameObject craftingPanel;            //조합 UI 패널
    public TextMeshProUGUI buildingNameText;    //건물 이름 텍스트
    public Transform recipeContainer;           //레시피 버튼이 들어갈 컨테이너
    public Button closeButten;                  //닫기 버튼
    public GameObject recipeButtonPrefabs;      //레시피 버튼 프리펩

    private BuildingCraftor currentCrafter;     //현재 선택된 건물의 제작 시스템

    private void Awake()
    {
        if(Instance == null) Instance = this;       //싱글톤 설정
        else Destroy(gameObject);

        craftingPanel.SetActive(false);
    }

    private void RefreshRecipeList()
    {
        foreach(Transform child in recipeContainer)
        {
            Destroy(child.gameObject);
        }
        if(currentCrafter != null && currentCrafter.recipes != null)        //새 레시피 버튼들 생성
        {
            foreach(CraftingRecipe recipe in currentCrafter.recipes)
            {
                GameObject buttenObj = Instantiate(recipeButtonPrefabs, recipeContainer);
                RecipeButton recipeButten = buttenObj.GetComponent<RecipeButton>();
                recipeButten.Setup(recipe, currentCrafter);
            }
        }
    }

    public void ShowUI(BuildingCraftor crafter)     //UI 표시
    {
        currentCrafter = crafter;
        craftingPanel.SetActive(true);              

        Cursor.visible = true;                      //마우스 커서 표시 잠금 해제
        Cursor.lockState = CursorLockMode.None;

        if(crafter!= null)
        {
            buildingNameText.text = crafter.GetComponent<ConstructibleBuilding>().buildingName;
            RefreshRecipeList();
        }
    }

    public void HideUI()        //UI 숨기기
    {
        craftingPanel.SetActive(false);
        currentCrafter = null;

        Cursor.visible = false;                     //마우스 커서 잠금 표시 없애기
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Start()
    {
        closeButten.onClick.AddListener(() => HideUI());
    }
}
