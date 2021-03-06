﻿//----------------------------------------------
//            NGUI: Next-Gen UI kit Extension
// Copyright © 2017 Car3man
//----------------------------------------------

using UnityEngine;

[AddComponentMenu("NGUI_Extension/Tween/Tween Alpha")]
public class TweenSpriteAlpha : UITweener {
    public float from;
    public float to;
    public bool updateTable = false;

    SpriteRenderer mSpriteRenderer;

    public SpriteRenderer cachedSpriteRenderer { get { if(mSpriteRenderer == null) mSpriteRenderer = GetComponent<SpriteRenderer>(); return mSpriteRenderer; } }

    public float value { get { if(cachedSpriteRenderer != null) return cachedSpriteRenderer.color.a; return 0; } set { if(cachedSpriteRenderer != null) cachedSpriteRenderer.color = new Color(cachedSpriteRenderer.color.r, cachedSpriteRenderer.color.g, cachedSpriteRenderer.color.b, value); } }

    [System.Obsolete("Use 'value' instead")]
    public float alpha { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) {
        value = from * (1f - factor) + to * factor;
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenSpriteAlpha Begin(GameObject go, float duration, float alpha) {
        TweenSpriteAlpha comp = UITweener.Begin<TweenSpriteAlpha>(go, duration);
        comp.from = comp.value;
        comp.to = alpha;

        if(duration <= 0f) {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue() { from = value; }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue() { to = value; }

    [ContextMenu("Assume value of 'From'")]
    void SetCurrentValueToStart() { value = from; }

    [ContextMenu("Assume value of 'To'")]
    void SetCurrentValueToEnd() { value = to; }
}
