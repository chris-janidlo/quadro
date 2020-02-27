using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RealtimeDriver : ADriver, QuadroInput.IPlayerActions
{
    public AudioSource TimingSource;

    public int TimingClipBPM, TimingClipMeasureLength;

    float inverseAudioFrequency;

    public override void Initialize (int seed)
    {
        base.Initialize(seed);

        TimingSource.Play();
    }

    void Update ()
    {
        if (!initialized) return;

        Player.Track.CurrentPositionInMeasure = (float) TimingSource.timeSamples * TimingClipBPM / 60 / TimingSource.clip.frequency;
        TimingSource.pitch = (float) Player.Track.BPM / TimingClipBPM * 4 / Track.BEATS_PER_MEASURE;
    }

    public void OnCommandC (InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        Player.DoInput(new InputFrame(CommandInput.C, null));
    }

    public void OnCommandD (InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        Player.DoInput(new InputFrame(CommandInput.D, null));
    }

    public void OnCommandE (InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        Player.DoInput(new InputFrame(CommandInput.E, null));
    }

    public void OnCommandF (InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        Player.DoInput(new InputFrame(CommandInput.F, null));
    }

    public void OnCommandG (InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        Player.DoInput(new InputFrame(CommandInput.G, null));
    }

    public void OnCommandA (InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        Player.DoInput(new InputFrame(CommandInput.A, null));
    }

    public void OnCommandB (InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        Player.DoInput(new InputFrame(CommandInput.B, null));
    }

    // assumes input is diamond-shaped
    public void OnCPUSelect (InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        Vector2 dir = context.ReadValue<Vector2>();

        if (dir.x == -1)
        {
            Player.DoInput(new InputFrame(null, CPUSwitchInput.Zero));
        }
        else if (dir.y == -1)
        {
            Player.DoInput(new InputFrame(null, CPUSwitchInput.One));
        }
        else if (dir.x == 1)
        {
            Player.DoInput(new InputFrame(null, CPUSwitchInput.Two));
        }
        else if (dir.y == 1)
        {
            Player.DoInput(new InputFrame(null, CPUSwitchInput.Three));
        }
    }
}
