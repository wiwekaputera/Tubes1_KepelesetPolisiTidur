using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class KepelesetPolisiTidurAlt4 : Bot
{
    private bool aggressiveMode = false;
    private int targetID = -1;
    
    static void Main(string[] args)
    {
        new KepelesetPolisiTidurAlt4().Start();
    }

    KepelesetPolisiTidurAlt4() : base(BotInfo.FromFile("KepelesetPolisiTidurAlt4.json")) { }

    public override void Run()
    {
        BodyColor = Color.FromArgb(0xFF, 0x8C, 0x00);
        TurretColor = Color.FromArgb(0xFF, 0xA5, 0x00);
        RadarColor = Color.FromArgb(0xFF, 0xD7, 0x00);
        BulletColor = Color.FromArgb(0xFF, 0x45, 0x00);
        ScanColor = Color.FromArgb(0xFF, 0xFF, 0x00);
        TracksColor = Color.FromArgb(0x99, 0x33, 0x00);
        GunColor = Color.FromArgb(0xCC, 0x55, 0x00);
        
        while (IsRunning)
        {
            if (!aggressiveMode)
            {
                Forward(80);
                TurnRight(45);
                Back(80);
                TurnLeft(45);
            }
            else
            {
                Forward(100);
                TurnGunRight(360);
            }
        }
    }

    public override void OnScannedBot(ScannedBotEvent evt)
    {
        if (targetID == -1 || evt.Energy < GetBotEnergy(targetID)) // Lock target dengan HP terendah
        {
            targetID = evt.ScannedBotId;
        }

        if (evt.ScannedBotId == targetID)
        {
            double distance = evt.Distance;
            double firePower = (evt.Energy < 20 || distance < 200) ? 3 : (distance < 500 ? 2 : 1);
            Fire(firePower);
            TurnGunTo(evt.X, evt.Y);
        }
    }

    private double GetBotEnergy(int botId)
    {
        var bot = GetBot(botId);
        return bot?.Energy ?? double.MaxValue;
    }

    public override void OnHitByBullet(HitByBulletEvent evt)
    {
        if (!aggressiveMode)
        {
            var bearing = CalcBearing(evt.Bullet.Direction);
            TurnLeft(90 - bearing);
            Forward(50);
        }
    }

    public override void OnBotDeath(BotDeathEvent evt)
    {
        if (evt.VictimId == targetID)
        {
            targetID = -1;
        }
    }
}