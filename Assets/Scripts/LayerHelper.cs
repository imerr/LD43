using UnityEngine;

public class LayerHelper {
    static LayerHelper() {
        LayerObstacle = LayerMask.NameToLayer("Obstacle");
        LayerExit = LayerMask.NameToLayer("Exit");
        LayerVictim = LayerMask.NameToLayer("Victim");
        LayerPlayer = LayerMask.NameToLayer("Player");
        MoveMask = 1 << LayerObstacle;
    }

    public static int LayerObstacle { get; private set; }
    public static int MoveMask { get; private set; }
    public static int LayerExit { get; private set; }
    public static int LayerVictim { get; private set; }
    public static int LayerPlayer { get; private set; }
}