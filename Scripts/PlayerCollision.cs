using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public Rigidbody player;
    public MoveObjectConstVel moveObjConst;
    public JumpObject jumpObj;
    public ScoreManager scoreMng;
    
    public GameObject ctrlManager;

    private bool isFirstContact = false;
    private float colY;
    private float colX;
    private float stuckTime = 0f;
    private bool isNeverStuck = true;

    private float gameoverTime = 0.5f;

    private void OnCollisionEnter(Collision collision)
    {
        // detect first contact with platform
        if (!isFirstContact)
        {
            isFirstContact = true;
            ctrlManager.SetActive(true);
        }

        colX = collision.collider.transform.position.x;
        colY = collision.collider.transform.position.y;

        if (colY + 0.8 <= transform.position.y)
        {
            jumpObj.OnSurfaceEnter();
            scoreMng.OnSuccessfulLand(Convert.ToInt32(collision.collider.name));
            collision.collider.gameObject.GetComponent<CollisionAnimTrig>().OnSuccessfulLand();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        jumpObj.OnSurfaceExit();
    }

    private void OnBecameInvisible()
    {
        player.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void FixedUpdate()
    {
        // detect stuck time
        if (Math.Abs(player.velocity.z) <= 4)
        {
            stuckTime += Time.deltaTime;
        }
        else
        {
            stuckTime = 0;
        }

        // motion after collision
        if (!isNeverStuck)
        {
            gameoverTime -= Time.deltaTime;
            player.AddForce(0f, 0f, -6000f * Time.deltaTime);
        }

        // game over after 1 second after the collision
        if (gameoverTime <= 0 && !SceneManager.GetSceneByName("Death").isLoaded){
            FindObjectOfType<GameOver>().OnGameOver();
        }

        // instantaneous motion during collision
        if (stuckTime > 0.06 && isNeverStuck)
        {
            isNeverStuck = false;

            // disable controls
            ctrlManager.SetActive(false);

            // disable camera focus to player
            GameObject.Find("Main Camera").GetComponent<CameraFollow>().enabled = false;

            // remove constant moving speed and jumping force
            moveObjConst.enabled = false;
            player.velocity = Vector3.zero;

            // bounce back direction
            if (Math.Abs(colX - player.position.x) <= 0.7)
            {
                player.AddForce(0f, 0f, -30f, ForceMode.Impulse);
            }
            else if ((Math.Abs(colX - player.position.x) <= 0.85))
            {
                if (colX < player.position.x)
                {
                    player.AddForce(8f, 0f, -28f, ForceMode.Impulse);
                }
                else
                {
                    player.AddForce(-8f, 0f, -28f, ForceMode.Impulse);
                }
            }
            else if (colX < player.position.x)
            {
                player.AddForce(15f, 0f, -25f, ForceMode.Impulse);
            }
            else
            {
                player.AddForce(-15f, 0f, -25f, ForceMode.Impulse);
            }
        }
    }
}
