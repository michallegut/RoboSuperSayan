using System.Collections;
using UnityEngine;
using UnityStandardAssets._2D;

public class BlackKnightController : MonoBehaviour
{
    public GameObject blackKnight;
    public GameObject characterRobotBoy;
    public GameObject swordCollider;
    public GameObject bigWaveCollider;
    public GameObject mediumWaveCollider;
    public GameObject smallWaveCollider;
    public GameObject gameOverMessage;
    private Animator blackKnightAnimator;

    private int health;

    private MovingState blackKnightState;
    private bool isAttackOnCooldown;

    private void Start()
    {
        blackKnightAnimator = blackKnight.GetComponent<Animator>();
        health = 18;
        blackKnightState = new WalkingState(blackKnight, characterRobotBoy, blackKnightAnimator);
        isAttackOnCooldown = false;
    }

    private void FixedUpdate()
    {
        blackKnightState.move();
        if (!isAttackOnCooldown && health > 6 && Mathf.Abs(blackKnight.transform.position.x - characterRobotBoy.transform.position.x) <= 5)
        {
            blackKnightAnimator.SetTrigger("skill_1");
            StartCoroutine(ActivateCooldown());
        }
    }

    private IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Laser") || collision.gameObject.tag.Equals("KiWave"))
        {
            Destroy(collision.gameObject);
            health--;
            if (collision.gameObject.tag.Equals("KiWave")) health--;
            switch (health)
            {
                case 12:
                    Enrage();
                    break;
                case 6:
                    blackKnightAnimator.SetTrigger("skill_2");
                    yield return new WaitForSeconds(1);
                    StartRolling();
                    break;
                case 0:
                    Die();
                    yield return new WaitForSeconds(3);
                    characterRobotBoy.GetComponent<Platformer2DUserControl>().enabled = false;
                    gameOverMessage.SetActive(true);
                    break;
                default:
                    if (health > 10)
                    {
                        blackKnightAnimator.SetTrigger("hit_1");
                    }
                    break;
            }
        }
    }

    private void Enrage()
    {
        blackKnightAnimator.SetTrigger("idle_2");
        blackKnightState = new RunningState(blackKnight, characterRobotBoy, blackKnightAnimator);
    }

    private void StartRolling()
    {
        blackKnightState = new RollingState(blackKnight, characterRobotBoy, blackKnightAnimator);
    }

    private void Die()
    {
        blackKnightAnimator.SetTrigger("death");
        swordCollider.SetActive(false);
        bigWaveCollider.SetActive(false);
        mediumWaveCollider.SetActive(false);
        smallWaveCollider.SetActive(false);
        blackKnight.GetComponent<BlackKnightController>().enabled = false;
    }

    private IEnumerator ActivateCooldown()
    {
        isAttackOnCooldown = true;
        yield return new WaitForSeconds(1);
        isAttackOnCooldown = false;
    }
}
