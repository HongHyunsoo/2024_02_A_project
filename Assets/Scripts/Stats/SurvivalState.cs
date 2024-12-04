using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalState : MonoBehaviour
{
    [Header("Hunger Setting")]
    public float maxHunger = 100;               //최대 허기량
    public float currentHunger;                 //현재 허기량
    public float hungerDecreaseRate = 1;        //초당 허기 감소량

    [Header("Space Suit Setting")]
    public float maxSuitDurability = 100;       //최대 우주복 내구도
    public float currentSuitDurability;         //현재 우주복 내구도
    public float havestingDamage = 5.0f;        //수집시 우주복 내구도
    public float craftingDamage = 3.0f;         //제작시 우주복 내구도

    private bool isGameOver = false;            //게임 오버 상태
    private bool isPaused = false;              //일시정지 상태
    private float hungerTimer = 0;              //허기 감소 타이머



    // Start is called before the first frame update
    void Start()
    {
        //시작 시 스텟 최대로
        currentHunger = maxHunger;
        currentSuitDurability = maxSuitDurability;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver || isPaused) 
            return;

        hungerTimer += Time.deltaTime;
        if (hungerTimer >= 1.0f)
        {
            currentHunger = Mathf.Max(0, currentHunger - hungerDecreaseRate);
            hungerTimer = 0.0f;

            CheckDeath();
        }
    }

    public void DamageHarvesting()      //아이템 수집 시 우주복 데미지
    {
        if (!isGameOver || isPaused) return;

        currentSuitDurability = Mathf.Max(0, currentSuitDurability - havestingDamage);      //0 이하로 떨어지지 않게
        CheckDeath();
    }

    public void DamageCrafting()        //아이템 제작 시 우주복 데미지
    {
        if (!isGameOver || isPaused) return;

        currentSuitDurability = Mathf.Max(0, currentSuitDurability - craftingDamage);       //0 이하로 떨어지지 않게
        CheckDeath();
    }

    public void EatFood(float amount)           //음식 섭취로 허기 회복
    {   
        if (isGameOver || isPaused) 
            return;

        currentHunger = Mathf.Min(maxHunger, currentHunger + amount);   //100 이상 가지 않게

        if (FloatingTextManager.instance != null )
        {
            FloatingTextManager.instance.Show($"허기 회복 + {amount}", transform.position + Vector3.up);
        }
    }
    public void RepairSuit(float amount)        //우주복 수리 크리스탈로 제작한 수리 키트 사용
    {
        if (isGameOver || isPaused) return;

        currentSuitDurability = Mathf.Min(maxSuitDurability, currentSuitDurability + amount);   //100 이상 가지 않게

        if (FloatingTextManager.instance != null)
        {
            FloatingTextManager.instance.Show($"우주복 수리 + {amount}", transform.position + Vector3.up);
        }
    }
    private void CheckDeath()           //플레이어 사망 처리 체크
    {
        if(currentHunger <= 0 || currentSuitDurability <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()      //플레이어 사망 함수
    {
        isGameOver = true;
        Debug.Log("플레이어 사망");
        //TODO: 사망 처리 추가 (게임 오버 UI, 리스폰 등등)
    }

    public float GetHungerPercentage()  //허기짐 % 리턴 함수
    {
        return (currentHunger / maxHunger) * 100;
    }

    public float GetSuitDurabilityPercentage()  //슈트 % 리턴 함수
    {
        return (currentSuitDurability / maxSuitDurability) * 100;
    }
    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void ResetStates()   //리셋 함수 작성
    {
        isGameOver = false;
        isPaused = false;
        currentHunger = maxHunger;
        currentSuitDurability = maxSuitDurability;
        hungerTimer = 0;
    }
}
