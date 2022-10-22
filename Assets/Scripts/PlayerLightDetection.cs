using Assets;
using UnityEngine;

public class PlayerLightDetection : MonoBehaviour
{
    public float DetectionRadius = 20f;

    private PlayerStatuses _playerStatus;
    private GameObject _player;
    
    // prevents multiple detectors from overwriting one another
    private bool _lastDetectedByThisReference;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Constants.PlayerTag);
        _playerStatus = FindObjectOfType<PlayerStatuses>();
    }

    void Update()
    {
        PlayerInLightCheck();
    }

    private void PlayerInLightCheck()
    {
        var direction = (_player.transform.position - this.transform.position).normalized;
        var ray = new Ray(this.transform.position, direction + new Vector3(0, 0.1f, 0)); // Apply a slight bias to Y-axis for player controller
        bool playerInRange = Physics.Raycast(ray, out RaycastHit hitInfo, DetectionRadius) && hitInfo.collider.CompareTag(Constants.PlayerTag);

        if (playerInRange)
        {
            Debug.DrawRay(this.transform.position, ray.direction * DetectionRadius, Color.green);

            _playerStatus.IsInLight = true;
            _lastDetectedByThisReference = true;
        }
        else if (_lastDetectedByThisReference)
        {
            Debug.DrawRay(this.transform.position, ray.direction * DetectionRadius, Color.red);

            _playerStatus.IsInLight = false;
            _lastDetectedByThisReference = false;
        }
        else
        {
            Debug.DrawRay(this.transform.position, ray.direction * DetectionRadius, Color.gray);
        }
    }
}
