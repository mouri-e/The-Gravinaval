using UnityEngine;
public class LaserBehavior : MonoBehaviour
{
    public float onOffTime = 10f;
    public float laserStartTime = 0f;
    private LineRenderer laserLine;
    private bool off = true;
    public ParticleSystem particles;

    void Awake()
    {
        laserLine = gameObject.GetComponent<LineRenderer>();
        InvokeRepeating("TurnOnOff", laserStartTime, onOffTime);
    }

    // Update is called once per frame
    void Update()
    {
        laserLine.SetPosition(0, transform.position);
        RaycastHit laserHit;
        Physics.Raycast(transform.position, transform.up, out laserHit);
        laserLine.SetPosition(1, laserHit.point);

        if (laserHit.collider && laserHit.collider.gameObject.CompareTag("Player") && !off) {
            PlayerController pc = laserHit.collider.gameObject.GetComponent<PlayerController>();
            pc.Kill();
        }

        laserLine.enabled = !off;
        if (!off && !particles.isPlaying) particles.Play(); else if (!particles.isStopped) particles.Stop();
    }

    void TurnOnOff()
    {
        off = !off;
    }
}
