using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
   protected PlayerStateMachine stateMachine;       //상태 머신 참조

   protected PlayerController playerController;     //플레이어 컨트롤러 참조

    public PlayerState(PlayerStateMachine stateMachine) //상태머신과 플레이어 컨트롤러 참조 초기화
    {
        this .stateMachine = stateMachine;
        this.playerController = stateMachine.playerController;
    }

    //가상 메서드들 : 하위 클레스에서 필요에 따라 오버라이드
    public virtual void Enter() { }     //상태 진입 시 호출
    public virtual void Exit() { }      //상태 종료 시 호출
    public virtual void Update() { }    //매 프레임 마다 호출
    public virtual void FixedUpdate() { }   //고정시간 간격으로 호출

    //상태 전환 조건을 호출하는 메서드
    protected void CheckTransitions()
    {
        if (playerController.isGrounded())  //지상에 있을 때의 전환 로직
        {
            if(Input.GetKeyUp(KeyCode.Space))   //점프 키 눌렀을 때
            {
                stateMachine.TransitionToState(new JumpingState(stateMachine));
            }
            else if(Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical") != 0)   //이동 키가 눌렸을 때
            {
                stateMachine.TransitionToState(new MovingState(stateMachine));
            }
            else
            {
                stateMachine.TransitionToState(new IdleState(stateMachine));        //아무 키도 누르지 않았을 때
            }
        }
        //공중에 있을 때 전환 로직
        else
        {
            if (playerController.GetVerticalVelocity() > 0)     //Y축 이동 속도 값이 양수 일 때 점프 중
            {
                stateMachine.TransitionToState(new JumpingState(stateMachine));
            }
            else                                                //Y축 이동 속도 값이 양수 일 때 점프 중
            {
                stateMachine.TransitionToState(new FallingState(stateMachine));
            }
        }
    }
}

//IdleState : 플레이어가 정지해 있는 상태
public class IdleState : PlayerState
{
    public IdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Update()
    {
        CheckTransitions();     //매 프레임 마다 상태 전환 조건 체크
    }
}

//MovingState : 플레이어가 이동 중인 상태
public class MovingState : PlayerState
{
    public MovingState (PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();     //매 프레임 마다 상태 전환 조건 체크
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();      //물리기반 이동 처리
    }
}

//MovingState : 플레이어가 점프 중인 상태
public class JumpingState : PlayerState
{
    public JumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();     //매 프레임 마다 상태 전환 조건 체크
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();      //물리기반 이동 처리
    }
}

//FallingState : 플레이어가 떨어지는 상태
public class FallingState : PlayerState
{
    public FallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();     //매 프레임 마다 상태 전환 조건 체크
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();      //물리기반 이동 처리
    }
}
