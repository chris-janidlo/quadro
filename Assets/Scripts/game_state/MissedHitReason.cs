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
			case MissedHitReason.ClosestBeatIsOff:
			case MissedHitReason.ClosestBeatOutOfRange:
				return 6;

			case MissedHitReason.AlreadyAttemptedBeat:
			case MissedHitReason.ComCantClearAttemptedBeat:
			case MissedHitReason.ComCantCombo:
			case MissedHitReason.InvalidCastInput:
				return 3;
			
			default:
				throw new ArgumentException($"unexpected MissedHitReason {reason}");
		}
	}
}
