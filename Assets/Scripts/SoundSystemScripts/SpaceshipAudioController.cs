using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipAudioController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody targetRigidbody;

    [Header("Ship Audio Sources")]
    [SerializeField] private AudioSource mainEngineSource;
    [SerializeField] private AudioSource strafeSource;
    [SerializeField] private AudioSource verticalSource;
    [SerializeField] private AudioSource rotateSource;

    [Header("Loop Clips")]
    [SerializeField] private AudioClip mainEngineClip;
    [SerializeField] private AudioClip strafeClip;
    [SerializeField] private AudioClip verticalClip;
    [SerializeField] private AudioClip rotateClip;

    [Header("Speed Ranges")]
    [Tooltip("Forward/back local speed that counts as full engine intensity.")]
    [SerializeField] private float maxForwardSpeed = 20f;

    [Tooltip("Left/right local speed that counts as full strafe intensity.")]
    [SerializeField] private float maxStrafeSpeed = 12f;

    [Tooltip("Up/down local speed that counts as full vertical intensity.")]
    [SerializeField] private float maxVerticalSpeed = 12f;

    [Tooltip("Angular speed in radians/sec around local Y that counts as full rotation intensity.")]
    [SerializeField] private float maxYawAngularSpeed = 2.5f;

    [Header("Volume")]
    [SerializeField] private float mainMinVolume = 0.02f;
    [SerializeField] private float mainMaxVolume = 0.15f;
    [SerializeField] private float strafeMinVolume = 0f;
    [SerializeField] private float strafeMaxVolume = 0.15f;
    [SerializeField] private float verticalMinVolume = 0f;
    [SerializeField] private float verticalMaxVolume = 0.15f;
    [SerializeField] private float rotateMinVolume = 0f;
    [SerializeField] private float rotateMaxVolume = 0.1f;

    [Header("Pitch")]
    [SerializeField] private float mainMinPitch = 0.65f;
    [SerializeField] private float mainMaxPitch = 1f;
    [SerializeField] private float strafeMinPitch = 0.7f;
    [SerializeField] private float strafeMaxPitch = 0.95f;
    [SerializeField] private float verticalMinPitch = 0.7f;
    [SerializeField] private float verticalMaxPitch = 0.95f;
    [SerializeField] private float rotateMinPitch = 0.7f;
    [SerializeField] private float rotateMaxPitch = 0.9f;

    [Header("Response")]
    [SerializeField] private float volumeLerpSpeed = 6f;
    [SerializeField] private float pitchLerpSpeed = 6f;

    private void Reset()
    {
        targetRigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        if (targetRigidbody == null)
            targetRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SetupLoopSource(mainEngineSource, mainEngineClip, mainMinVolume, mainMinPitch);
        SetupLoopSource(strafeSource, strafeClip, strafeMinVolume, strafeMinPitch);
        SetupLoopSource(verticalSource, verticalClip, verticalMinVolume, verticalMinPitch);
        SetupLoopSource(rotateSource, rotateClip, rotateMinVolume, rotateMinPitch);
    }

    private void Update()
    {
        if (targetRigidbody == null)
            return;

        Vector3 localVelocity = transform.InverseTransformDirection(targetRigidbody.linearVelocity);
        Vector3 localAngularVelocity = transform.InverseTransformDirection(targetRigidbody.angularVelocity);

        float forwardAmount = Mathf.Clamp01(Mathf.Abs(localVelocity.z) / Mathf.Max(0.01f, maxForwardSpeed));
        float strafeAmount = Mathf.Clamp01(Mathf.Abs(localVelocity.x) / Mathf.Max(0.01f, maxStrafeSpeed));
        float verticalAmount = Mathf.Clamp01(Mathf.Abs(localVelocity.y) / Mathf.Max(0.01f, maxVerticalSpeed));
        float rotateAmount = Mathf.Clamp01(Mathf.Abs(localAngularVelocity.y) / Mathf.Max(0.01f, maxYawAngularSpeed));

        UpdateLoop(mainEngineSource, forwardAmount, mainMinVolume, mainMaxVolume, mainMinPitch, mainMaxPitch);
        UpdateLoop(strafeSource, strafeAmount, strafeMinVolume, strafeMaxVolume, strafeMinPitch, strafeMaxPitch);
        UpdateLoop(verticalSource, verticalAmount, verticalMinVolume, verticalMaxVolume, verticalMinPitch, verticalMaxPitch);
        UpdateLoop(rotateSource, rotateAmount, rotateMinVolume, rotateMaxVolume, rotateMinPitch, rotateMaxPitch);
    }

    private void SetupLoopSource(AudioSource source, AudioClip clip, float startVolume, float startPitch)
    {
        if (source == null)
        {
            Debug.LogWarning("A ship AudioSource is missing.", this);
            return;
        }

        if (clip == null)
        {
            Debug.LogWarning($"Missing clip for source {source.name}", this);
            return;
        }

        source.clip = clip;
        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.volume = startVolume;
        source.pitch = startPitch;
        source.Play();
    }

    private void UpdateLoop(
        AudioSource source,
        float amount,
        float minVolume,
        float maxVolume,
        float minPitch,
        float maxPitch)
    {
        if (source == null)
            return;

        float targetVolume = Mathf.Lerp(minVolume, maxVolume, amount);
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, amount);

        source.volume = Mathf.Lerp(source.volume, targetVolume, Time.deltaTime * volumeLerpSpeed);
        source.pitch = Mathf.Lerp(source.pitch, targetPitch, Time.deltaTime * pitchLerpSpeed);
    }
}