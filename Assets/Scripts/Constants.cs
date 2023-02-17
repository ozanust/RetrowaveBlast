using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour {

    public static Color UNDER_20_COLOR = new Color(1, 1, 1);
    public static Color BETWEEN_20_60_COLOR = new Color(10, 10, 10);
    public static Color BETWEEN_60_100_COLOR = new Color(20, 20, 20);
    public static Color BETWEEN_100_160_COLOR = new Color(1, 1, 1);
    public static Color BETWEEN_160_220_COLOR = new Color(1, 1, 1);
    public static Color BETWEEN_220_300_COLOR = new Color(1, 1, 1);
    public static Color BETWEEN_300_400_COLOR = new Color(1, 1, 1);
    public static Color BETWEEN_400_500_COLOR = new Color(1, 1, 1);

    public static float MIN_BOUNCE_HEIGHT = 0f;
    public static float MAX_BOUNCE_HEIGHT = 6.5f;

    public static float SMALL_BLOCK_SIZE_MULTIPLIER = 4f;
    public static float MEDIUM_BLOCK_SIZE_MULTIPLIER = 8f;
    public static float BIG_BLOCK_SIZE_MULTIPLIER = 12f;
    public static float HUGE_BLOCK_SIZE_MULTIPLIER = 16f;

    public static int MAX_BLOCK_COUNT_IN_GAMEPLAY_AREA = 3;
    public static int MAX_BULLET_COUNT_IN_GAMEPLAY_AREA = 100;

    public static float LEFT_BARRIER_POSITION = -Camera.main.orthographicSize * (float)Screen.width / (float)Screen.height;
    public static float RIGHT_BARRIER_POSITION = Camera.main.orthographicSize * (float)Screen.width / (float)Screen.height;
}
