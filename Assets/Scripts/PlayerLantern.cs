using UnityEngine;

public class PlayerLantern : MonoBehaviour
{
    public Light LanternLight;
    public float LanternLightIntensity = 1.58f;
    public float LanternLightFadeInSpeed = 3f;
    public float LanternLightFadeOutSpeed = 3f;

    private PlayerStatuses _playerStatuses;
    private bool _lanternOn;

    private void Start()
    {
        _playerStatuses = FindObjectOfType<PlayerStatuses>();       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _lanternOn = !_lanternOn;
            _playerStatuses.IsInLight = _lanternOn;
        }

        if (_lanternOn && LanternLight.intensity < LanternLightIntensity)
        {
            LanternLight.intensity = Mathf.Lerp(LanternLight.intensity, LanternLightFadeOutSpeed, LanternLightFadeInSpeed * Time.deltaTime);
        }
        else if (!_lanternOn && LanternLight.intensity > 0)
        {
            LanternLight.intensity = Mathf.Lerp(LanternLight.intensity, 0, LanternLightFadeOutSpeed * Time.deltaTime);
        }
    }
}
