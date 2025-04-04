# Tugas Besar 1 IF2211 Strategi Algoritma
Semester II tahun 2024/2025

## Pemanfaatan Algoritma Greedy dalam Pembuatan Bot Robocode TankRoyale

### Ringkasan
Repositori ini merupakan bagian dari Tugas Besar 1 IF2211 Strategi Algoritma, yaitu pemanfaatan algoritma greedy dalam pembuatan bot untuk permainan Robocode TankRoyale. Empat bot (satu bot utama dan tiga bot alternatif) telaH dibuat dengan pendekatan heuristik yang berbeda.

## Bot Utama
### **1. KepelesetPolisiTidur (Greedy berdasarkan Jarak Tembak Minimum)**
- Memilih musuh terdekat sebagai target.
- Bergerak mendekati target dengan kecepatan berdasarkan jarak.
- Terus memutar senjata 360° untuk mencari musuh.
- Menembak saat bot yang terdeteksi adalah musuh terdekat.
- Menangani tabrakan dengan bergerak mundur dan berputar.

## Bot Alternatif
### **2. KepelesetPolisiTidurAlt1 (Greedy berdasarkan HP+Jarak Minimum)**
- Memilih target berdasarkan gabungan HP (energy) dan jarak, dengan menghitung skor = enemyHP + distance; target dengan skor terendah diprioritaskan.
- Bergerak mendekati target dengan kecepatan berdasarkan jarak.
- Terus memutar senjata 360° untuk mencari musuh.
- Menembak saat bot yang terdeteksi adalah target dengan skor terendah.
- Menangani tabrakan dengan bergerak mundur dan berputar.

### **3. KepelesetPolisiTidurAlt2 (Greedy berdasarkan pendekatan first-scanned target)**
- Memilih dan mengunci target yang pertama dideteksi, lalu mempertahankan target tersebut sampai mati.
- Bergerak mendekati target dengan kecepatan berdasarkan jarak.
- Terus memutar senjata 360° untuk mencari musuh.
- Apabila target sudah mati, bot akan melakukan reset dan pemindaian ulang.
- Menangani tabrakan dengan bergerak mundur dan berputar.

### **4. KepelesetPolisiTidurAlt3 (Greedy berdasarkan pendekatan Energy Optimization Firing dan Random Move)**
- Memilih target yang pertama kali dideteksi.
- Bergerak random agar tidak mudah ditembak oleh bot lain.
- Terus melakukan pemindaian 360°
- Apabila terdeteksi bot, kekuatan tembakan akan disesuaikan berdasarkan energi yang dimiliki bot dan jarak dengan bot target.
- Menangani tabrakan dan terkena tembak dengan mundur dan berputar

## Persyaratan
- [.NET 6.0+](https://dotnet.microsoft.com/download)
- [Custom Engine Robocode TankRoyale](https://github.com/Ariel-HS/tubes1-if2211-starter-pack/releases/tag/v1.0) dari starter pack tubes telah terinstal.

## Instalasi & Kompilasi
1. Clone repositori ini:
   ```sh
   git clone https://github.com/wiwekaputera/Tubes1_KepelesetPolisiTidur.git
   ```
2. Buka *custom engine* `.jar` yang telah diunduh.
3. Pilih menu **Config > Bot Root Directories**, lalu pilih parent folder yang berisi folder bot.
4. Pilih menu **Battle > Start Battle**, lalu boot bot untuk otomatis mem-build bot.

## Author
- **Naufarrel Zhafif Abhista** - 13523149
- **Hasri Fayadh Muqaffa** - 13523156
- **I Made Wiweka Putera** - 13523160
