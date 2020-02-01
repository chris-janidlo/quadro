using System;

public enum MissedHitReason
{
    AlreadyAttemptedBeat,
    NeverAttemptedBeat,
    ClosestBeatIsOff,
    ClosestBeatOutOfRange,
    ComCantCombo,
    ComCantClearAttemptedBeat,
    InvalidCastInput
}

public static class MissedHitReasonExtensions
{
	public static int Damage (this MissedHitReason reason)
	{
		switch (reason)
		{
			case MissedHitReason.NeverAttemptedBeat:
				return 4;
			
			case MissedHitReason.AlreadyAttemptedBeat:
			case MissedHitReason.ClosestBeatIsOff:
			case MissedHitReason.ClosestBeatOutOfRange:
			case MissedHitReason.ComCantClearAttemptedBeat:
				return 1;

			case MissedHitReason.ComCantCombo:
			case MissedHitReason.InvalidCastInput:
				return 2;
			
			default:
				throw new ArgumentException($"unexpected MissedHitReason {reason}");
		}
	}
}
