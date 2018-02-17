using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Extensions;
using SwiftKernelUnity.Core;
using UnityEngine;

public class Player : MonoBehaviour {
    public string ID;

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveError;
    [SerializeField] protected Animator feetAnimator;
    [SerializeField] protected GameObject bulletPoint;
    [SerializeField] protected ParticleSystem bulletEffect;
    [SerializeField] protected bool IsInterpolate = true;
    [SerializeField] protected float PowerInterpolate = 20F;

    private Vector3 newPosition;
    private Vector3 newEulerAngles;

    private void Start() {
        newPosition = transform.position;
        newEulerAngles = transform.eulerAngles;
    }

    private void Update() {
        Vector3 lastPosition = transform.position;

        Interpolating();
        HandleInput();
        Animate(transform.position - lastPosition);
    }

    public void ApplyPosition(Vector3 newPosition) {
        this.newPosition = newPosition;

        if (!IsInterpolate) transform.position = newPosition;
    }

    public void ApplyEulerAngles(Vector3 newEulerAngles) {
        this.newEulerAngles = newEulerAngles;

        if (!IsInterpolate) transform.eulerAngles = newEulerAngles;
    }

    protected virtual void Interpolating() {
        if (IsInterpolate) {
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * PowerInterpolate);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newEulerAngles), Time.deltaTime * PowerInterpolate);
        }
    }

    private void HandleInput() {
        if (!NetManager.I.ID.Equals(ID)) return;

        if (Input.GetMouseButtonDown(0)) {
            RequestShoot();
        }

        NetData data = new NetData(RequestTypes.SyncPlayer, new System.Collections.Generic.Dictionary<string, ObjectWrapper>());
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));

        data.Values["id"] = new ObjectWrapper<string>(NetManager.I.ID);
        data.Values["position"] = new ObjectWrapper<Vector3K>((transform.position + ((Vector3.up * Input.GetAxis("Vertical")) + (Vector3.right * Input.GetAxis("Horizontal"))) * moveSpeed * Time.deltaTime).ToServerVector3());
        data.Values["eulerAngles"] = new ObjectWrapper<Vector3K>((new Vector3(0, 0, Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg)).ToServerVector3());

        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(data), GameManager.I.SyncPlayerNetworkID);
    }

    private void RequestShoot() {
        NetData data = new NetData(RequestTypes.Shoot, new System.Collections.Generic.Dictionary<string, ObjectWrapper>());
        data.Values["id"] = new ObjectWrapper<string>(NetManager.I.ID);
        data.Values["position"] = new ObjectWrapper<Vector3K>(bulletPoint.transform.position.ToServerVector3());
        data.Values["eulerAngles"] = new ObjectWrapper<Vector3K>(bulletPoint.transform.eulerAngles.ToServerVector3());
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

    private void Animate(Vector3 delta) {
        feetAnimator.SetBool("Run", delta.magnitude > moveError);
    }
}
