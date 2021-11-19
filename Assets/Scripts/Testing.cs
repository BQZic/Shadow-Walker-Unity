using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using LightObject;

public class Testing : MonoBehaviour
{
    [SerializeField] private PlayerAim playerAim;

    private void Start()
    {
        playerAim.OnShoot += PlayerAim_OnShoot;
    }

    private void PlayerAim_OnShoot(object sender, PlayerAim.OnShootEventArgs e)
    {
        // Debug.DrawLine(e.startPosition, e.shootPosition, Color.white, .1f);
        Transform ballTransform = Instantiate(GameAssets.i.pfLightBall, e.StartPosition, Quaternion.identity);
        
        Vector3 shootDir = (e.ShootPosition - e.StartPosition).normalized;
        
        ballTransform.GetComponent<LightBall>().SetUp(shootDir, e.LightBallLevel, e.StartPosition, 
            e.ShootPosition, e.MaxShootRange);
    }

    private void CreateTracer(Vector3 fromPosition, Vector3 targetPosition)
    {
        Vector3 dir = (targetPosition - fromPosition).normalized;
    }
}
