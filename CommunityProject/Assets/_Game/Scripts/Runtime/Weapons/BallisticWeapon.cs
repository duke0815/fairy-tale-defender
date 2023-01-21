using System;
using System.Threading;
using BoundfoxStudios.CommunityProject.Weapons.Bullets;
using BoundfoxStudios.CommunityProject.Weapons.ScriptableObjects;
using BoundfoxStudios.CommunityProject.Weapons.Targeting;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace BoundfoxStudios.CommunityProject.Weapons
{
	/// <summary>
	/// Class for all ballistic weapons, like a catapult.
	/// </summary>
	[AddComponentMenu(Constants.MenuNames.Weapons + "/" + nameof(BallisticWeapon))]
	public class BallisticWeapon : Weapon<BallisticWeaponSO>
	{
		[field: SerializeField]
		public ArmSettings Arm { get; private set; }

		[SerializeField]
		private BallisticProjectile ProjectilePrefab;

		private BallisticProjectile _projectile;
		private Quaternion _targetRotation;

		[Serializable]
		public class ArmSettings
		{
			public Transform ArmPivot;
			public Transform LaunchPoint;

			[Tooltip("Defines the X start and X end rotation for the arm.")]
			public Vector2 XRotation;
		}

		private void Awake()
		{
			PrepareProjectile();
		}

		private void PrepareProjectile()
		{
			_projectile = Instantiate(ProjectilePrefab, Arm.LaunchPoint.transform);

			// Ignoring collision between tower and projectile collider, otherwise the physics engine might move the
			// projectile due to the collider being in another collider.
			// We will not change the layer collision matrix here, because projectiles may collide with a tower
			// where we want to destroy it.
			Physics.IgnoreCollision(Tower.Collider, _projectile.Collider);
		}

		protected override UniTask LaunchProjectileAsync(Vector3 target, CancellationToken cancellationToken)
		{
			var launchVelocity = BallisticCalculationUtilities
				.CalculateBallisticArcVelocity(Arm.LaunchPoint.position, target, transform.forward);

			_projectile.Launch(launchVelocity);
			_projectile = null;

			return UniTask.CompletedTask;
		}

		protected override async UniTask StartAnimationAsync(CancellationToken cancellationToken) => await Arm.ArmPivot
			.DOLocalRotate(new(Arm.XRotation.y, 0, 0), WeaponDefinition.LaunchAnimationSpeed)
			.SetEase(WeaponDefinition.LaunchEasing)
			.WithCancellation(cancellationToken);

		protected override async UniTask RewindAnimationAsync(CancellationToken cancellationToken) =>
			await Arm.ArmPivot.DOLocalRotate(new(Arm.XRotation.x, 0, 0), WeaponDefinition.RewindAnimationSpeed)
				.SetEase(WeaponDefinition.RewindEasing)
				.OnComplete(PrepareProjectile)
				.WithCancellation(cancellationToken);

		protected override void TrackTarget(TargetPoint target)
		{
			var direction = target.transform.position - transform.position;
			direction.y = 0;

			var rotation = Quaternion.LookRotation(direction);
			_targetRotation = rotation;
		}

		protected override void Update()
		{
			base.Update();

			transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation,
				WeaponDefinition.RotationSpeedInDegreesPerSecond * Time.deltaTime);
		}

		protected override float CalculateLaunchAnimationDelay() => WeaponDefinition.LaunchAnimationSpeed + WeaponDefinition.RewindAnimationSpeed;
	}
}