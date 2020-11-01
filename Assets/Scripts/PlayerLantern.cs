using UnityEngine;

public class PlayerLantern : MonoBehaviour
{
    public Light LanternLight;
    public float LanternLightIntensity = 1.58f;
    public float LanternLightFadeInSpeed = 3f;
    public float LanternLightFadeOutSpeed = 3f;
    public bool LanternOn;

    private PlayerStatuses _playerStatuses;

    private void Start()
    {
        _playerStatuses = FindObjectOfType<PlayerStatuses>();       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            LanternOn = !LanternOn;
            _playerStatuses.IsInLight = LanternOn;
        }

        if (LanternOn && LanternLight.intensity < LanternLightIntensity)
        {
            LanternLight.intensity = Mathf.Lerp(LanternLight.intensity, LanternLightFadeOutSpeed, LanternLightFadeInSpeed * Time.deltaTime);
        }
        else if (!LanternOn && LanternLight.intensity > 0)
        {
            LanternLight.intensity = Mathf.Lerp(LanternLight.intensity, 0, LanternLightFadeOutSpeed * Time.deltaTime);
        }
    }
}
