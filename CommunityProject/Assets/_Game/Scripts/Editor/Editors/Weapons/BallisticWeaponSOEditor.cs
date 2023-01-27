using System;
using BoundfoxStudios.CommunityProject.Editor.Extensions;
using BoundfoxStudios.CommunityProject.Editor.GuiControls;
using BoundfoxStudios.CommunityProject.Weapons.BallisticWeapons.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace BoundfoxStudios.CommunityProject.Editor.Editors.Weapons
{
	[CustomEditor(typeof(BallisticWeaponSO))]
	public class BallisticWeaponSOEditor : WeaponSOEditor
	{
		private SerializedProperty _minimumRangeProperty;
		private SerializedProperty _rotationSpeedInDegreesPerSecondProperty;
		private SerializedProperty _launchAnimationSpeedProperty;
		private SerializedProperty _launchEasingProperty;
		private SerializedProperty _rewindAnimationSpeedProperty;
		private SerializedProperty _rewindEasingProperty;

		private static GUIStyle _helpBoxRichTextStyle;
		private static GUIStyle HelpBoxRichTextStyle => _helpBoxRichTextStyle ??= new(EditorStyles.helpBox)
		{
			richText = true
		};

		protected override string[] ToolbarEntries { get; } = { "Parameters", "Animation" };

		protected override void OnEnable()
		{
			base.OnEnable();

			_minimumRangeProperty = serializedObject.FindRealProperty(nameof(BallisticWeaponSO.MinimumRange));
			_rotationSpeedInDegreesPerSecondProperty =
				serializedObject.FindRealProperty(nameof(BallisticWeaponSO.RotationSpeedInDegreesPerSecond));
			_launchAnimationSpeedProperty = serializedObject.FindRealProperty(nameof(BallisticWeaponSO.LaunchAnimationSpeed));
			_launchEasingProperty = serializedObject.FindRealProperty(nameof(BallisticWeaponSO.LaunchEasing));
			_rewindAnimationSpeedProperty = serializedObject.FindRealProperty(nameof(BallisticWeaponSO.RewindAnimationSpeed));
			_rewindEasingProperty = serializedObject.FindRealProperty(nameof(BallisticWeaponSO.RewindEasing));
		}

		protected override void RenderToolbar(int selectedToolbar)
		{
			switch (selectedToolbar)
			{
				case 0:
					RenderParameters();
					break;

				case 1:
					RenderAnimation();
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(selectedToolbar));
			}
		}

		private void RenderParameters()
		{
			EditorGUILayout.PropertyField(FireRateInSecondsProperty);
			AttackAngleControl.DrawEditorGUILayout(AttackAngleProperty);
			RangeControl.DrawEditorGUILayout(_minimumRangeProperty, RangeProperty);
		}

		private void RenderAnimation()
		{
			EditorGUILayout.PropertyField(_rotationSpeedInDegreesPerSecondProperty);
			EditorGUILayout.PropertyField(_launchAnimationSpeedProperty);
			EditorGUILayout.PropertyField(_launchEasingProperty);
			EditorGUILayout.PropertyField(_rewindAnimationSpeedProperty);
			EditorGUILayout.PropertyField(_rewindEasingProperty);

			var launchAnimationSpeed = _launchAnimationSpeedProperty.GetValue<float>();
			var rewindAnimationSpeed = _rewindAnimationSpeedProperty.GetValue<float>();
			var fireRatePerSeconds = FireRateInSecondsProperty.GetValue<float>();

			var animationTime = launchAnimationSpeed + rewindAnimationSpeed;
			var idleTime = fireRatePerSeconds - animationTime;

			if (idleTime < 0)
			{
				EditorGUILayout.HelpBox($"Careful! The total time of animation " +
				                        $"({nameof(BallisticWeaponSO.LaunchAnimationSpeed)} + {nameof(BallisticWeaponSO.RewindAnimationSpeed)}; " +
				                        $"{launchAnimationSpeed:F2} s + {rewindAnimationSpeed:F2} s = {animationTime:F2} s) " +
				                        $"must be lower or equal than {nameof(BallisticWeaponSO.FireRateInSeconds)} ({fireRatePerSeconds:F2} s)", MessageType.Error);
				return;
			}

			EditorGUILayout.TextArea($"<b>Idle Time:</b> {idleTime:F2} s\n" +
			                        $"<b>Animation Time:</b> {animationTime:F2} s", HelpBoxRichTextStyle);
		}
	}
}
