using UnityEngine;

public class Global : MonoBehaviour
{
    public static int mineChance = 10;
    public static int negativeMineChance = 15;
    public static int y = 10;
    public static int x = 10;
    public static int questionChance = 0;
    public static int exclamationChance = 0;
    public static int morelessChance = 0;
    public static bool negativeInclude = false;
    public static int colors = 1;

    public static int minesCount;
    public static int revealedCount;
    public static int currentMinesCount;

    public NeighborType type;
    public bool sameGridType;
}