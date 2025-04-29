using UnityEngine;

public class TapManager : MonoBehaviour
{
    public Camera mainCamera;
    private BallPassManager passManager;
    public float rotationDelay = 0.2f;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {

                GameObject tappedPlayer = hit.collider.gameObject;

                if (tappedPlayer == GameManager.Instance.currentTarget)
                {
                    Vector3 mousePos = Input.mousePosition;
                    Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
                    worldPos.z = 0f;
                    StartCoroutine(RotateThenPass(tappedPlayer, worldPos));
                    GameManager.Instance.RegisterCorrectPass();
                }
                else
                {
                    GameManager.Instance.RegisterWrongPass();
                }
            }
        }
    }

    private System.Collections.IEnumerator RotateThenPass(GameObject targetPlayer, Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle += 80f;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        yield return new WaitForSeconds(rotationDelay);

        passManager = targetPlayer.GetComponent<BallPassManager>();
        passManager?.PassToPlayer(targetPlayer);
    }
}
