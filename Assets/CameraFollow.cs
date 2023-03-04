using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] float followTime = 1f;
    [SerializeField] float emphasisOnTop = 1f;

    private void FixedUpdate() {
        Vector3 desiredPosition = Mathf.Max(0, (GameManager.currentScore - 3)) * Vector3.up * emphasisOnTop;
        float totalSpeed = emphasisOnTop;
        foreach(TetrisCat cat in GameManager.freeCats){
            float catSpeed = cat.getSpeed();
            desiredPosition += cat.transform.position * catSpeed;
            totalSpeed += catSpeed;
        }
        if(totalSpeed > 0) desiredPosition /= totalSpeed;

        transform.position = Vector3.Lerp(transform.position, desiredPosition + offset, Time.fixedDeltaTime / followTime);
    }
}
