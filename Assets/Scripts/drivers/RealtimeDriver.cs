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

    bool defense, register;

    CommandZone currentZone => register ? CommandZone.Register : (defense ? CommandZone.Defense : CommandZone.Attack);

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

	public void OnJab (InputAction.CallbackContext context)
	{
        if (context.phase != InputActionPhase.Performed) return;

        Player.RunCommand(currentZone, CommandButton.Jab);
	}

	public void OnKick (InputAction.CallbackContext context)
	{
        if (context.phase != InputActionPhase.Performed) return;

        Player.RunCommand(currentZone, CommandButton.Kick);
	}

	public void OnUtility (InputAction.CallbackContext context)
	{
        if (context.phase != InputActionPhase.Performed) return;

        Player.RunCommand(currentZone, CommandButton.Utility);
	}

	public void OnFinisher (InputAction.CallbackContext context)
	{
        if (context.phase != InputActionPhase.Performed) return;

        Player.RunCommand(currentZone, CommandButton.Finisher);
	}

	public void OnDefenseModifier (InputAction.CallbackContext context)
	{
        if (context.phase != InputActionPhase.Performed) return;

        defense = !defense;
	}

	public void OnRegisterModifier (InputAction.CallbackContext context)
	{
        if (context.phase != InputActionPhase.Performed) return;

        register = !register;
	}

    // assumes input is diamond-shaped
    public void OnCPUSelect (InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        Vector2 dir = context.ReadValue<Vector2>();

        if (dir.x == -1)
        {
            Player.SwitchActiveCPU(CPUSwitchInput.Zero);
        }
        else if (dir.y == -1)
        {
            Player.SwitchActiveCPU(CPUSwitchInput.One);
        }
        else if (dir.x == 1)
        {
            Player.SwitchActiveCPU(CPUSwitchInput.Two);
        }
        else if (dir.y == 1)
        {
            Player.SwitchActiveCPU(CPUSwitchInput.Three);
        }
    }
}
