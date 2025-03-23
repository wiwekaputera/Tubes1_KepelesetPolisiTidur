using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

/* Bntr deskripsinya nyusul */

public class KepelesetPolisiTidurAlt3 : Bot {
    private int bestTargetID = -1;
    private double bestTargetAngle = double.MaxValue; // Ini untuk sudut mininum ke best target
    private double targetX = 0;
    private double targetY = 0;

    static void Main() {
        new KepelesetPolisiTidurAlt3().Start();
    }

    public KepelesetPolisiTidurAlt3() : base(BotInfo.FromFile("KepelesetPolisiTidurAlt3.json")) { }

    public override void Run()
    {
        // Setting warna bot
        BodyColor   = Color.FromArgb(0xFF, 0x10, 0x10, 0x10);
        TurretColor = Color.FromArgb(0xFF, 0x30, 0x30, 0x30);
        RadarColor  = Color.FromArgb(0xFF, 0x50, 0x50, 0x50);
        GunColor    = Color.FromArgb(0xFF, 0x40, 0x40, 0x40);
        TracksColor = Color.FromArgb(0xFF, 0x20, 0x20, 0x20);
        BulletColor = Color.FromArgb(0xFF, 0xFF, 0xA0, 0x00);
        ScanColor   = Color.FromArgb(0xFF, 0xFF, 0xA0, 0x00);

        while (IsRunning) {
            // Selalu scan area sekitar
            TurnRadarRight(360);

            if (bestTargetID != -1) {
                TurnGunRight(NormalizeRelativeAngle(BearingTo(targetX, targetY) - GunDirection));

                // Arahin bot ke target
                double turnAngle = NormalizeRelativeAngle(BearingTo(targetX, targetY));
                TurnRight(turnAngle);

                double distance = DistanceTo(targetX, targetY);
                if (distance > 150) {
                    Forward(100);
                } else if (distance < 50) {
                    Back(100);
                } else {
                    Forward(50);
                }
            }
        }
    }

    public override void OnScannedBot(ScannedBotEvent evt) {
        double angleToTarget = Math.Abs(NormalizeRelativeAngle(BearingTo(evt.X, evt.Y) - GunDirection));

        // Nyari target dengan minimum sudut
        if (angleToTarget < bestTargetAngle) {
            bestTargetAngle = angleToTarget;
            bestTargetID = evt.ScannedBotId;
            targetX = evt.X;
            targetY = evt.Y;
        }

        // FIRE jika merupakan target terbaik
        if (evt.ScannedBotId == bestTargetID) {
            Fire(1);
        }
    }

    // Jika tertabrak tembok, mundur dan putar ke kanan 90 derajat
    public override void OnHitWall(HitWallEvent evt)
    {
        Back(50);
        TurnRight(90);
    }

    // Jika tertabrak, mundur dan putar ke kanan 45 derajat
    public override void OnHitBot(HitBotEvent evt) {
        Back(100);
        TurnRight(45);
    }

    // Reset jika target mati
    public override void OnBotDeath(BotDeathEvent evt)
    {
        if (evt.VictimId == bestTargetID)
        {
            bestTargetAngle = double.MaxValue;
            bestTargetID = -1;
        }
    }
}

// using System;
// using System.Drawing;
// using Robocode.TankRoyale.BotApi;
// using Robocode.TankRoyale.BotApi.Events;

// public class KepelesetPolisiTidurAlt3 : Bot
// {
//     private int bestTargetID = -1;
//     private double bestTargetAngle = double.MaxValue;

//     static void Main(string[] args)
//     {
//         new KepelesetPolisiTidurAlt3().Start();
//     }

//     KepelesetPolisiTidurAlt3() : base(BotInfo.FromFile("KepelesetPolisiTidurAlt3.json")) { }

//     public override void Run()
//     {
//         BodyColor = Color.FromArgb(0xFF, 0x8C, 0x00);
//         TurretColor = Color.FromArgb(0xFF, 0xA5, 0x00);
//         RadarColor = Color.FromArgb(0xFF, 0xD7, 0x00);
//         BulletColor = Color.FromArgb(0xFF, 0x45, 0x00);
//         ScanColor = Color.FromArgb(0xFF, 0xFF, 0x00);
//         TracksColor = Color.FromArgb(0x99, 0x33, 0x00);
//         GunColor = Color.FromArgb(0xCC, 0x55, 0x00);
        
//         while (IsRunning)
//         {
//             Forward(80);
//             TurnGunRight(360);
//             Back(80);
//             TurnGunRight(360);
//         }
//     }

//     public override void OnScannedBot(ScannedBotEvent evt)
//     {
//         double distance = DistanceTo(evt.X, evt.Y);
//         double targetSpeed = evt.Speed;
        
//         double firePower;
//         if (targetSpeed == 0 || distance < 200)
//             firePower = 3;
//         else if (distance < 500)
//             firePower = 2;
//         else
//             firePower = 1;

//         Fire(firePower);
//     }

//     public override void OnHitByBullet(HitByBulletEvent evt)
//     {
//         var bearing = CalcBearing(evt.Bullet.Direction);
//         TurnLeft(90 - bearing);
//         Forward(50);
//     }

//     public override void OnHitWall(HitWallEvent evt)
//     {
//         Back(50);
//         TurnRight(90);
//     }

//     public override void OnHitBot(HitBotEvent evt)
//     {
//         Back(100);
//         TurnRight(45);
//     }

//     public override void OnBotDeath(BotDeathEvent evt)
//     {
//         if (evt.VictimId == bestTargetID)
//         {
//             bestTargetAngle = double.MaxValue;
//             bestTargetID = -1;
//         }
//     }
// }