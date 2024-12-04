using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    
    public PlayerState currentState;                //���� �÷��̾��� ���¸� ��Ÿ���� ����
    public PlayerController playerController;       //�÷��̾� ������ 

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();        //�÷��̾� ������Ʈ ����
    }

    // Start is called before the first frame update
    void Start()
    {
        TransitionToState(new IdleState(this));     //�ʱ� ���¸� Idle�� ����
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != null)        //���� ���°� �����Ѵٸ�
        {
            currentState.Update();
        }
    }

    private void FixedUpdate()
    {
        if(currentState != null)         //���� ���°� �����Ѵٸ�
        {
            currentState.FixedUpdate();
        }
    }


    //TransitionToState ���ο� ���·� ��ȯ�ϴ� �޼���
    public void TransitionToState(PlayerState newState)
    {
        //���� ���¿� ���ο� ���°� ���� Ÿ���� ��� ���� ��ȯ�� ���� �ʰ� �Ѵ�.
        if (currentState?.GetType() == newState.GetType())
        {
            return;     //���� Ÿ������ ���� ��ȯ ���� �ʰ� return
        }

        currentState?.Exit();           //���� ���°� �����Ѵٸ� [?] IF�� ó�� ����

        currentState = newState;        //���ο� ���·� ��ȯ

        currentState.Enter();            //���� ����

        Debug.Log($"Transitioned to State{newState.GetType().Name}");       //���� ���� �α׷� ���
    }
}