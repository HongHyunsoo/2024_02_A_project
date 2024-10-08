using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bicycle : Vehicle
{
    //Move 메서드 재정의
    public override void Move()
    {
        base.Move();        //기본 이동
        //자전거 만의 추가 동작
        transform.Rotate(0,10 * Time.deltaTime,0);
    }
    public override void Horn()
    {
        Debug.Log("자전거 경적 : DingDing");
    }
}
