using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    public float rotationSpeed = 30;
    public GameObject GemIcon;
    public int GemIndex;
    public LevelCompleteBehavior complete;

    void Start()
    {
        if (GemIcon != null)
            GemIcon.SetActive(false);
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("pickupDestroyed");
            AudioSource collectSound = GetComponent<AudioSource>();
            collectSound.Play();
            if (GemIcon != null)
                GemIcon.SetActive(true);
            complete.SaveGem(GemIndex);
            Destroy(gameObject, 2);
        }
    }
}

