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
				return 0;

			case MissedHitReason.ClosestBeatOutOfRange:
			case MissedHitReason.ClosestBeatIsOff:
			case MissedHitReason.AlreadyAttemptedBeat:
			case MissedHitReason.AlreadyOnCPU:
			case MissedHitReason.CommandCantCombo:
			case MissedHitReason.CPUHasNoInstr:
				return 6;
			
			default:
				throw new ArgumentException($"unexpected MissedHitReason {reason}");
		}
	}
}
