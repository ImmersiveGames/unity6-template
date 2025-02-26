using System.Collections.Generic;
using System.Linq;
using _ImmersiveGames.Scripts.AdvancedTimers;
using _ImmersiveGames.Scripts.DebugSystems;
using _ImmersiveGames.Scripts.Utils.DependencyInjectSystem;
using UnityEngine;
using UnityEngine.AI;

namespace _ImmersiveGames.Scripts.GoapSystem {
    [RequireComponent(typeof(NavMeshAgent))]
    //[RequireComponent(typeof(AnimationController))]
    public class GoapAgent : MonoBehaviour {
        [Header("Sensors")] 
        [SerializeField]
        private Sensor chaseSensor;
        [SerializeField] private Sensor attackSensor;
        //TODO: aqui tem dois sensores mas pode haver mais aqui é um sensor para começar a perseguir e outro para atacar
        //Para o PEGA provavelmente vai haver o sensor de sala para detectar tudo que tem na sala,
        //e um detector de cena para objetivos maiores e locais disponíveis.
    
        //Esta é uma parte para definir locais específicos, provavelmente será necessário um factory ou algo para detectar 
        //flags de locais de interesse.
        [Header("Known Locations")] 
        [SerializeField]
        private Transform restingPosition;
        [SerializeField] private Transform foodShack;
        [SerializeField] private Transform doorOnePosition;
        [SerializeField] private Transform doorTwoPosition;

        private NavMeshAgent navMeshAgent;
        //AnimationController animations;
        private Rigidbody rb;
    
        //Aqui é apenas um rascunho de um persnagem apra fazer testes provavelmente vamos ter um sistema para isso
        [Header("Stats")] 
        public float health = 100;
        public float stamina = 100;

        private CountdownTimer statsTimer;

        private GameObject target;
        private Vector3 destination;

        private AgentGoal lastGoal;
        public AgentGoal CurrentGoal;
        public ActionPlan ActionPlan;
        public AgentAction CurrentAction;
    
        public Dictionary<string, AgentBelief> Beliefs;
        public HashSet<AgentAction> Actions;
        private HashSet<AgentGoal> _goals;
    
        [Inject] private GoapFactory gFactory;
        private IGoapPlanner gPlanner;

        private void Awake() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            //animations = GetComponent<AnimationController>();
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        
            gPlanner = gFactory.CreatePlanner();
        }

        private void Start() {
            SetupTimers();
            SetupBeliefs();
            SetupActions();
            SetupGoals();
        }

        private void SetupBeliefs() {
            Beliefs = new Dictionary<string, AgentBelief>();
            var factory = new BeliefFactory(this, Beliefs);
        
            factory.AddBelief("Nothing", () => false);
        
            factory.AddBelief("AgentIdle", () => !navMeshAgent.hasPath);
            factory.AddBelief("AgentMoving", () => navMeshAgent.hasPath);
            factory.AddBelief("AgentHealthLow", () => health < 30);
            factory.AddBelief("AgentIsHealthy", () => health >= 50);
            factory.AddBelief("AgentStaminaLow", () => stamina < 10);
            factory.AddBelief("AgentIsRested", () => stamina >= 50);
        
            factory.AddLocationBelief("AgentAtDoorOne", 3f, doorOnePosition);
            factory.AddLocationBelief("AgentAtDoorTwo", 3f, doorTwoPosition);
            factory.AddLocationBelief("AgentAtRestingPosition", 3f, restingPosition);
            factory.AddLocationBelief("AgentAtFoodShack", 3f, foodShack);
        
            factory.AddSensorBelief("PlayerInChaseRange", chaseSensor);
            factory.AddSensorBelief("PlayerInAttackRange", attackSensor);
            factory.AddBelief("AttackingPlayer", () => false); // Player can always be attacked, this will never become true
        }

        private void SetupActions() {
            Actions = new HashSet<AgentAction> {
                new AgentAction.Builder("Relax")
                    .WithStrategy(new IdleStrategy(5))
                    .AddEffect(Beliefs["Nothing"])
                    .Build(),
                new AgentAction.Builder("Wander Around")
                    .WithStrategy(new WanderStrategy(navMeshAgent, 10))
                    .AddEffect(Beliefs["AgentMoving"])
                    .Build(),
                new AgentAction.Builder("MoveToEatingPosition")
                    .WithStrategy(new MoveStrategy(navMeshAgent, () => foodShack.position))
                    .AddEffect(Beliefs["AgentAtFoodShack"])
                    .Build(),
                new AgentAction.Builder("Eat")
                    .WithStrategy(new IdleStrategy(5))  // Later replace with a Command
                    .AddPrecondition(Beliefs["AgentAtFoodShack"])
                    .AddEffect(Beliefs["AgentIsHealthy"])
                    .Build(),
                new AgentAction.Builder("MoveToDoorOne")
                    .WithStrategy(new MoveStrategy(navMeshAgent, () => doorOnePosition.position))
                    .AddEffect(Beliefs["AgentAtDoorOne"])
                    .Build(),
                new AgentAction.Builder("MoveToDoorTwo")
                    .WithStrategy(new MoveStrategy(navMeshAgent, () => doorTwoPosition.position))
                    .AddEffect(Beliefs["AgentAtDoorTwo"])
                    .Build(),
                new AgentAction.Builder("MoveFromDoorOneToRestArea")
                    .WithCost(2)
                    .WithStrategy(new MoveStrategy(navMeshAgent, () => restingPosition.position))
                    .AddPrecondition(Beliefs["AgentAtDoorOne"])
                    .AddEffect(Beliefs["AgentAtRestingPosition"])
                    .Build(),
                new AgentAction.Builder("MoveFromDoorTwoRestArea")
                    .WithStrategy(new MoveStrategy(navMeshAgent, () => restingPosition.position))
                    .AddPrecondition(Beliefs["AgentAtDoorTwo"])
                    .AddEffect(Beliefs["AgentAtRestingPosition"])
                    .Build(),
                new AgentAction.Builder("Rest")
                    .WithStrategy(new IdleStrategy(5))
                    .AddPrecondition(Beliefs["AgentAtRestingPosition"])
                    .AddEffect(Beliefs["AgentIsRested"])
                    .Build(),
                new AgentAction.Builder("ChasePlayer")
                    .WithStrategy(new MoveStrategy(navMeshAgent, () => Beliefs["PlayerInChaseRange"].Location))
                    .AddPrecondition(Beliefs["PlayerInChaseRange"])
                    .AddEffect(Beliefs["PlayerInAttackRange"])
                    .Build(),
                new AgentAction.Builder("AttackPlayer")
                    //.WithStrategy(new AttackStrategy(animations))
                    .WithStrategy(new AttackStrategy())
                    .AddPrecondition(Beliefs["PlayerInAttackRange"])
                    .AddEffect(Beliefs["AttackingPlayer"])
                    .Build()
            };
        }

        private void SetupGoals() {
            _goals = new HashSet<AgentGoal> {
                new AgentGoal.Builder("Chill Out")
                    .WithPriority(1)
                    .WithDesiredEffect(Beliefs["Nothing"])
                    .Build(),
                new AgentGoal.Builder("Wander")
                    .WithPriority(1)
                    .WithDesiredEffect(Beliefs["AgentMoving"])
                    .Build(),
                new AgentGoal.Builder("KeepHealthUp")
                    .WithPriority(2)
                    .WithDesiredEffect(Beliefs["AgentIsHealthy"])
                    .Build(),
                new AgentGoal.Builder("KeepStaminaUp")
                    .WithPriority(2)
                    .WithDesiredEffect(Beliefs["AgentIsRested"])
                    .Build(),
                new AgentGoal.Builder("SeekAndDestroy")
                    .WithPriority(3)
                    .WithDesiredEffect(Beliefs["AttackingPlayer"])
                    .Build()
            };
        }

        private void SetupTimers() {
            statsTimer = new CountdownTimer(2f);// arrumar um bom lugar apra setar a mudança de timers
            statsTimer.OnTimerStop += () => {
                UpdateStats();
                statsTimer.Start();
            };
            statsTimer.Start();
        }

        // TODO move to stats system
        private void UpdateStats() {
            stamina += InRangeOf(restingPosition.position, 3f) ? 20 : -10;
            health += InRangeOf(foodShack.position, 3f) ? 20 : -5;
            stamina = Mathf.Clamp(stamina, 0, 100);
            health = Mathf.Clamp(health, 0, 100);
        }

        //TODO usar a extesão
        private bool InRangeOf(Vector3 pos, float range) => Vector3.Distance(transform.position, pos) < range;

        private void OnEnable() => chaseSensor.OnTargetChanged += HandleTargetChanged;
        private void OnDisable() => chaseSensor.OnTargetChanged -= HandleTargetChanged;

        private void HandleTargetChanged() {
            DebugManager.Log<GoapAgent>("Target changed, clearing current action and goal");
            // Force the planner to re-evaluate the plan
            CurrentAction = null;
            CurrentGoal = null;
        }

        private void Update() {
            //animations.SetSpeed(navMeshAgent.velocity.magnitude);
        
            // Update the plan and current action if there is one
            if (CurrentAction == null) {
                DebugManager.Log<GoapAgent>("Calculating any potential new plan");
                CalculatePlan();

                if (ActionPlan != null && ActionPlan.Actions.Count > 0) {
                    navMeshAgent.ResetPath();

                    CurrentGoal = ActionPlan.AgentGoal;
                    DebugManager.Log<GoapAgent>($"Goal: {CurrentGoal.Name} with {ActionPlan.Actions.Count} actions in plan");
                    CurrentAction = ActionPlan.Actions.Pop();
                    DebugManager.Log<GoapAgent>($"Popped action: {CurrentAction.Name}");
                    // Verify all precondition effects are true
                    if (CurrentAction.Preconditions.All(b => b.Evaluate())) {
                        CurrentAction.Start();
                    } else {
                        DebugManager.Log<GoapAgent>("Preconditions not met, clearing current action and goal");
                        CurrentAction = null;
                        CurrentGoal = null;
                    }
                }
            }

            // If we have a current action, execute it
            if (ActionPlan != null && CurrentAction != null) {
                CurrentAction.Update(Time.deltaTime);

                if (CurrentAction.Complete) {
                    DebugManager.Log<GoapAgent>($"{CurrentAction.Name} complete");
                    CurrentAction.Stop();
                    CurrentAction = null;

                    if (ActionPlan.Actions.Count == 0) {
                        DebugManager.Log<GoapAgent>("Plan complete");
                        lastGoal = CurrentGoal;
                        CurrentGoal = null;
                    }
                }
            }
        }

        private void CalculatePlan() {
            var priorityLevel = CurrentGoal?.Priority ?? 0;
        
            var goalsToCheck = _goals;
        
            // If we have a current goal, we only want to check goals with higher priority
            if (CurrentGoal != null) {
                Debug.Log("Current goal exists, checking goals with higher priority");
                goalsToCheck = new HashSet<AgentGoal>(_goals.Where(g => g.Priority > priorityLevel));
            }
        
            var potentialPlan = gPlanner.Plan(this, goalsToCheck, lastGoal);
            if (potentialPlan != null) {
                ActionPlan = potentialPlan;
            }
        }
    }
}