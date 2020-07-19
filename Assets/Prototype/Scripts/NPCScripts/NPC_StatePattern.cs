using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace GM
{
    public class NPC_StatePattern : MonoBehaviour
    {
        //Used in descision making
        private float checkRate = 0.1f;
        private float NextCheck;

        public float sightRange = 40;
        public float detectBehindRange = 5;
        public float meleeAttackRange = 7;
        public float meleeAttackDamage = 10;
        public float rangeAttackRange = 35;
        public float rangeAttackDamage = 5;
        public float rangeAttackSpread = 0.5f;
        public float attackRate = 0.4f;
        public float nextAttack;
        public float fleeRange = 25;
        public float offset = 0.4f;

        public int requirdDetectionCount = 15;

        public bool hasRangeAttack;
        public bool hasMeleeAttack;
        public bool isMeleeAttacking;

        public Transform myFollowTarget;
        [HideInInspector] public Transform pursueTarget;
        [HideInInspector] public Vector3 locationOfIntrest;
        [HideInInspector] public Vector3 wanderTarget;
        [HideInInspector] public Transform myAttacker;

        //Used For sight
        public LayerMask sightLayers;
        public LayerMask myEnemyLayers;
        public LayerMask myFriendlyLayers;

        public string[] myEnemyTags;
        public string[] myFriendlyTags;

        //References
        public Transform[] waypoints;
        public Transform head;
        public MeshRenderer meshRendererFlag;
        public GameObject rangeWeapon;
        public NPC_Master npcMaster;
        [HideInInspector] public NavMeshAgent myNavMeshAgent;

        //Used for state AI
        public NPCState_Interface currentState;
        public NPCState_Interface captureState;
        public NPCState_Patrol patrolState;
        public NPCState_Alert alertState;
        public NPCState_Pursue pursurState;
        public NPCState_MeleeAttack meleeAttackState;
        public NPCState_RangeAttack rangeAttackState;
        public NPCState_Flee fleeState;
        public NPCState_Struck struckState;
        public NPCState_InvestigateHarm investigateHarmState;
        public NPCState_Follow followState;

        private void Awake()
        {
            SetupStateReferences();
            SetInitialReferences();
            npcMaster.EventNpcLowHealth += ActivateFleeState;
            npcMaster.EventNpcRecoveredAnim += ActivatePatrolState;
            npcMaster.EventNpcDeductHealth += ActivateStruckState;
        }
        private void Start()
        {
            SetInitialReferences();
        }
        private void OnDisable()
        {
            npcMaster.EventNpcLowHealth -= ActivateFleeState;
            npcMaster.EventNpcRecoveredAnim -= ActivatePatrolState;
            npcMaster.EventNpcDeductHealth -= ActivateStruckState;
            StopAllCoroutines();
        }
        private void Update()
        {
            CarryOutUpdateState();
        }
        private void SetupStateReferences()
        {
            patrolState = new NPCState_Patrol(this);
            alertState = new NPCState_Alert(this);
            pursurState = new NPCState_Pursue(this);
            fleeState = new NPCState_Flee(this);
            followState = new NPCState_Follow(this);
            meleeAttackState = new NPCState_MeleeAttack(this);
            rangeAttackState = new NPCState_RangeAttack(this);
            struckState = new NPCState_Struck(this);
            investigateHarmState = new NPCState_InvestigateHarm(this);
        }

        private void SetInitialReferences()
        {
            myNavMeshAgent = GetComponent<NavMeshAgent>();
            ActivatePatrolState();
        }
        private void CarryOutUpdateState()
        {
            if (Time.time > NextCheck)
            {
                NextCheck = Time.time + checkRate;
                currentState.UpdateState();
            }
        }
        private void ActivatePatrolState()
        {
            currentState = patrolState;
        }
        private void ActivateFleeState()
        {
            if (currentState == struckState)
            {
                captureState = fleeState;
                return;
            }
            currentState = fleeState;
        }
        private void ActivateStruckState(int dummy)
        {
            StopAllCoroutines();
            if (currentState != struckState)
            {
                captureState = currentState;
            }
            if (rangeWeapon != null)//Change or Remove if you have proper gun  holding animations
            {
                rangeWeapon.SetActive(false);
            }
            if (myNavMeshAgent.enabled)
            {
                myNavMeshAgent.isStopped = true;
            }
            currentState = struckState;
            isMeleeAttacking = false;
            npcMaster.CallEventNpcStruckAnim();
            StartCoroutine(RecoverFromStruckState());
        }

        IEnumerator RecoverFromStruckState()
        {
            yield return new WaitForSeconds(1.5f);
            npcMaster.CallEventNpcRecoveredAnim();
            if (rangeWeapon != null)
            {
                rangeWeapon.SetActive(true);
            }
            if (myNavMeshAgent.enabled)
            {
                myNavMeshAgent.isStopped = false; ;
            }
            currentState = captureState;
        }

        public void OnEnemyAttack()//Called by melee attack animation
        {
            if (pursueTarget != null)
            {
                if (Vector3.Distance(transform.position, pursueTarget.position) <= meleeAttackRange)
                {
                    Vector3 toOther = pursueTarget.position - transform.position;
                    if (Vector3.Dot(toOther, transform.forward) > 0.5f)
                    {
                        pursueTarget.SendMessage("CallEventPlayerHealthDeduction", meleeAttackDamage, SendMessageOptions.DontRequireReceiver);
                        pursueTarget.SendMessage("ProcessDamage", meleeAttackDamage, SendMessageOptions.DontRequireReceiver);
                        pursueTarget.SendMessage("SetMyAttacker", transform.root, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            isMeleeAttacking = false;
        }

        //public void SetMyAttacker(Transform attacker)
        //{
        //    myAttacker = attacker;
        //}

        public void Distract(Vector3 distractionPos)
        {
            locationOfIntrest = distractionPos;
            if (currentState == patrolState)
            {
                currentState = alertState;
            }
        }
    }
}
