namespace SunamoCmdArgs.Cmd;

public class ParsedArgsStatic
{
    static ParsedArgs pa = new ParsedArgs();

    static string Arg1
    {
        get => pa.Arg1;
        set => pa.Arg1 = value;
    }
}
