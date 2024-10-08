using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
   protected PlayerStateMachine stateMachine;       //���� �ӽ� ����

   protected PlayerController playerController;     //�÷��̾� ��Ʈ�ѷ� ����

    public PlayerState(PlayerStateMachine stateMachine) //���¸ӽŰ� �÷��̾� ��Ʈ�ѷ� ���� �ʱ�ȭ
    {
        this .stateMachine = stateMachine;
        this.playerController = stateMachine.playerController;
    }

    //���� �޼���� : ���� Ŭ�������� �ʿ信 ���� �������̵�
    public virtual void Enter() { }     //���� ���� �� ȣ��
    public virtual void Exit() { }      //���� ���� �� ȣ��
    public virtual void Update() { }    //�� ������ ���� ȣ��
    public virtual void FixedUpdate() { }   //�����ð� �������� ȣ��

    //���� ��ȯ ������ ȣ���ϴ� �޼���
    protected void CheckTransitions()
    {
        if (playerController.isGrounded())  //���� ���� ���� ��ȯ ����
        {
            if(Input.GetKeyUp(KeyCode.Space))   //���� Ű ������ ��
            {
                stateMachine.TransitionToState(new JumpingState(stateMachine));
            }
            else if(Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical") != 0)   //�̵� Ű�� ������ ��
            {
                stateMachine.TransitionToState(new MovingState(stateMachine));
            }
            else
            {
                stateMachine.TransitionToState(new IdleState(stateMachine));        //�ƹ� Ű�� ������ �ʾ��� ��
            }
        }
        //���߿� ���� �� ��ȯ ����
        else
        {
            if (playerController.GetVerticalVelocity() > 0)     //Y�� �̵� �ӵ� ���� ��� �� �� ���� ��
            {
                stateMachine.TransitionToState(new JumpingState(stateMachine));
            }
            else                                                //Y�� �̵� �ӵ� ���� ��� �� �� ���� ��
            {
                stateMachine.TransitionToState(new FallingState(stateMachine));
            }
        }
    }
}

//IdleState : �÷��̾ ������ �ִ� ����
public class IdleState : PlayerState
{
    public IdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Update()
    {
        CheckTransitions();     //�� ������ ���� ���� ��ȯ ���� üũ
    }
}

//MovingState : �÷��̾ �̵� ���� ����
public class MovingState : PlayerState
{
    public MovingState (PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();     //�� ������ ���� ���� ��ȯ ���� üũ
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();      //������� �̵� ó��
    }
}

//MovingState : �÷��̾ ���� ���� ����
public class JumpingState : PlayerState
{
    public JumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();     //�� ������ ���� ���� ��ȯ ���� üũ
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();      //������� �̵� ó��
    }
}

//FallingState : �÷��̾ �������� ����
public class FallingState : PlayerState
{
    public FallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();     //�� ������ ���� ���� ��ȯ ���� üũ
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();      //������� �̵� ó��
    }
}
