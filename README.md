# Zombie-Apocolypse---3D


<font size="+20"> **Game Mechanics** </font>

|     **GameManager**       |    **Player-World Interaction**   |   **Player-Item Instruction**  |
| ------------------------  |  -------------------------------- |  ---------------------------   |
| *MenuInteractionEvent*    | *InventoryChangeEvent*            | *ItemThrowAndPickEvent*        |
| *InventoryUpdateEvent*    | *PlayerHandIntractionEvent*       | *ItemPlayerIntractionEvent*    |
| *RestartLevelEvent*       | *AmmoIntractionEvent*             | *ItemAnimationSystem*          |
| *GameOverEvent*           | *PlayerHealthUpdateEvent*         |                                |
| *SavePreviousDataEvent*   | *PlayerMechanicsScriptEvent*      |                                |
| *ItemInteractionEvent*    |                                   |                                |

<font size="+20"> **NPC_Mechanics** </font>
- *Patrol*
- *Follow*
- *Alert*
- *Pursue*
- *Flee*
- *Struck*
- *InvestigateHarm*
- *MeleeAttack*
- *RangeAttack*

 <font size="+20"> **NPC-Player-Interaction**</font>
 - *ChangeHealth*
 - *TurnOffAnimator*
 - *TurnOffNavAgent*
 - *TurnOffStatePattern*
 - *TakeDamageEvent*
 - *RagdollActivation*
 - *DropItem*
 - *HeadLookToPlayer*
 - *HoldRangeOrMeleeWeapon*


<font size="+20"> **Gun System**</font>
 - *PlayerInput*
 - *NPCInput*
 - *ReloadRequest*
 - *GunReset*
 - *BurstModeToggle*
 - *ChangeAmmoCount*
 - *DynamicCrosshairSetup*
 - *ApplyDamge*
 - *ShotDefault*
 - *ShotEnemy*
 - *ChangeAmooCountUI*


 <font size="+20"> **Destructible-Items**</font>
 - *DeductHealth*
 - *DestroyMe*
 - *EventHealthLow*
 - *TakeDamage*
 - *Expolde*
 - *LowHealthEffect*
 - *DestructibleDegenerateToDie*
 - *InventoryUpdate*
