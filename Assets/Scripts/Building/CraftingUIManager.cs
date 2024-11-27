using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingUIManager : MonoBehaviour
{
    public static CraftingUIManager Instance { get; private set; }  //�̱��� �ν��Ͻ�

    [Header("UI References")]
    public GameObject craftingPanel;            //���� UI �г�
    public TextMeshProUGUI buildingNameText;    //�ǹ� �̸� �ؽ�Ʈ
    public Transform recipeContainer;           //������ ��ư�� �� �����̳�
    public Button closeButten;                  //�ݱ� ��ư
    public GameObject recipeButtonPrefabs;      //������ ��ư ������

    private BuildingCraftor currentCrafter;     //���� ���õ� �ǹ��� ���� �ý���

    private void Awake()
    {
        if(Instance == null) Instance = this;       //�̱��� ����
        else Destroy(gameObject);

        craftingPanel.SetActive(false);
    }

    private void RefreshRecipeList()
    {
        foreach(Transform child in recipeContainer)
        {
            Destroy(child.gameObject);
        }
        if(currentCrafter != null && currentCrafter.recipes != null)        //�� ������ ��ư�� ����
        {
            foreach(CraftingRecipe recipe in currentCrafter.recipes)
            {
                GameObject buttenObj = Instantiate(recipeButtonPrefabs, recipeContainer);
                RecipeButton recipeButten = buttenObj.GetComponent<RecipeButton>();
                recipeButten.Setup(recipe, currentCrafter);
            }
        }
    }

    public void ShowUI(BuildingCraftor crafter)     //UI ǥ��
    {
        currentCrafter = crafter;
        craftingPanel.SetActive(true);              

        Cursor.visible = true;                      //���콺 Ŀ�� ǥ�� ��� ����
        Cursor.lockState = CursorLockMode.None;

        if(crafter!= null)
        {
            buildingNameText.text = crafter.GetComponent<ConstructibleBuilding>().buildingName;
            RefreshRecipeList();
        }
    }

    public void HideUI()        //UI �����
    {
        craftingPanel.SetActive(false);
        currentCrafter = null;

        Cursor.visible = false;                     //���콺 Ŀ�� ��� ǥ�� ���ֱ�
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Start()
    {
        closeButten.onClick.AddListener(() => HideUI());
    }
}
