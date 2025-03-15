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
    // Flag untuk menentukan arah rotasi senjata
    private bool rotateGunRight = true;

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
                Forward(50);
                
                // Alternating gun rotation biar scanning lebih merata
                if (rotateGunRight) TurnGunRight(360);
                else TurnGunLeft(360);
                rotateGunRight = !rotateGunRight;

                Back(50);

                if (rotateGunRight) TurnGunRight(360);
                else TurnGunLeft(360);
                rotateGunRight = !rotateGunRight;
            }
            else
            {
                // Jika tidak ada target, scan 360
                if (rotateGunRight)
                {
                    TurnGunRight(360);
                }
                else
                {
                    TurnGunLeft(360);
                }
                rotateGunRight = !rotateGunRight;
            }
        }
    }

    // Constantly update bot musuh terdekat
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

    public override void OnHitWall(HitWallEvent evt)
    {
        // Cabut dari tembok
        Back(50);
        TurnRight(90);
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