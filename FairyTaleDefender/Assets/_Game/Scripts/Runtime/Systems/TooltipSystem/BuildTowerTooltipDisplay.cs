using BoundfoxStudios.FairyTaleDefender.Common;
using BoundfoxStudios.FairyTaleDefender.Entities.Weapons.BallisticWeapons.ScriptableObjects;
using BoundfoxStudios.FairyTaleDefender.Entities.Weapons.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace BoundfoxStudios.FairyTaleDefender.Systems.TooltipSystem
{
	[AddComponentMenu(Constants.MenuNames.UI + "/" + nameof(BuildTowerTooltipDisplay))]
	public class BuildTowerTooltipDisplay : TooltipDisplay
	{
		[field: SerializeField]
		private TextMeshProUGUI TowerName { get; set; } = default!;

		[field: SerializeField]
		private TextMeshProUGUI AttackRangeText { get; set; } = default!;

		[field: SerializeField]
		private TextMeshProUGUI FireRatePerSecondsText { get; set; } = default!;

		[field: SerializeField]
		private TextMeshProUGUI AttackAngleText { get; set; } = default!;

		protected override void SetTooltip<T>(T tooltip)
		{
			var resolvedTooltip = ResolveTooltip<IBuildTowerTooltip, T>(tooltip);
			TowerName.text = resolvedTooltip.TowerDefinition.Name.GetLocalizedString();
			AttackRangeText.text = GetRange(resolvedTooltip.WeaponDefinition);

			// FireRate is every seconds, but we'll show fire rate per seconds to the player.
			FireRatePerSecondsText.text = (1 / resolvedTooltip.WeaponDefinition.FireRateEverySeconds).ToString("0.00");
			AttackAngleText.text = resolvedTooltip.WeaponDefinition.AttackAngle.ToString();
		}

		private string GetRange(WeaponSO weapon) => weapon switch
		{
			BallisticWeaponSO ballisticWeapon => $"{ballisticWeapon.MinimumRange:0.##}-{ballisticWeapon.MaximumRange:0.##}",
			_ => weapon.Range.ToString("0.##"),
		};
	}
}
