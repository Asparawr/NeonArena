using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject moveJoystickObject;
    public GameObject shootJoystickObject;
    public GameObject playerModel;
    public GameObject gameManager;

    private FixedJoystick moveJoystick;
    private FixedJoystick shootJoystick;
    private Rigidbody2D playerRigidbody;
    private PlayerStats playerStats;
    private PauseController pauseController;

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
        pauseController = gameManager.GetComponent<PauseController>();
    }

    void Update()
    {
        //pause game when no joysticks are being used
        if (!moveJoystick.touchStart && !shootJoystick.touchStart)
        {
            pauseController.Pause();
        }
        else
        {
            pauseController.Unpause();
        }

        //open pause menu on back button press
        if (Input.GetKeyDown(KeyCode.Escape))
            pauseController.OpenPauseMenu();

        //movement
        if (moveJoystick.direction != Vector2.zero)
            playerRigidbody.velocity += moveJoystick.direction * playerStats.movementSpeed;

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
        foreach (string modName in playerStats.bulletModList)
        {
            (bullet.GetComponent(modName) as MonoBehaviour).enabled = true;
        }
        Destroy(bullet, playerStats.range * playerStats.rangeMod);
    }

    public void DisableJoysticks()
    {
        moveJoystick.isEnabled = false;
        shootJoystick.isEnabled = false;
        moveJoystick.ResetJoystick();
        shootJoystick.ResetJoystick();
    }

    public void EnableJoysticks()
    {
        moveJoystick.isEnabled = true;
        shootJoystick.isEnabled = true;
    }
}
