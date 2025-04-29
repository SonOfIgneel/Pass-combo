using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BallPassManager : MonoBehaviour
{
    public GameObject ball;
    public float passSpeed = 5f;
    public GameObject mainPlayer;
    private Vector3 targetPosition;
    private bool isPassing = false;

    void Update()
    {
        if (isPassing)
        {
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, targetPosition, passSpeed * Time.deltaTime);

            if (Vector3.Distance(ball.transform.position, targetPosition) < 0.05f)
            {
                isPassing = false;
                StartCoroutine(ResetBallPos());
            }
        }
    }

    public void PassToPlayer(GameObject player)
    {
        Animator animator = mainPlayer.GetComponent<Animator>();
        animator.enabled = true;
        StartCoroutine(DelayedPass(player, 0.45f));
    }

    private IEnumerator ResetBallPos()
    {
        yield return new WaitForSeconds(0.5f);
        ball.transform.localPosition = Vector3.zero;
        mainPlayer.transform.rotation = new quaternion(0, 0, 0, 0);
    }

    private IEnumerator DelayedPass(GameObject player, float delay)
    {
        yield return new WaitForSeconds(delay);
        Animator animator = mainPlayer.GetComponent<Animator>();
        animator.enabled = false;
        Transform footTarget = player.transform.Find("TargetPoint");
        if (footTarget != null)
        {
            targetPosition = footTarget.position;
            isPassing = true;
        }
    }
}
