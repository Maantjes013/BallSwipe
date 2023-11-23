using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public Transform spawnPoint;
    public Ball ballPrefab;
    public Text scoreField;
    public Text windField;
    public CinemachineVirtualCamera ballCamera;
    public WindManager windManager;
    public GameObject explosionEffect;

    private Ball instantiatedBall;
    private GameObject instantiatedExplosionEffect;

    private float points = 0;

    private bool effectIsPlaying;

    private void Start()
    {
        SpawnBall();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !effectIsPlaying)
        {
            Destroy(instantiatedBall.gameObject);
            SpawnBall();
        }
    }

    private void SpawnBall()
    {
        instantiatedBall = Instantiate(ballPrefab, spawnPoint);
        AdjustCamera(instantiatedBall);
        windManager.DetermineWindForce(instantiatedBall);
        UpdateWindUI();
    }

    public void ExplodeBall()
    {
        GameObject instantiatedExplosionEffect = Instantiate(explosionEffect);
        instantiatedExplosionEffect.transform.SetPositionAndRotation(instantiatedBall.transform.position, Quaternion.identity);

        effectIsPlaying = true;
        Destroy(instantiatedBall.gameObject);

        StartCoroutine(SpawnBallAfterDelay());
    }

    public IEnumerator SpawnBallAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        SpawnBall();
        effectIsPlaying = false;
    }

    private void AdjustCamera(Ball ballToFollow)
    {
        ballCamera.Follow = ballToFollow.transform;
        ballCamera.LookAt = ballToFollow.transform;
    }

    public void AddPoints()
    {
        //Grant more points when more wind is present
        float PositiveWind = Mathf.Round(Mathf.Abs(windManager.GetWindForce()));
        if (PositiveWind > 0)
            points += PositiveWind;
        else
            points++;

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreField.text = points.ToString();
    }

    private void UpdateWindUI()
    {
        windField.text = Mathf.Abs(windManager.GetWindForce()).ToString();
    }
}
