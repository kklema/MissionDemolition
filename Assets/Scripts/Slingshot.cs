using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;

    public GameObject prefabProjectile;
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    public float velocityMult = 8f;

    private Rigidbody projectileRb;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;

            return S.launchPos;
        }
    }

    private void Awake()
    {
        S = this;

        Transform launchPointTransform = transform.Find("LaunchPoint");
        launchPoint = launchPointTransform.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTransform.position;
    }

    private void Update()
    {
        if (!aimingMode) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            projectileRb.isKinematic = false;
            projectileRb.velocity = -mouseDelta * velocityMult;
            FollowCamera.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile;
        }
    }

    private void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;
        projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.isKinematic = true;
    }
}