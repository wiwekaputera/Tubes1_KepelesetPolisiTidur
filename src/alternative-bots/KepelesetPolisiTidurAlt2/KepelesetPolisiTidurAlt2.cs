using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

/*
 * KepelesetPolisiTidurAlt2
 * 
 * Strategy:
 * - First-Scanned Lock: target bot pertama yang discan, kunci sampai dia mati
 * - Gun muter 360 untuk scanning + fire kalau musuh yang discan = target
 */

public class KepelesetPolisiTidurAlt2 : Bot
{
    private int? targetId = null;
    private double targetX = 0;
    private double targetY = 0;

    private bool rotateGunRight = true;

    static void Main() => new KepelesetPolisiTidurAlt2().Start();
    public KepelesetPolisiTidurAlt2() : base(BotInfo.FromFile("KepelesetPolisiTidurAlt2.json")) { }

    public override void Run()
    {
        BodyColor   = Color.DarkRed;
        TurretColor = Color.Red;
        RadarColor  = Color.OrangeRed;
        GunColor    = Color.Firebrick;
        TracksColor = Color.DimGray;
        BulletColor = Color.Yellow;
        ScanColor   = Color.Orange;

        while (IsRunning)
        {
            if (targetId != null)
            {
                double turnAngle = NormalizeRelativeAngle(BearingTo(targetX, targetY));
                TurnRight(turnAngle);

                double dist = DistanceTo(targetX, targetY);
                if (dist > 150)
                    Forward(100);
                else if (dist < 50)
                    Back(100);
                else
                    Forward(50);
            }

            if (rotateGunRight)
                TurnGunRight(360);
            else
                TurnGunLeft(360);

            rotateGunRight = !rotateGunRight;
        }
    }

    public override void OnScannedBot(ScannedBotEvent evt)
    {
        // Ambil target pertama yang discan
        if (targetId == null)
        {
            targetId = evt.ScannedBotId;
            targetX = evt.X;
            targetY = evt.Y;
        }

        // Kalau yang discan adalah target kita, update posisi & tembak
        if (evt.ScannedBotId == targetId)
        {
            targetX = evt.X;
            targetY = evt.Y;
            Fire(3);
        }
    }

    public override void OnBotDeath(BotDeathEvent evt)
    {
        if (evt.VictimId == targetId)
        {
            targetId = null;
        }
    }

    public override void OnHitWall(HitWallEvent evt)
    {
        Back(50);
        TurnRight(90);
    }

    public override void OnHitBot(HitBotEvent evt)
    {
        Back(100);
        TurnRight(45);
    }
}
