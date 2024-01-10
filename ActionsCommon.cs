using SunamoDelegates;
using SunamoValues;

namespace SunamoCmdArgs.Cmd;

public partial class ProgramCommon
{
    public void AddToAllActions(string v, Dictionary<string, VoidVoid> actions, Dictionary<string, TaskVoid> actionsAsync)
    {
        if (actions != null)
        {
            foreach (var item in actions)
            {
                if (item.Key != "None")
                {
                    allActions.Add(v + AllStrings.swd + item.Key, item.Value);
                }
            }
        }

        if (actionsAsync != null)
        {
            foreach (var item in actionsAsync)
            {
                if (item.Key != "None")
                {
                    allActionsAsync.Add(v + AllStrings.swd + item.Key, item.Value);
                }
            }
        }
    }
}
