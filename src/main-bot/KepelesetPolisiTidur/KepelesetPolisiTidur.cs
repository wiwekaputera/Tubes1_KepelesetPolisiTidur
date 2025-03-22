using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

/*
 * KepelesetPolisiTidur Bot (Greedy: HP + Jarak)
 * 
 * Overview:
 * - Bot bakal cari target berdasarkan gabungan HP (energy) dan jarak.
 * - Hitung skor target = enemyHP + distance. target dengan skor terendah diprioritaskan.
 * - Arahin body ke target tersebut. Kalo jarak > 150, maju penuh; kalo < 50, mundur; kalo di antaranya, maju dengan kecepatan sedang.
 * - Gun terus berotasi 360 buat scanning, dan FIRE kalo target yang dipilih terdeteksi.
 * - Kalo tabrakan dengan dinding atau bot lain, bot akan mundur dan berputar untuk menghindar.
 */

public class KepelesetPolisiTidur : Bot
{
    // Variabel buat tracking target berdasarkan skor (HP + jarak)
    private double bestScore = double.MaxValue;
    private int bestTargetId = -1;
    // Variabel buat simpen koordinat target terakhir yang dipindai
    private double targetX = 0;
    private double targetY = 0;
    // Flag buat tentuin arah rotasi gun (buat scanning)
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
            if (bestTargetId != -1)
            {
                // Kalo ada target (berdasarkan skor HP + jarak):
                // Hitung perbedaan sudut antara arah bot dan target, arahin bot ke target tsb
                double turnAngle = NormalizeRelativeAngle(BearingTo(targetX, targetY));
                TurnRight(turnAngle);

                // Gerakan sesuai jarak target:
                double dist = DistanceTo(targetX, targetY);
                if (dist > 150)
                    Forward(100);
                else if (dist < 50)
                    Back(100);
                else
                    Forward(50);
                
                // Rotasi gun 360 dengan arah bergantian (biar scanning merata)
                if (rotateGunRight)
                    TurnGunRight(360);
                else
                    TurnGunLeft(360);
                rotateGunRight = !rotateGunRight;
            }
            else
            {
                // Kalo ga ada target, scan 360 pake gun
                if (rotateGunRight)
                    TurnGunRight(360);
                else
                    TurnGunLeft(360);
                rotateGunRight = !rotateGunRight;
            }
        }
    }

    // Update target dengan skor terbaik dari gabungan HP dan jarak
    public override void OnScannedBot(ScannedBotEvent evt)
    {
        double dist = DistanceTo(evt.X, evt.Y);
        double enemyHP = evt.Energy;
        double score = enemyHP + dist;
        
        if (score < bestScore)
        {
            bestScore = score;
            bestTargetId = evt.ScannedBotId;
            targetX = evt.X;
            targetY = evt.Y;
        }
        
        // Kalo bot yang ter-scan adalah target dengan skor terendah, FIREEE
        if (evt.ScannedBotId == bestTargetId)
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

    // Kalo tabrakan dengan bot lain, mundur dan turnRight 45
    public override void OnHitBot(HitBotEvent evt)
    {
        Back(100);
        TurnRight(45);
    }

    // Kalo current target mati, reset tracking target biar bisa cari target baru.
    public override void OnBotDeath(BotDeathEvent evt)
    {
        if (evt.VictimId == bestTargetId)
        {
            bestScore = double.MaxValue;
            bestTargetId = -1;
        }
    }
}
