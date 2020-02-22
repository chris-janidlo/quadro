using System;

public enum MissedHitReason
{
    AlreadyAttemptedBeat,
    NeverAttemptedBeat,
    ClosestBeatIsOff,
    ClosestBeatOutOfRange,
    CommandCantCombo,
	AlreadyOnCPU,
    CPUHasNoInstr
}

public static class MissedHitReasonExtensions
{
	public static int Damage (this MissedHitReason reason)
	{
		switch (reason)
		{
			case MissedHitReason.NeverAttemptedBeat:
			case MissedHitReason.ClosestBeatIsOff:
			case MissedHitReason.ClosestBeatOutOfRange:
				return 6;

			case MissedHitReason.AlreadyAttemptedBeat:
			case MissedHitReason.AlreadyOnCPU:
			case MissedHitReason.CommandCantCombo:
			case MissedHitReason.CPUHasNoInstr:
				return 3;
			
			default:
				throw new ArgumentException($"unexpected MissedHitReason {reason}");
		}
	}
}
