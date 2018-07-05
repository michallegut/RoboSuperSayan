using UnityEngine;

public class RunningState : MovingState
{
    private GameObject blackKnight;
    public GameObject characterRobotBoy;
    private Animator blackKnightAnimator;
    public RunningState(GameObject blackKnight, GameObject characterRobotBoy, Animator blackKnightAnimator)
    {
        this.blackKnight = blackKnight;
        this.characterRobotBoy = characterRobotBoy;
        this.blackKnightAnimator = blackKnightAnimator;
    }

    public void move()
    {
        blackKnightAnimator.SetTrigger("run");
        if (blackKnightAnimator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            blackKnight.transform.position = Vector2.MoveTowards(blackKnight.transform.position, new Vector2(characterRobotBoy.transform.position.x, blackKnight.transform.position.y), 4 * Time.deltaTime);
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
