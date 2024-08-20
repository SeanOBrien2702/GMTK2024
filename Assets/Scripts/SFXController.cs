using UnityEngine;

public class SFXController : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    public static SFXController Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    AudioSource audioSource;
    private float volume = 1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }

    private void Start()
    {
        DeliveryController.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryController.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        PlayerController.OnPickUp += Player_OnPickedUp;
        Counter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.trash);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectDrop);
    }

    private void Player_OnPickedUp(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.chop);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess);
    }

    private void PlaySound(AudioClip[] audioClipArray, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], volume);
    }

    private void PlaySound(AudioClip audioClip, float volumeMultiplier = 1f)
    {
        audioSource.PlayOneShot(audioClip, volumeMultiplier * volume);
    }

    public void PlayFootstepsSound(float volume = 1)
    {
        PlaySound(audioClipRefsSO.footstep, volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefsSO.warning);
    }

    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}