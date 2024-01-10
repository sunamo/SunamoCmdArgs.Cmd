using SunamoCl;
using SunamoCmd.Helpers;
using SunamoCmdArgs.Data;
using SunamoDelegates;
using SunamoEnumsHelper;
using SunamoString;
using SunamoThisApp;
using SunamoValues;

namespace SunamoCmdArgs.Cmd;

public partial class ProgramCommon
{
    public Dictionary<string, VoidVoid> allActions = new Dictionary<string, VoidVoid>();
    public Dictionary<string, TaskVoid> allActionsAsync = new Dictionary<string, TaskVoid>();
    // Způsobuje mi to problémy tím že se pokouší vložit klíč který již existuje (např. Dating)
    // Zdá se že k ničemu to nepotřebuji, proto veškerou práci s tím všude zakomentuji
    // tak ne, potřebuji ho i nadále abych si do něj uložil názvy všech akcí
    // a přes fulltext vyberu hledané
    public Dictionary<string, object> groupsOfActions = new Dictionary<string, object>();

    /// <summary>
    /// vypadá to že:
    /// 
    /// true -  
    /// </summary>
    static bool perform
    {
        get => CLCmd.perform;
        set => CLCmd.perform = value;
    }

    /// <summary>
    /// must be IEnumerable
    /// </summary>
    /// <param name="e"></param>
    void ProcessArgsErrors(IEnumerable<Error> e)
    {
    }

    public Tuple<T, Mode> ProcessArgs<T, Mode>(String[] args, Mode ifParseFail, bool writeError = true) where T : CommonArgs
        where Mode : struct
    {
        T a = default(T);
        string arg = null;

        #region Parse and executing node if was set

        CLCmd.WriteLine("args.Length: " + args.Length);



        if (args.Length != 0)
        {
            // 2) parsování atributů
            CmdArgs.ProcessArgsErrors = ProcessArgsErrors;
            a = CmdArgs.SaveArgsWorker<T>(args);

            //Arg1 etc. is in if below
            //CL.WriteLine(a.Path);
            //CL.WriteLine(a.Mode);
            arg = a.Mode;
        }
        Mode mode = EnumHelper.Parse<Mode>(arg, ifParseFail);
#if !DEBUG
        CL.WriteLine("Arg: " + SH.NullToStringOrDefault(arg));
        if (arg != null)
        {
            CL.WriteLine("arg is NOT null");
            if (EqualityComparer<Mode>.Default.Equals(mode, ifParseFail))
            {
                if (writeError)
                {
                    ThisApp.Error("Parse mode failed, probably " + arg + " is not in Mode defined");
                }
                return new Tuple<T, Mode>(a, ifParseFail);
            }
        }
        else
        {
            CL.WriteLine("arg is null");
        }
#endif
        #endregion

        return new Tuple<T, Mode>(a, mode);
    }

    public Func<Dictionary<string, VoidVoid>> AddGroupOfActions;

    public
#if ASYNC
    async Task
#else
    void
#endif
 PerformAction(object mode)
    {
        perform = false;
        //Dictionary<string, VoidVoid> groupsOfActions = AddGroupOfActions();

        //foreach (var item in  groupsOfActions)
        //{
        //    // perform was setted to false, therefore this will do nothing
        //    item.Value();
        //}

        CLCmd.WriteLine("allActions.Count: " + allActions.Count);

        foreach (var item in allActions)
        {
            if (item.Key.Contains(AllStrings.swd + mode.ToString()))
            {
                item.Value();
                return;
            }
        }

        foreach (var item in allActionsAsync)
        {
            if (item.Key.Contains(AllStrings.swd + mode.ToString()))
            {
#if ASYNC
                await
#endif
                            item.Value();
                return;
            }
        }

        ThisApp.Error("No method to call was founded");

        perform = true;
    }
}
