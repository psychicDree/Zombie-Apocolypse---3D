using UnityEngine;

namespace GM
{
	public class GameManager_Master : MonoBehaviour
	{
		public delegate void GameManagerEventHandler();
		public event GameManagerEventHandler MenuToggleEvent;
		public event GameManagerEventHandler InventoryToggleEvent;
		public event GameManagerEventHandler RestartLevelEvent;
		public event GameManagerEventHandler GotoMenuSceneEvent;
		public event GameManagerEventHandler GameOverEvent;

		public bool isGameOver;
		public bool isInventoryUIOn;
		public bool isMenuOn;

		public void CallEventMenuToggle()
		{
			if (MenuToggleEvent != null)
			{
				MenuToggleEvent();
			}
		}

		public void CallInventoryToggleEvent()
		{
			if (InventoryToggleEvent != null)
			{
				InventoryToggleEvent();
			}
		}

		public void CallRestartLevelEvent()
		{
			if (RestartLevelEvent != null)
			{
				RestartLevelEvent();
			}
		}

		public void CallGotoMenuSceneEvent()
		{
			if (GotoMenuSceneEvent != null)
			{
				GotoMenuSceneEvent();
			}
		}

		public void CallEventGameOver()
		{
			if (!isGameOver)
			{
				if (GameOverEvent != null)
				{
					isGameOver = true;
					GameOverEvent();
				}
			}
		}
	}
}