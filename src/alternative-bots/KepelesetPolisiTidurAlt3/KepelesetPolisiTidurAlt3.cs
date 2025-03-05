using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class KepelesetPolisiTidurAlt3 : Bot
{
    // The main method starts our bot
    static void Main()
    {
        new KepelesetPolisiTidurAlt3().Start();
    }

    // Constructor, which loads the bot config file
    KepelesetPolisiTidurAlt3() : base(BotInfo.FromFile("KepelesetPolisiTidurAlt3.json")) { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        BodyColor = Color.FromArgb(0xFF, 0x00, 0x00);   // Red
        TurretColor = Color.FromArgb(0xFF, 0xFF, 0xFF); // White
        RadarColor = Color.FromArgb(0xFF, 0x00, 0x00);  // Red
        BulletColor = Color.FromArgb(0xFF, 0xFF, 0xFF); // White
        ScanColor = Color.FromArgb(0xFF, 0x00, 0x00);   // Red
        TracksColor = Color.FromArgb(0xFF, 0xFF, 0xFF); // White
        GunColor = Color.FromArgb(0xFF, 0x00, 0x00);    // Red

        // Repeat while the bot is running
        while (IsRunning)
        {
            Forward(100);
            TurnGunRight(360);
            Back(100);
            TurnGunRight(360);
        }
    }

    // We saw another bot -> fire!
    public override void OnScannedBot(ScannedBotEvent evt)
    {
        Fire(1);
    }

    // We were hit by a bullet -> turn perpendicular to the bullet
    public override void OnHitByBullet(HitByBulletEvent evt)
    {
        // Calculate the bearing to the direction of the bullet
        double bearing = CalcBearing(evt.Bullet.Direction);

        // Turn 90 degrees to the bullet direction based on the bearing
        TurnLeft(90 - bearing);
    }
}
