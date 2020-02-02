public class ArmorBar : ResourceBar, IDriverSubscriber
{
	public ADriver Driver { get; set; }

	protected override float currentValue => Driver.Player.Armor.Value;
	protected override float maxValue => Driver.Player.Armor.Max;
}
