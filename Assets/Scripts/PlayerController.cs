using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //�÷��̾��� ������ �ӵ��� �����ϴ� ����
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;      //�̵� �ӵ�
    public float jumpForce = 5.0f;      //���� ��
    public float rotationSpeed = 10.0f; //ȸ�� �ӵ�

    //���� ������
    public bool isFirstPerson = true;          //1��Ī ������� ����
    private bool isGrounded;    //�÷��̾ ���� �ִ��� üũ ����
    private Rigidbody rb;       //�÷��̾��� rigidbody

    [Header("Camera Settings")]
    public Camera firstPersonCamera;        //1��Ī ī�޶�
    public Camera thirdPersonCamera;        //3��Ī ī�޶�
    public float mouseSenesitivity = 2.0f;  //���콺 ����

    public float radius = 5.0f;             //3��Ī ī�޶�� �÷��̾� ���� �Ÿ�
    public float minRadius = 1.0f;          //ī�޶� �ּ� �Ÿ�
    public float maxRadius = 10.0f;         //ī�޶� �ִ� �Ÿ�

    public float yMinLimit = -90;           //ī�޶� ���� ȸ�� �ּҰ�
    public float yMaxLimit = 90;            //ī�޶� ���� ȸ�� �ִ밢

    private float theta = 0.0f;                     //ī�޶��� ���� ȸ�� ����
    private float phi = 0.0f;                       //ī�޶��� ���� ȸ�� ����
    private float targetVericalRotation = 0;        //��ǥ ���� ȸ�� ����
    private float verticalRotationSpeed = 240f;     //���� ȸ�� ����


    void Start()
    {
        rb = GetComponent<Rigidbody>();                 //Rigidbody ������Ʈ�� ��������

        Cursor.lockState = CursorLockMode.Locked;       //���콺 Ŀ���� ��װ� �����.
        SetupCameras();
        SetActiveCamera();
    }
    void Update()
    {
        HandleRotation();
        HandleJump();
        HandleCameraToggie();
    }

    void FixedUpdate()
    {
        HandleMovement();

    }
    void SetActiveCamera()
    {
        firstPersonCamera.gameObject.SetActive(isFirstPerson);
        thirdPersonCamera.gameObject.SetActive(!isFirstPerson);
    }

    //ī�޶� �ʱ� ��ġ �� ȸ���� �����ϴ� �Լ�
    void SetupCameras()
    {
        firstPersonCamera.transform.localPosition = new Vector3(0.0f, 0.6f, 0.0f);      //1��Ī ī�޶� ��ġ
        firstPersonCamera.transform.localRotation = Quaternion.identity;                //1��Ī ī�޶� ȸ�� �ʱ�ȭ

    }

    //ī�޶� �� ĳ���� ȸ���� ó���ϴ� �Լ�

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSenesitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSenesitivity;

        //���� ȸ��(theta ��)
        theta += mouseX;                         //���콺 �Է� �� �߰�
        theta = Mathf.Repeat(theta, 360.0f);     //���� ���� 360���� ���� �ʵ��� ���� (0~360)

        //���� ȸ�� ó��
        targetVericalRotation -= mouseY;
        targetVericalRotation = Mathf.Clamp(targetVericalRotation,yMinLimit,yMaxLimit);     //���� ȸ�� ����
        phi = Mathf.MoveTowards(phi, targetVericalRotation, verticalRotationSpeed * Time.deltaTime);    

       

        if (isFirstPerson)
        {
            firstPersonCamera.transform.localRotation = Quaternion.Euler(phi, 0.0f, 0.0f);     //1��Ī ī�޶� ���� ȸ��
            transform.rotation = Quaternion.Euler(0.0f, theta, 0.0f);     //�÷��̾��� ȸ�� (ĳ���Ͱ� �������θ� ȸ��)

        }
        else
        {
            //3��Ī ī�޶� ���� ��ǥ�迡�� ��ġ �� ȸ�� ���
            float x = radius * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Cos(Mathf.Deg2Rad * theta);
            float y = radius * Mathf.Cos(Mathf.Deg2Rad * phi);
            float z = radius * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Sin(Mathf.Deg2Rad * theta);

            thirdPersonCamera.transform.position = transform.position + new Vector3(x, y, z);
            thirdPersonCamera.transform.LookAt(transform);  //ī�޶� �׻� �÷��̾ �ٶ󺸵���

            //���콺 ��ũ���� ����ϸ� ī�޶� �� ����
            radius = Mathf.Clamp(radius - Input.GetAxis("Mouse ScrollWheel") * 5, minRadius, maxRadius);
        }

    }//1��Ī�� 3��Ī ī�޶� ��Ȳ�ϴ� �Լ�
    void HandleCameraToggie()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;     //ī�޶� ��� ��ȯ
            SetActiveCamera();
        }
        
    }

    //�÷��̾� ���� ó�� �Լ� 
    void HandleJump()
    {
        //���� ��ư�� ������ ���� ���� ��
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);     //�������� ���� ���� ����
            isGrounded = false;         
        }
        //�÷��̾ ���� ��� �ִ��� ����
    }

    // �÷��̾��� �̵��� ó���ϴ� �Լ�

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); //�¿� �Է� (-1 ~ 1)
        float moveVertical = Input.GetAxis("Vertical");     //�յ� �Է� (1 ~ -1)

        Vector3 movement;

        if (!isFirstPerson)  //3��Ī ����� ��, ī�޶� �������� �̵� ó�� 
        {
            Vector3 cameraForward = thirdPersonCamera.transform.forward;    //ī�޶� �� ����
            cameraForward.y = 0.0f;     
            cameraForward.Normalize();  //���� ���� ����ȭ

            Vector3 cameraRight = thirdPersonCamera.transform.right;        //ī�޶� ������ ����
            cameraRight.y = 0.0f;
            cameraRight.Normalize();   //���� ���� ����ȭ

            movement = cameraRight * moveHorizontal + cameraForward * moveVertical;
        }
        else
        {
            //ĳ���� �������� �̵�
             movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        }

        //�̵� �������� ĳ���͸� ȸ��
        if(movement.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;      //�浹 ���̸� �÷��̾�� ���� �ִ�.
    }

}