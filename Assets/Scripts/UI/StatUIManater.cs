using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUIManater : MonoBehaviour
{
    public static StatUIManater Instance {  get; private set; }

    [Header("UI References")]
    public Slider hungerSlider;             //��� ������
    public Slider suitDurabillitySlider;    //���ֺ� ������ ������
    public TextMeshProUGUI hungerText;      //��� ��ġ �ؽ�Ʈ
    public TextMeshProUGUI durabillityText; //���ֺ� ������ ��ġ �ؽ�Ʈ

    private SurvivalState survivalStats;

    private void Awake()
    {
         Instance = this;   
    }
    // Start is called before the first frame update
    void Start()
    {
        survivalStats = FindObjectOfType<SurvivalState>();

        //�����̴� �ִ밪 ����
        hungerSlider.maxValue = survivalStats.maxHunger;        
        suitDurabillitySlider.maxValue = survivalStats.maxSuitDurability;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatUI();
    }

    private void UpdateStatUI()
    {
        //�����̴� �� ������Ʈ
        hungerSlider.value = survivalStats.currentHunger;
        suitDurabillitySlider.value = survivalStats.currentSuitDurability;

        //�ؽ�Ʈ ������Ʈ 
        hungerText.text = $"��� : {survivalStats.GetHungerPercentage(): F0}%";
        durabillityText.text = $"���ֺ� ������ : {survivalStats.GetSuitDurabilityPercentage(): F0}%";

        //���� ������ �� ���� ����
        hungerSlider.fillRect.GetComponent<Image>().color =
            survivalStats.currentHunger < survivalStats.maxHunger * 0.3f ? Color.red : Color.green;

        suitDurabillitySlider.fillRect.GetComponent<Image>().color =
            survivalStats.currentSuitDurability < survivalStats.maxSuitDurability * 0.3f ? Color.red : Color.blue;
    }
}
