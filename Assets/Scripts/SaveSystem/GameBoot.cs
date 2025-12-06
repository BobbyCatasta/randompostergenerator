/// <summary>
/// Static class containing game startup configuration data.
/// </summary>
public static class GameBoot 
{
    /// <summary>
    /// Indicates whether to start a new game or load a saved one.
    /// </summary>
    public static bool IsNewGame;

    /// <summary>
    /// Number of rows for the card grid.
    /// </summary>
    public static int Rows = 4;
    
    /// <summary>
    /// Number of columns for the card grid.
    /// </summary>
    public static int Columns = 4;

    /// <summary>
    /// Activates the Blind Game Mode
    /// </summary>
    public static bool IsBlindMode = false;
}