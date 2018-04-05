using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour {
    public float speed = 20;//针移动的速度
    private bool isFly = false;//针是否在从起始点到圆的过程中
    private bool isReach = false;//针是否实例化后到达了屏幕起始位置
    private Transform startPoint;//屏幕上点击之前针的位置
    private Transform circle;//获取组件大圆
    private Vector3 targetCirclePosition;//由于不是插到圆心位置，需要另求目标位置
	// Use this for initialization
	void Start () {
        startPoint = GameObject.Find("StartPosition").transform;
        circle = GameObject.Find("Circle").transform;
        targetCirclePosition = circle.position;
        targetCirclePosition.y -= 1.55f;//这里是理想碰撞下圆的y坐标和针的y坐标的差，也就是目标的插入位置

    }
    /// <summary>
    /// 从起始点到插入，具体的移动见Update
    /// </summary>
	public void StartFly()
    {
        isFly = true;
        isReach = true;
    }
	// Update is called once per frame
	void Update () {
		if(isFly == false)
        {
            if(isReach==false)
            {//这一段是从屏幕外的生成位置，到屏幕上的开始位置，参数：当前位置，目标位置，速度
                transform.position= Vector3.MoveTowards(transform.position, startPoint.position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, startPoint.position) < 0.05f)//计算距离
                    isReach = true;//已到达起始位置
            }
        }
        //从屏幕起始位置移动到圆
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetCirclePosition, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position,targetCirclePosition)<0.05f)
            {
                transform.position = targetCirclePosition;
                transform.parent = circle;//这样就会和圆一起rotate
                isFly = false;
            }
        }
	}
}
