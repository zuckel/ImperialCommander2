﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Saga
{
	public class GroupMugshotToggle : MonoBehaviour
	{
		bool _isOn;

		public Image mugImage, bgImage;

		[HideInInspector]
		public bool isOn
		{
			get { return _isOn; }
			set { _isOn = value; }
		}
		DeploymentCard card;
		int dataMode;

		//mode 0=enemies, 1=villains
		public void Init( DeploymentCard cd, int mode )
		{
			dataMode = mode;
			card = cd;
			if ( mode == 0 )
				mugImage.sprite = Resources.Load<Sprite>( $"Cards/Enemies/{cd.expansion}/{cd.id.Replace( "DG", "M" )}" );
			else
				mugImage.sprite = Resources.Load<Sprite>( $"Cards/Villains/{cd.id.Replace( "DG", "M" )}" );

			if ( cd.isElite )
				mugImage.color = new Color( 1, 40f / 255f, 0 );
			isOn = false;
		}

		public void UpdateToggle()
		{
			EventSystem.current.SetSelectedGameObject( null );
			if ( isOn )
				bgImage.color = Color.green;
			else
			{
				bgImage.color = Color.white;
				isOn = false;
			}

			FindObjectOfType<SagaModifyGroupsPanel>().UpdateExpansionCounts();
		}

		public void OnToggle()
		{
			EventSystem.current.SetSelectedGameObject( null );
			isOn = !isOn;
			if ( isOn && !FindObjectOfType<SagaModifyGroupsPanel>().OnToggle( card ) )
				isOn = false;
			if ( !isOn )
			{
				if ( dataMode == 0 )
					DataStore.sagaSessionData.MissionIgnored.Remove( card );
				else
					DataStore.sagaSessionData.EarnedVillains.Remove( card );
			}

			UpdateToggle();
		}
	}
}
