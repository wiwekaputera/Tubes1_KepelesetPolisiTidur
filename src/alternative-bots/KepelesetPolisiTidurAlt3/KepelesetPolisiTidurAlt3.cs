using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

/* */

public class KepelesetPolisiTidurAlt3 : Bot
{
    private double lastEnemyAngle; // Simpan sudut terakhir ke musuh yg ke-detect
    private double lastEnemyDistance; // Simpan jarak terakhir ke musuh yg ke-detect
    
    static void Main()
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
            MoveBot(); // Menggerakan Bot
            TurnGunRight(360); // Menggerakan Gun
        }
    }

    // Gerakan dibuat random agar tidak mudah terkena tembakan musuh
    private void MoveBot()
    {
        Forward(Random.Shared.Next(50, 151));

        if (Random.Shared.Next(0, 2) == 0)
            TurnRight(Random.Shared.Next(15, 91));
        else
            TurnLeft(Random.Shared.Next(15, 91));

        Back(Random.Shared.Next(50, 151));

        if (Random.Shared.Next(0, 2) == 0)
            TurnRight(Random.Shared.Next(15, 91));
        else
            TurnLeft(Random.Shared.Next(15, 91));
    }

    public override void OnScannedBot(ScannedBotEvent evt)
    {
        lastEnemyAngle = NormalizeRelativeAngle(evt.Direction - GunDirection);
        lastEnemyDistance = DistanceTo(evt.X, evt.Y);

        // Tembakan disesuaikan dengan energi yang dimiliki bot dan jarak dengan musuh
        double firePower = 0;
        if (Energy < 20) firePower = 1;
        else if (evt.Speed == 0 || DistanceTo(evt.X, evt.Y) < 200) firePower = 3;
        else if (DistanceTo(evt.X, evt.Y) < 500) firePower = 2;
        else firePower = 1;

        Fire(firePower);
        
        // Ngarahin gun ke musuh
        TurnGunRight(NormalizeRelativeAngle(evt.Direction - GunDirection));
    }

    public override void OnHitByBullet(HitByBulletEvent evt)
    {
        // Jika terkena tembakan, maka bot akan mengubah arah gerak
        Back(150);
        if (Random.Shared.Next(0, 2) == 0)
            TurnRight(45);
        else
            TurnLeft(45);
    }

    // Jika terkena tembok, bot mengubah arah geraknya
    public override void OnHitWall(HitWallEvent evt)
    {
        Back(150);
        TurnRight(90);
    }

    // Jika terkena bot lain, bot mengubah arah geraknya
    public override void OnHitBot(HitBotEvent evt)
    {
        Back(150);
        TurnRight(45);
    }
}