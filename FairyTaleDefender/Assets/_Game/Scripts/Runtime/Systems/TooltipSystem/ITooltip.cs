using BoundfoxStudios.FairyTaleDefender.Entities.Buildings.Towers.ScriptableObjects;
using BoundfoxStudios.FairyTaleDefender.Entities.Weapons.ScriptableObjects;
using UnityEngine.Localization;

namespace BoundfoxStudios.FairyTaleDefender.Systems.TooltipSystem
{
	public interface ITooltip
	{
		// Marker.
	}

	public interface ITextTooltip : ITooltip
	{
		LocalizedString Text { get; }
	}

	public interface IBuildTowerTooltip : ITooltip
	{
		WeaponSO WeaponDefinition { get; }
		TowerSO TowerDefinition { get; }
	}
}
