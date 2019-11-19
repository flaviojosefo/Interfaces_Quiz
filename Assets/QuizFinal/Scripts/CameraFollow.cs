using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class CameraFollow : MonoBehaviour {

    [SerializeField] private PathCreator pathCreator = null;

    [SerializeField] private Transform point = null;

    [SerializeField] private float speed = 0.0f;

    [SerializeField] private float minDistance = 0.0f;

    private float distanceTravelled = 0.0f;

    void Update() {

        if (!SlowDown()) {

            distanceTravelled += speed * Time.deltaTime;

            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
        }

        
    }

    private bool SlowDown() {

        if (Vector3.Distance(transform.position, point.position) <= minDistance) {

            speed = 0;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            return true;
        }

        return false;
    }
}
