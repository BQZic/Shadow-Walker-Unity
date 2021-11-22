using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightObject;

public class PlayerController : MonoBehaviour
{
    private PlayerAim _playerAim;

    private void Awake()
    {
        _playerAim = GetComponent<PlayerAim>();
    }

    private void Start()
    {
        _playerAim.OnShoot += PlayerAim_OnShoot;
    }

    private void PlayerAim_OnShoot(object sender, PlayerAim.OnShootEventArgs e)
    {
        // Debug.DrawLine(e.startPosition, e.shootPosition, Color.white, .1f);
        Transform ballTransform = Instantiate(GameAssets.i.pfLightBall, e.StartPosition, Quaternion.identity);
        
        Vector3 shootDir = (e.ShootPosition - e.StartPosition).normalized;
        
        ballTransform.GetComponent<LightBall>().SetUp(shootDir, e.LightBallLevel, e.StartPosition, 
            e.ShootPosition, e.MaxShootRange);
    }
}
