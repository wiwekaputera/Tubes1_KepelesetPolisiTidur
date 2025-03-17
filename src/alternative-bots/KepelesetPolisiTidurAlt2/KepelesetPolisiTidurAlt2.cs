using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class KepelesetPolisiTidurAlt2 : Bot
{
    private int? targetId = null;
    private int lostTargetCounter = 0;

    static void Main()
    {
        new KepelesetPolisiTidurAlt2().Start();
    }

    KepelesetPolisiTidurAlt2() : base(BotInfo.FromFile("KepelesetPolisiTidurAlt2.json")) { }

    public override void Run()
    {
        // Set warna biar keren
        BodyColor = Color.Red;
        TurretColor = Color.White;
        RadarColor = Color.Red;
        BulletColor = Color.White;
        ScanColor = Color.Red;
        TracksColor = Color.White;
        GunColor = Color.Red;

        // Gerak awal biar ga diem aja
        Forward(100);
        TurnRight(90);

        // Loop utama
        while (IsRunning)
        {
            if (targetId == null)
            {
                TurnGunRight(10);  // Kalau belum ada target, scanning dulu
            }
            else
            {
                // Bergerak ke arah target sambil tembak
                Forward(50);
                Fire(3);

                // Kalau target ilang lebih dari 30 tick, reset
                lostTargetCounter++;
                if (lostTargetCounter > 30)
                {
                    targetId = null;
                    lostTargetCounter = 0;
                }
            }
        }
    }

   public override void OnScannedBot(ScannedBotEvent evt)
    {
        if (targetId == null)
        {
            targetId = evt.ScannedBotId;  // Kunci target pertama yang terlihat
        }
        else if (targetId == evt.ScannedBotId)
        {
            // Dapatkan sudut turret ke target
            double gunBearing = GunBearingTo(evt.X, evt.Y);

            // Putar turret ke arah target
            TurnGunLeft(gunBearing);

            // Tembak full power
            Fire(3);
            lostTargetCounter = 0;  // Reset lost counter kalau kita masih melihat target

            // Jika musuh jauh (> 100), mendekat
            double distance = DistanceTo(evt.X, evt.Y);
            if (distance > 100)
            {
                Forward(50);
            }
        }
    }
}