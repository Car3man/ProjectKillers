using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Extensions;
using UnityEngine;

public class Player : NetworkMissionObject, IHumanObject {
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveError;
    [SerializeField] protected Animator feetAnimator;
    [SerializeField] protected GameObject bulletPoint;
    [SerializeField] protected ParticleSystem bulletEffect;

    public override void Update() {
        Vector3 lastPosition = transform.position;
        base.Update();
        Animate(transform.position - lastPosition);
        HandleInput();
    }

    private void HandleInput() {
        if (!IsOwn) return;

        if (Input.GetMouseButtonDown(0)) {
            RequestShoot();
        }

        NetDataRequest data = new NetDataRequest(RequestTypes.SyncPlayer, new System.Collections.Generic.Dictionary<string, ObjectWrapper>());
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));

        data.Values["id"] = new ObjectWrapper<string>(ID);
        data.Values["position"] = new ObjectWrapper<Vector3K>((transform.position + ((Vector3.up * Input.GetAxis("Vertical")) + (Vector3.right * Input.GetAxis("Horizontal"))) * moveSpeed * Time.deltaTime).ToServerVector3());
        data.Values["eulerAngles"] = new ObjectWrapper<Vector3K>((new Vector3(0, 0, Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg)).ToServerVector3());

        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(data));
    }

    private void RequestShoot() {
        NetDataRequest data = new NetDataRequest(RequestTypes.Shoot, new System.Collections.Generic.Dictionary<string, ObjectWrapper>());
        data.Values["id"] = new ObjectWrapper<string>(ID);
        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(data), GameManager.I.ShootPlayerNetworkID);
    }

    public void DoShoot(string id) {
        AudioManager.I.PlayAsAudioSource("Sfx/Shoot");

        if (bulletEffect.isPlaying) {
            bulletEffect.Stop();
            bulletEffect.Play();
        } else {
            bulletEffect.Play();
        }
    }

    public void Animate(Vector3 delta) {
        feetAnimator.SetBool("Run", delta.magnitude > moveError);
    }

    public void SyncHealth(int health, int maxHealth) {
        if (IsOwn) {
            if (health <= 0) Debug.Log("Are you dead");
        }
    }
}
