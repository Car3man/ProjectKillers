using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skeleton : NetworkMissionObject, IHumanObject {
    [SerializeField] protected float moveError;
    [SerializeField] protected Animator bodyAnimator;

    [SerializeField] protected Image healthBarFill;

    public override void InitID(string id) {
        base.InitID(id);

        string hitID = NetManager.I.Client.UnityEventReceiver.AddEventObserver(DoHit, false, string.Format("EventZombieHit_{0}", id));
    }

    public override void Update() {
        Vector3 lastPosition = transform.position;
        base.Update();
        Animate(transform.position - lastPosition);
    }

    public void Animate(Vector3 delta) {
        bodyAnimator.SetBool("Run", delta.magnitude > moveError);
    }

    public void DoHit(byte[] data) {
        AudioManager.I.PlayAsAudioSource("Sfx/ZombieAttack");

        bodyAnimator.SetBool("Attack", true);

        StartCoroutine(IEDoHit());
    }

    private IEnumerator IEDoHit() {
        yield return new WaitForSeconds(0.5f);
        bodyAnimator.SetBool("Attack", false);
    }

    public void SyncHealth(int health, int maxHealth) {
        healthBarFill.fillAmount = health / (float)maxHealth;
    }
}
