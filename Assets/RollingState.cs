using UnityEngine;

public class RollingState : MovingState
{
    private GameObject blackKnight;
    public GameObject characterRobotBoy;
    private Animator blackKnightAnimator;
    public RollingState(GameObject blackKnight, GameObject characterRobotBoy, Animator blackKnightAnimator)
    {
        this.blackKnight = blackKnight;
        this.characterRobotBoy = characterRobotBoy;
        this.blackKnightAnimator = blackKnightAnimator;
    }

    public void move()
    {
        blackKnightAnimator.SetTrigger("evade_1");
        if (blackKnightAnimator.GetCurrentAnimatorStateInfo(0).IsName("evade_1"))
        {
            blackKnight.transform.position = Vector2.MoveTowards(blackKnight.transform.position, new Vector2(characterRobotBoy.transform.position.x, blackKnight.transform.position.y), 6 * Time.deltaTime);
            if (blackKnight.transform.position.x < characterRobotBoy.transform.position.x)
            {
                blackKnight.transform.localEulerAngles = new Vector2(0, 0);
            }
            else
            {
                blackKnight.transform.localEulerAngles = new Vector2(0, 180);
            }
        }
    }
}
