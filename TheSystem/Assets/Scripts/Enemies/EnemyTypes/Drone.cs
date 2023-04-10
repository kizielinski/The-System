//Kyle Zielinski
//2/10/2023
//This script handles enemy drone hovering and interaction with our FOV cone.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drone : Enemy
{
    // Start is called before the first frame update
    private FieldOfView fov;
    private Vector3 currentPos;
    private List<GameObject> bullets;
    private int bulletsMAX = 10;
    private Object bulletPrefab;
    private bool canFire;
    private float gunCooldown = 0.4f;

    [Tooltip ("Adjusts speed of this drone's bullets. Default: 1.0f")]
    [SerializeField] private float bulletSpeedValue = 1.0f;

    void Start()
    {
        fov = GetComponentInChildren<FieldOfView>();
        isAlive = true;
        StartCoroutine(Hover());
        walkSpeed = 5;
        stepValue = 0.5f;

        bulletPrefab = Resources.Load("Prefabs/Bullet");
        bullets = new List<GameObject>();
        canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if(isStunned)
        {
            isAlive = false;
        }
        else
        {
            isAlive = true;
            //Cone alignment
            fov.SetAimDirection(Vector3.down);
            fov.SetOrigin(transform.position);
            if (fov.playerHit)
            {
                fov.PlayerHitIsNow = false;
                StartCoroutine(Shoot());
            }
        }
    }

    /// <summary>
    /// Runs a thread to constantly hover the drone even when attacking
    /// </summary>
    /// <returns></returns>
    private IEnumerator Hover()
    {
        currentPos = transform.position;

        float cosValue = 0;

        while (isAlive)
        {
            currentPos.x += walkSpeed * Mathf.Cos(cosValue) * Time.deltaTime;
            cosValue += stepValue * Time.deltaTime;
            transform.position = currentPos;
            yield return null;
        }
    }

    private IEnumerator Shoot()
    {
        if(bullets.Count < bulletsMAX && canFire)
        {
            canFire = false;
            bullets.Add(Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject);
            bullets[bullets.Count -1].GetComponent<Bullet>().SetSpeed = bulletSpeedValue;
            yield return new WaitForSeconds(gunCooldown);
            canFire = true;
        }
        else if (canFire)
        {
            canFire = false;
            GameObject bullet = null;
            foreach(GameObject b in bullets)
            {
                if (!b.activeSelf)
                {
                    bullet = b;
                    break;
                }
            }

            if(bullet != null)
            {
                bullet.SetActive(true);
                bullet.transform.position = transform.position;
            }
            
            yield return new WaitForSeconds(gunCooldown);
            canFire = true;
        }
    }

}
