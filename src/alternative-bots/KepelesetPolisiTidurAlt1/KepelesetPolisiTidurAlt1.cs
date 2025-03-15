using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class KepelesetPolisiTidurAlt1 : Bot
{
    // Variabel untuk melacak target terdekat
    private double closestDistance = double.MaxValue;
    private int closestBotId = -1;
    // Variabel untuk menyimpan koordinat target terakhir yang terdeteksi
    private double targetX = 0;
    private double targetY = 0;

    static void Main()
    {
        new KepelesetPolisiTidurAlt1().Start();
    }

    public KepelesetPolisiTidurAlt1() : base(BotInfo.FromFile("KepelesetPolisiTidurAlt1.json")) { }

    public override void Run()
    {
        BodyColor   = Color.FromArgb(0xFF, 0x20, 0x20, 0x20);
        TurretColor = Color.FromArgb(0xFF, 0x40, 0x40, 0x40);
        RadarColor  = Color.FromArgb(0xFF, 0x60, 0x60, 0x60);
        GunColor    = Color.FromArgb(0xFF, 0x55, 0x55, 0x55);
        TracksColor = Color.FromArgb(0xFF, 0x30, 0x30, 0x30);
        BulletColor = Color.FromArgb(0xFF, 0xFF, 0x90, 0x00);
        ScanColor   = Color.FromArgb(0xFF, 0xFF, 0x90, 0x00);

        while (IsRunning)
        {
            if (closestBotId != -1)
            {
                double angleToTarget = BearingTo(targetX, targetY);
                double turnAngle = NormalizeRelativeAngle(angleToTarget - Direction);
                TurnRight(turnAngle);

                // TODO: Greedy movement
                // TODO: TurnGun sebanyak selisih ke closestBotId
                Forward(100);
                TurnGunRight(360);
                Back(100);
                TurnGunRight(360);
            }
            else
            {
                TurnGunRight(360);
            }
        }
    }

    public override void OnScannedBot(ScannedBotEvent evt)
    {
        double dist = DistanceTo(evt.X, evt.Y);
        if (dist < closestDistance)
        {
            closestDistance = dist;
            closestBotId = evt.ScannedBotId;
            targetX = evt.X;
            targetY = evt.Y;
        }
        
        if (evt.ScannedBotId == closestBotId)
        {
            Fire(1);
        }
    }

    public override void OnBotDeath(BotDeathEvent evt)
    {
        if (evt.VictimId == closestBotId)
        {
            closestDistance = double.MaxValue;
            closestBotId = -1;
        }
    }
}
