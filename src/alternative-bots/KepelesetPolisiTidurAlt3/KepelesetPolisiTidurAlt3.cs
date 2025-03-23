using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class KepelesetPolisiTidurAlt3 : Bot
{
    private double lastEnemyBearing;
    private double lastEnemyDistance;
    
    static void Main(string[] args)
    {
        new KepelesetPolisiTidurAlt3().Start();
    }

    KepelesetPolisiTidurAlt3() : base(BotInfo.FromFile("KepelesetPolisiTidurAlt3.json")) { }

    public override void Run()
    {
        // Warna Bot
        BodyColor = Color.FromArgb(0xFF, 0x00, 0x4B, 0x82);
        TurretColor = Color.FromArgb(0xFF, 0x00, 0x80, 0x00);
        RadarColor = Color.FromArgb(0xFF, 0xFF, 0xA5, 0x00);
        BulletColor = Color.FromArgb(0xFF, 0xDC, 0x14, 0x3C);
        ScanColor = Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00); 
        TracksColor = Color.FromArgb(0xFF, 0x80, 0x00, 0x80); 
        GunColor = Color.FromArgb(0xFF, 0x00, 0xBF, 0xFF);
        
        while (IsRunning)
        {
            MoveSmart();
            TurnGunRight(360);
        }
    }

    private void MoveSmart()
    {
        Forward(80);
        if (Random.Shared.Next(0, 2) == 0)
            TurnRight(30); // Mengubah sudut gerakan secara acak
        else
            TurnLeft(30);
        Back(80);
    }

    public override void OnScannedBot(ScannedBotEvent evt)
    {
        lastEnemyBearing = NormalizeRelativeAngle(evt.Direction - GunDirection);
        lastEnemyDistance = DistanceTo(evt.X, evt.Y);

        // Menyesuaikan tembakan agar efisien sesuai dengan sisa energy
        double fire;
        if (Energy < 20) fire = 1;
        else if (evt.Speed == 0 || DistanceTo(evt.X, evt.Y) < 200) fire = 3;
        else if (DistanceTo(evt.X, evt.Y) < 500) fire = 2;
        else fire = 1;

        Fire(fire);
        
        // Ngarahin gun ke musuh
        TurnGunRight(NormalizeRelativeAngle(evt.Direction - GunDirection));
    }

    public override void OnHitByBullet(HitByBulletEvent evt)
    {
        // Jika terkena tembakan, maka bot akan mengubah arah gerak
        Back(50);
        if (Random.Shared.Next(0, 2) == 0)
            TurnRight(45);
        else
            TurnLeft(45);
    }

    // Jika terkena tembok, bot mengubah arah geraknya
    public override void OnHitWall(HitWallEvent evt)
    {
        Back(50);
        TurnRight(90);
    }

    // Jika terkena bot lain, bot mengubah arah geraknya
    public override void OnHitBot(HitBotEvent evt)
    {
        Back(100);
        TurnRight(45);
    }
}