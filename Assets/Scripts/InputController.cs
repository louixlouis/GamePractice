using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        UpdateMouse();
    }

    void UpdateInput()
    {
        Vector3 moveDirection = Vector3.zero;

        // 작년 10월에 공개된 입력 패키지는 버그의 가능성 때문에 옛날 버전을 사용함.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection.y = 1;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection.y = -1;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x = -1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x = 1;
        }

        SystemManager.Instance.Hero.ProcessInput(moveDirection);
    }

    void UpdateMouse()
    {
        // 0: Left, 1: Right, 2: Middle
        if (Input.GetMouseButtonDown(0))
        {
            SystemManager.Instance.Hero.Fire();
        }
    }
}
