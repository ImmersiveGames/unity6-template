using System.Collections.Generic;

namespace _ImmersiveGames.Scripts.Utils.GoapSystem {
    public interface IGoapPlanner {
        ActionPlan Plan(GoapAgent agent, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal = null);
    }
}