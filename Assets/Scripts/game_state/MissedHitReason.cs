using System;

public enum MissedHitReason
{
    AlreadyAttemptedBeat,
    NeverAttemptedBeat,
    ClosestBeatIsOff,
    ClosestBeatOutOfRange
}

public static class MissedHitReasonExtensions
{
	public static int Damage (this MissedHitReason reason)
	{
		return reason == MissedHitReason.NeverAttemptedBeat
			? 0
			: 6;
	}
}
