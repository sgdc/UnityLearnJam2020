using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerBehavior : MonoBehaviour
{
    PlayerControllerBehavior player;

    public float wanderConstant = 7.5f;
    public float speed = 0.1f;
    public Vector3 slide;

    void Start()
    {
        player = FindObjectOfType<PlayerControllerBehavior>();
    }

    public bool CanBeStruck()
    {
        Vector3 meToPlayerVector = player.transform.position - this.transform.position;
        float distanceToPlayer = meToPlayerVector.magnitude;
        float angleFromFront = Vector3.Angle(this.transform.forward, meToPlayerVector);
        return 
            distanceToPlayer < PlayerControllerBehavior.strikeDistance
            && angleFromFront > PlayerControllerBehavior.minStrikeAngleFromFront;
    }

    void UpdateMaterial() {
        if (CanBeStruck())
            this.GetComponent<Renderer>().material.color= new Color(1.0f, 0.5f, 0.5f);
        else
            this.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
    }

    void Update()
    {
        float rotation = (
            Mathf.Sin(Time.time) * 0.3f
            + Mathf.Sin(Time.time * 0.5f) * 0.2f
            + Mathf.Sin(Time.time * 30.0f) * 0.3f
            + Mathf.Sin(Time.time * 23.0f) * 0.2f
        ) * Random.Range(0, wanderConstant);
        this.transform.Rotate(Vector3.up, rotation);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(player.transform.position - this.transform.position), 0.02f);

        slide *= 0.9f;
        this.transform.position += slide + this.transform.forward * speed;

        UpdateMaterial();
    }
}
