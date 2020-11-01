using Assets;
using UnityEngine;

public class PlayerLightDetection : MonoBehaviour
{
    public float DetectionRadius = 20f;

    private PlayerStatuses _playerStatus;
    private GameObject _player;
    
    // prevents multiple detectors from overwriting one another
    private bool _lastDetectedByThisReference;

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
        var ray = new Ray(this.transform.position, direction);
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
    }
}
