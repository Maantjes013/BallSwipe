using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindManager : MonoBehaviour
{
    public float minWind;
    public float maxWind;
    public Image windImage;

    private Ball activeBall;
    private float windForce;

    private Vector3 windDirection = new Vector3(1, 0, 0);

    private void Update()
    {
        ApplyWindToBall();
    }

    public float GetWindForce()
    {
        return windForce;
    }

    private void ApplyWindToBall()
    {
        if (activeBall.rb != null && activeBall.isFlying)
        {
            // Apply wind force to the ball
            activeBall.rb.AddForce(windDirection * windForce);
        }
    }

    public void DetermineWindForce(Ball instantiatedBall)
    {
        //Get random wind force and round by 2 decimals
        windForce = (float)System.Math.Round(Random.Range(minWind, maxWind), 2);

        //rotate wind image to give indication of wind direction
        if (windForce > 0)
            windImage.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            windImage.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        activeBall = instantiatedBall;
    }
}
