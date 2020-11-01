using Assets;
using UnityEngine;

public class PlayerLightDetection : MonoBehaviour
{
    public float DetectionRadius = 20f;

    private PlayerStatuses _playerStatus;
    private GameObject _player;
    
    // prevents multiple detectors from overwriting one another
    private bool _lastDetectedByThisReference;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Constants.PlayerTag);
        _playerStatus = FindObjectOfType<PlayerStatuses>();
        if (_player == null || _playerStatus == null)
        {
            Debug.LogError("Player or Status is null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        var direction = (_player.transform.position - this.transform.position).normalized;
        var ray = new Ray(this.transform.position, direction);

        bool playerInRange = Physics.Raycast(ray, out RaycastHit hitInfo, DetectionRadius) && hitInfo.collider.CompareTag(Constants.PlayerTag);

        if (playerInRange)
        {
            Debug.DrawRay(this.transform.position, ray.direction * DetectionRadius, Color.green);
            print("In the light!" + hitInfo.collider.gameObject.name);

            _playerStatus.IsInLight = true;
            _lastDetectedByThisReference = true;
        }
        else if (_lastDetectedByThisReference)
        {
            Debug.DrawRay(this.transform.position, ray.direction * DetectionRadius, Color.red);
            print("Not in the light");

            _playerStatus.IsInLight = false;
            _lastDetectedByThisReference = false;
        }
    }
}
