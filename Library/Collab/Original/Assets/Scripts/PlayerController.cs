using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject moveJoystickObject;
    public GameObject shootJoystickObject;
    public GameObject playerModel;

    private FixedJoystick moveJoystick;
    private FixedJoystick shootJoystick;
    private Rigidbody2D playerRigidbody;
    private PlayerStats playerStats;

    private Quaternion newRotation;
    public float rotationSpeed = 1;

    public GameObject bulletSpawner;
    public GameObject bulletPrefab;
    private float shootCooldown;

    private void Start()
    {
        moveJoystick = moveJoystickObject.GetComponent<FixedJoystick>();
        shootJoystick = shootJoystickObject.GetComponent<FixedJoystick>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        //movement
        if (moveJoystick.direction != Vector2.zero)
            playerRigidbody.velocity = moveJoystick.direction * playerStats.movementSpeed;

        //slerp rotation
        newRotation = Quaternion.FromToRotation(Vector3.up, shootJoystick.direction);
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        playerModel.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, playerModel.transform.rotation.eulerAngles.z));

        shootCooldown -= Time.deltaTime;
        if (shootJoystick.touchStart && shootCooldown <= 0f)
        {
            shootCooldown = playerStats.fireRate * playerStats.fireRateMod;
            Fire();
        }
    }

    private void Fire()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawner.transform.position, playerModel.transform.rotation);

        //set bullet stats
        bullet.transform.localScale *= playerStats.bulletScaleMod;
        bullet.GetComponent<Rigidbody2D>().velocity = playerModel.transform.up * playerStats.bulletSpeed;

        PlayerBulletStats bulletStats = bullet.GetComponent<PlayerBulletStats>();
        bulletStats.damage = playerStats.damage * playerStats.damageMod;

        //activate bullet effects
        playerStats.bulletModList.ForEach(delegate (string name)
        {
            Debug.Log(name);
            (bullet.GetComponent(name) as MonoBehaviour).enabled = false;
        });
        Destroy(bullet, playerStats.range * playerStats.rangeMod);
    }
}
