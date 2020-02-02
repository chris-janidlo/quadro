public class HealthBar : ResourceBar, IDriverSubscriber
{
	public ADriver Driver { get; set; }

	protected override float currentValue => Driver.Player.Health.Value;
	protected override float maxValue => Driver.Player.Health.Max;
}
