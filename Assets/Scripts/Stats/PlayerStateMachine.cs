using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    
    public PlayerState currentState;                //현재 플레이어의 상태를 나타내는 변수
    public PlayerController playerController;       //플레이어 참조용 

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();        //플레이어 컨포넌트 참조
    }

    // Start is called before the first frame update
    void Start()
    {
        TransitionToState(new IdleState(this));     //초기 상태를 Idle로 설정
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != null)        //현재 상태가 존재한다면
        {
            currentState.Update();
        }
    }

    private void FixedUpdate()
    {
        if(currentState != null)         //현재 상태가 존재한다면
        {
            currentState.FixedUpdate();
        }
    }


    //TransitionToState 새로운 형태로 전환하는 메서드
    public void TransitionToState(PlayerState newState)
    {
        //현재 상태와 새로운 상태가 같은 타입일 경우 상태 전환을 하지 않게 한다.
        if (currentState?.GetType() == newState.GetType())
        {
            return;     //같은 타입으면 상태 전환 하지 않고 return
        }

        currentState?.Exit();           //현재 상태가 존재한다면 [?] IF문 처럼 쓰임

        currentState = newState;        //새로운 상태로 전환

        currentState.Enter();            //상태 시작

        Debug.Log($"Transitioned to State{newState.GetType().Name}");       //관련 내용 로그로 출력
    }
}
