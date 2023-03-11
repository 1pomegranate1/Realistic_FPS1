using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    GameObject FpsArm, Body, Camera, InertiaGameObject;

    [SerializeField]
    Rigidbody rigidBody;
    private Vector2 _rotation;
    public Vector2 aimInertia, aimRecoil, plusDirection;
    public float aimMaxInertia, aimMaxRecoil, recoilRecoveryDelay;
    [SerializeField]
    AnimationCurve inertiaRecoveryCurve, recoilRecoveryCurve;



    [SerializeField]
    float mouseSenstive;
    // Start is called before the first frame update
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OtherInput();
        PlayerRotate();
    }
    private void FixedUpdate()
    {
        PlayerMove();
    }
    Vector2 _move;
    void PlayerMove()
    {
        if (CanvasManager.UIActive == false)
        {
            _move = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        }
        else _move = Vector2.zero;

        Vector3 moveVector;
        if (_move != Vector2.zero)
        {
            moveVector = (transform.forward * _move.x + transform.right * _move.y ).normalized * Time.fixedDeltaTime * 300f + rigidBody.velocity.y * Vector3.up;
            rigidBody.velocity = moveVector;
        }
        else rigidBody.velocity = Vector3.zero + rigidBody.velocity.y * Vector3.up;
    }
    float lastFrameTime = 0f;
    void PlayerRotate()
    {
        float deltaTime = Time.time - lastFrameTime;
        lastFrameTime = Time.time;
        if (CanvasManager.UIActive == false)
        {
            _rotation = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));
        }
        else _rotation = Vector2.zero;
        float xRotPrev = Camera.transform.localEulerAngles.x;
        float yRotPrev = Body.transform.localEulerAngles.y;
        Vector2 RotNext;
        float deltaCoef = Time.smoothDeltaTime * 50f;

        // 마우스 회전시 총기 관성    
        aimInertia = new Vector2(   aimInertia.x + inertiaRecoveryCurve.Evaluate(aimInertia.x / aimMaxInertia) * deltaTime * (aimInertia.x < 0 ? 1f : -1f) * 70,
                                    aimInertia.y + inertiaRecoveryCurve.Evaluate(aimInertia.y / aimMaxInertia) * deltaTime * (aimInertia.y < 0 ? 1f : -1f) * 70);
        if (recoilRecoveryDelay <= 0)
        {
            //총기 반동
            aimRecoil = new Vector2(aimRecoil.x + recoilRecoveryCurve.Evaluate(aimRecoil.x / aimMaxRecoil) * deltaTime * (aimRecoil.x < 0 ? 1f : -1f) * 40,
                                        aimRecoil.y + recoilRecoveryCurve.Evaluate(aimRecoil.y / aimMaxRecoil) * deltaTime * (aimRecoil.y < 0 ? 1f : -1f) * 40);
        }
        else recoilRecoveryDelay -= Time.deltaTime;
        
        // 카메라 회전
        float yRotNext = yRotPrev + _rotation.x * deltaCoef;
        RotNext = (new Vector2(xRotPrev + _rotation.y * mouseSenstive, yRotPrev + _rotation.x * mouseSenstive)); 
        if(RotNext.x > 180f)
        {
            RotNext = new Vector2(RotNext.x -= 360, RotNext.y);
        }
        aimInertia = new Vector2(   Mathf.Clamp(aimInertia.x + -_rotation.x * 0.16f, -aimMaxInertia, aimMaxInertia),
                                    Mathf.Clamp(aimInertia.y + -_rotation.y * 0.16f, -aimMaxInertia, aimMaxInertia));
        plusDirection = aimInertia + new Vector2(aimRecoil.x, -aimRecoil.y);

        Camera.transform.localEulerAngles = Vector3.right * RotNext.x;
        Body.transform.localEulerAngles = Vector3.up * RotNext.y;
        //FpsArm.transform.rotation = Quaternion.Euler(Quaternion.RotateTowards(FpsArm.transform.rotation, Body.transform.rotation, Time.smoothDeltaTime * 500f).eulerAngles);
        FpsArm.transform.rotation = Body.transform.rotation;
        InertiaGameObject.transform.rotation = Camera.transform.rotation * Quaternion.Euler(new Vector3(plusDirection.y, plusDirection.x, 0));

    }
    void OtherInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바: 슬로우모션
        {
            if (Time.timeScale != 1)
                Time.timeScale = 1f;
            else
                Time.timeScale = 0.01f;

            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }
}
