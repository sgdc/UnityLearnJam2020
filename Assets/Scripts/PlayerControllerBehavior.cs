using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerBehavior : MonoBehaviour
{
    public static float strikeDistance = 6.0f;
    public static float minStrikeAngleFromFront = 50.0f;

    const float acceleration = 0.01f;
    const float mouseSensitivity = 2.5f;
    Vector3 velocity;

    bool dead = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(dead) {
            if (Input.GetKey(KeyCode.Space))
             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        Vector3 instantAcceleration = (this.transform.right * Input.GetAxis("Horizontal") + this.transform.forward * Input.GetAxis("Vertical"));
        instantAcceleration = acceleration * instantAcceleration.normalized;
        velocity += instantAcceleration;
        velocity *= 0.9f;

        this.transform.position += velocity;

        float mouseRotation = Input.GetAxis("Mouse X");
        this.transform.Rotate(Vector3.up, mouseRotation);

        if(Input.GetMouseButton(0)) {
            EnemyControllerBehavior enemy = FindObjectOfType<EnemyControllerBehavior>();
            if(enemy.CanBeStruck()) {
                Vector3 difference = enemy.transform.position - this.transform.position;
                this.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(difference, Vector3.up), Vector3.up);
                this.transform.position += difference / 2.0f;
                enemy.transform.position += difference / 1.5f;
                enemy.speed *= 1.5f;
                enemy.wanderConstant *= 0.8f;
                enemy.transform.localScale *= 0.75f;
                enemy.slide = difference.normalized * 2.0f;
                if(enemy.transform.localScale.x < 0.4f) {
                    Destroy(enemy);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        dead = true;
        this.GetComponent<Renderer>().enabled = false;
    }
}
