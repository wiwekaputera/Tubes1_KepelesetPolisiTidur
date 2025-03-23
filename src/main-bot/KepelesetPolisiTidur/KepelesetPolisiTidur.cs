using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

/*
 * KepelesetPolisiTidur Bot
 * 
 * Overview:
 * - Bot bakal cari target terdekat, arahin body ke target tersebut, dan bergerak sesuai ketentuan di poin berikutnya.
 * - Jarak ke target > 150, bot maju (Forward); Jarak < 50, bot mundur (Back); Jarak di antaranya, maju dengan kecepatan sedang.
 * - Gun terus  berotasi 360 untuk scanning musuh dan fire apabila scanned bot = closest bot
 * - Kalo tabrakan dengan dinding atau bot lain, bot akan mundur dan berputar untuk menghindar.
 */

public class KepelesetPolisiTidur : Bot
{
    // Variabel untuk melacak target terdekat
    private double closestDistance = double.MaxValue;
    private int closestBotId = -1;
    // Variabel untuk menyimpan koordinat target terakhir yang terdeteksi
    private double targetX = 0;
    private double targetY = 0;
    // Bool untuk menentukan arah rotasi gun (untuk scanning)
    private bool rotateGunRight = true;

    static void Main()
    {
        new KepelesetPolisiTidur().Start();
    }

    public KepelesetPolisiTidur() : base(BotInfo.FromFile("KepelesetPolisiTidur.json")) { }

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
                // Kalo ada target:
                // Hitung perbedaan sudut antara arah bot saat ini dengan arah ke target, sehingga bot dapat menghadap langsung ke target
                double turnAngle = NormalizeRelativeAngle(BearingTo(targetX, targetY));
                TurnRight(turnAngle);

                // Gerakan berdasarkan jarak ke target:
                double dist = DistanceTo(targetX, targetY);
                if (dist > 150)
                    Forward(100);
                else if (dist < 50)
                    Back(100);
                else
                    Forward(50);
                
                // Rotasi gun 360 dengan arah terus bergantian (biar scanning lebih merata)
                if (rotateGunRight)
                    TurnGunRight(360);
                else
                    TurnGunLeft(360);
                rotateGunRight = !rotateGunRight;
            }
            else
            {
                // Kalo ga ada target, scan 360 dengan gun
                if (rotateGunRight)
                    TurnGunRight(360);
                else
                    TurnGunLeft(360);
                rotateGunRight = !rotateGunRight;
            }
        }
    }

    // Update target berdasarkan jarak terdekat
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
        
        // Jika bot yang ter-scan adalah target terdekat, FIRE
        if (evt.ScannedBotId == closestBotId)
        {
            Fire(3);
        }
    }

    // Kalo nabrak dinding, mundur dan turnRight 90
    public override void OnHitWall(HitWallEvent evt)
    {
        Back(50);
        TurnRight(90);
    }

    // Kalo tabrakan dengan bot lain, mundur dan turn right 45.
    public override void OnHitBot(HitBotEvent evt)
    {
        Back(100);
        TurnRight(45);
    }

    // Kalo current target mati, reset tracking target biar bot bisa cari target baru.
    public override void OnBotDeath(BotDeathEvent evt)
    {
        if (evt.VictimId == closestBotId)
        {
            closestDistance = double.MaxValue;
            closestBotId = -1;
        }
    }
}