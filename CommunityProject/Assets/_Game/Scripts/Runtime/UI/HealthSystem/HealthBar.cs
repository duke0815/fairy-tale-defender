using BoundfoxStudios.CommunityProject.HealthSystem;
using BoundfoxStudios.CommunityProject.UI.Utility;
using UnityEngine;

namespace BoundfoxStudios.CommunityProject.UI.HealthSystem
{
	[AddComponentMenu(Constants.MenuNames.UI + "/" + nameof(HealthBar))]
	public class HealthBar : MonoBehaviour
	{
		[field: SerializeField]
		private Bar Bar { get; set; } = default!;

		[field: SerializeField]
		private Health Health { get; set; } = default!;

		private void Start()
		{
			Bar.Initialize(Health.Current, Health.Maximum);
		}

		private void OnEnable()
		{
			Health.Dead += Dead;
			Health.Change += Change;
		}

		private void OnDisable()
		{
			Health.Dead -= Dead;
			Health.Change -= Change;
		}

		private void Change(int current, int change)
		{
			Bar.Value = current;
		}

		private void Dead()
		{
			Bar.Value = 0;
		}
	}
}