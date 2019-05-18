﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading;

namespace Kontrola_Lotów
{
    class Baza
    {
        string[] wierza;
        public Baza(Radar r)            // tworzenie mapy wymaga wczytania 2 plikow: wierza.txt, radar.txt
        {
            wierza = System.IO.File.ReadAllLines("wierza.txt");
            string[] ObiektNaRadarze = System.IO.File.ReadAllLines("radar.txt");  // zawartosc pliku radar w liniach
            int rows = int.Parse(ObiektNaRadarze[0]);
            for (int i = 1; i < ObiektNaRadarze.Length; i++)
            {
                r.s.Add(new Statek(ObiektNaRadarze[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)));
            }
        }
        public Baza() { }
        public void save()              // zapisuje stan radaru do pliku
        {
            // zapisywanie mapy
        }
        public void insert(int x, int y, string s)
        {
            Console.SetCursorPosition(x - 1, y - 1);
            Console.Write(s);
        }
        public string kierunek(int d)
        {
            switch(d)
            {
                case 0: return "[E] Wschod";
                case 1: return "[SE] Poludniowy Wschod";
                case 2: return "[S] Poludnie";
                case 3: return "[SW] Poludniowy Zachod";
                case 4: return "[W] Zachod";
                case 5: return "[NZ] Polnocny Zachod";
                case 6: return "[N] Polnocny";
                case 7: return "[NE] Polnocny Wschod";
            }
            return "Nieznany";
        }
        public void czyscKonsole()
        {
            for (int i = 29; i < 45; i++)
                insert(137, i, "                                                                       ");
            for (int i = 45; i < 49; i++)
                insert(137, i, "                                                             ");
        }
        public void pokaInterfejs()              // wyswietla interfejs graficzny programu
        {
            insert(1, 1, "");
            for (int i = 0; i < wierza.Length; i++)
            {
                Console.WriteLine(wierza[i]);
            }
        }
        public void pokaListeLotow(Radar r, int czy)
        {
            if (czy > 0)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < r.s.Count; i++)
            {
                insert(119, 32 + i * 2, "[" + Convert.ToString(i + 1) + "] " + r.s[i].typ + " - " + r.s[i].stanLotu);
            }
            Console.ResetColor();
        }
        public void pokaLot(Radar r, int i)
        {
            string xx = "", yy = "";
            if (i>=0)
            {
                if (r.s[i].x < 19) xx = "0";
                if (r.s[i].y < 19) yy = "0";
                insert(138, 29, "Dane lotu: " + r.s[i].typ);
                insert(138, 30, "----------------------------------------------------------------------");
                insert(138, 32, "Wspolrzedne geograficzne: (4." + yy + r.s[i].y + "' S, 21." + xx + (r.s[i].x+1)/2 + "' E)");
                insert(138, 33, "Pulap: " + r.s[i].h + " m  n.p.m.");
                insert(138, 34, "Predkosc: " + r.s[i].v + " kmph ("+(r.s[i].v*0.62)+" mph)");
                insert(138, 35, "Kierunek: " + kierunek(r.s[i].d));
                insert(138, 37, "[1] Wyswietl/Ukryj traiektorie");
                insert(138, 38, "[0] Opusc okno Zarzadzania Lotem");
            }
        }
        public void pokaSamolot(ref Trasa pozycja)
        {
            int tmp = 3;
            string[] samolot = System.IO.File.ReadAllLines("samolot.txt");
            for (int i=1;i<22;i++) insert(1, i, wierza[i-1]);
            if (pozycja.x < 116)    // przesuwanie sie w prawo
            {
                if (pozycja.x == 1) tmp = 0;
                
                insert(pozycja.x, pozycja.y + 1, samolot[0 + tmp]);
                insert(pozycja.x + 1, pozycja.y + 2, samolot[1 + tmp]);
                insert(pozycja.x + 7, pozycja.y + 3, samolot[2 + tmp]);

                if (pozycja.y < 4 ) pozycja.y++;
                pozycja.x += pozycja.s/16;                                 // predkosc opadania (mniej-dluzej)
                if (pozycja.s > 35) pozycja.s = pozycja.s * 9 / 10;        // hamowanie (mniej-szybsze)

                insert(6, 3, "|    ||");
                insert(6, 4, "|    ||");
                insert(6, 5, "|    ||");
            }
            else if (pozycja.y<22)   // przesuwanie w dol
            {
                pozycja.y++;
                pozycja.x++;

                if (pozycja.y < 7)
                {
                    pozycja.y--;
                    pozycja.x += 4;
                    insert(pozycja.x + 1, pozycja.y + 0, samolot[6]);
                    insert(pozycja.x + 0, pozycja.y + 1, samolot[7]);
                    insert(pozycja.x + 3, pozycja.y + 2, samolot[8]);
                    insert(pozycja.x + 0, pozycja.y + 3, samolot[9]);
                    insert(pozycja.x + 0, pozycja.y + 4, samolot[10]);
                    insert(pozycja.x + 6, pozycja.y + 5, samolot[11]);
                    pozycja.y++;
                }
                else
                {
                    insert(pozycja.x + 4, pozycja.y + 0, samolot[12]);
                    insert(pozycja.x + 6, pozycja.y + 1, samolot[13]);
                    insert(pozycja.x + 0, pozycja.y + 2, samolot[14]);
                    insert(pozycja.x + 1, pozycja.y + 3, samolot[15]);
                    insert(pozycja.x + 8, pozycja.y + 4, samolot[16]);
                    insert(pozycja.x + 9, pozycja.y + 5, samolot[17]);
                }
                for (int i = 22; i < 28; i++) insert(1, i, wierza[i - 1]);
            }
            else pozycja.s = 0;
        }
    }
    class Trasa
    {
        public int x, y, h, d, s;
        public Trasa(int xx,int yy, int hh, int dd, int ss)
        {
            x = xx;
            y = yy;
            h = hh;
            d = dd;
            s = ss;
        }
    }
    class Statek
    {
        public string typ,stanLotu;
        public int x, y, h, d, v;   // [x,y,h]- wspolrzedne, d- kierunek, v- predkosc
        public int szybkosc;
        public int traiektoria;
        List<Trasa> tr;             // traiektoria
        public Statek(string[] dane)
        {
            typ = dane[0];
            x = int.Parse(dane[1]);
            y = int.Parse(dane[2]);
            h = int.Parse(dane[3]);
            d = int.Parse(dane[4]);
            v = int.Parse(dane[5]);
            szybkosc = 0;
            traiektoria = 0;
            ustalTraiektorie(x, y, d);
            stanLotu = "Spoko";
        }
        public Statek()
        {
            typ = "";
            x = 0;
            y = 0;
            h = 0;
            d = 0;
            v = 0;
            szybkosc = 0;
            traiektoria = 0;
            ustalTraiektorie(x, y, d);
            stanLotu = "";
        }
        public void ustalTraiektorie(int x_cel,int y_cel,int direct)
        {
            tr = new List<Trasa>();
            tr.Add(new Trasa(x, y, h, d, 0));
            int i = 1;
            int xx = x, yy = y, hh = h, dd = d, vv = v;

            /*
            if(x_cel!=x || y_cel!=y)    // znajdz korzystniejsza trase
            {
                Statek s1 = new Statek();
                while()
                {
                    s1.ustalTraiektorie()
                }
            }
            */

            while(xx<96 && yy<32 && xx > 0 && yy > 0)
            {
                switch(dd)
                {
                    case 0:  xx++; break;
                    case 1:  xx+=2; yy++; break;
                    case 2:  yy++; break;
                    case 3:  xx-=2; yy++; break;
                    case 4:  xx--; break;
                    case 5:  xx-=2; yy--; break;
                    case 6:  yy--; break;
                    case 7:  xx+=2; yy--; break;
                }
                vv++;
                tr.Add(new Trasa(xx,yy,hh,dd,vv));
                i = tr.Count - 1;
                xx = tr[i].x;
                yy = tr[i].y;
                hh = tr[i].h;
                dd = tr[i].d;
                vv = tr[i].s;
            }
        }
        public void pokaTraiektorie()
        {
            Baza b =new Baza();
            for(int i=1;i<tr.Count;i++)
            {
                if (tr[i].d % 4 == 0 && tr[i].x % 2 == 1) b.insert(11 + tr[i].x, 18 + tr[i].y, " ");
                else b.insert(11 + tr[i].x, 18 + tr[i].y, "^");
            }
        }
        public void aktualizujTraiektorie()
        {
            tr.RemoveAt(1);
        }
        public void zmienTraiektorie()
        {
            Baza b = new Baza();
            b.insert(138, 40, "Podaj nowe wspolrzedne: S 21.");
            int y = Convert.ToInt32(Console.ReadLine());
            b.insert(169, 40, ", E 4.");
            int x = Convert.ToInt32(Console.ReadLine());
            ustalTraiektorie(x, y, d);
            b.insert(138, 40, "                                         ");
        }
    }
    class Radar
    {
        public List<Statek> s = new List<Statek>();
        string[] mapa = System.IO.File.ReadAllLines("mapa.txt");
        public int run=-1;
        public void pokaRadar()
        {
            Baza b = new Baza();
            for (int i = 0; i < mapa.Length; i++)
                b.insert(10, 18 + i, mapa[i]);
            for (int i = 0; i < s.Count; i++)
                {
                    s[i].szybkosc += 1;
                    if ((32000 / s[i].v) <= s[i].szybkosc && s[i].d % 2 == 1)        // 560km/h - 1k/2.23s      (skos)
                    {
                        switch (s[i].d)
                        {
                            case 1:s[i].x += 2; s[i].y++; break;
                            case 3:s[i].x -= 2; s[i].y++; break;
                            case 5:s[i].x -= 2; s[i].y--; break;
                            case 7:s[i].x += 2; s[i].y--; break;
                        }
                        s[i].szybkosc = 0;
                        s[i].aktualizujTraiektorie();
                    }
                    else if ((22400 / s[i].v) <= s[i].szybkosc && s[i].d % 4 == 2)   // 560km/h - 1k/2s         (pion)
                    {
                        switch (s[i].d)
                        {
                            case 2:s[i].y++; break;
                            case 6:s[i].y--; break;
                        }
                        s[i].szybkosc = 0;
                        s[i].aktualizujTraiektorie();
                    }
                    else if ((11200 / s[i].v) <= s[i].szybkosc && s[i].d % 4 == 0)    // 560km/h - 1k/1s         (poziom)
                    {
                        switch (s[i].d)
                        {
                            case 0:s[i].x++; break;
                            case 4:s[i].x--; break;
                        }
                        s[i].szybkosc = 0;
                        s[i].aktualizujTraiektorie();
                    }
                    b.insert(11 + s[i].x, 18 + s[i].y, s[i].typ);
                    if (s[i].traiektoria == 1) { s[i].pokaTraiektorie(); }
                }
        }
        public void Kolizja()
        {
            for (int i = 0; i < s.Count; i++)
            {
                for (int j = i + 1; j < s.Count; j++)
                {
                    if (s[i].h == s[j].h)     // sprawdzenie wysokosci
                    {
                        ;
                    }
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(213, 52);
            Console.SetBufferSize(213, 52);
            Console.Title = "Kontrola Lotow";

            Radar radar = new Radar();
            Baza b = new Baza(radar);
            Trasa losowySamolot = new Trasa(0,0,0,0,0);

            b.pokaInterfejs();
            int ms = 0 ;
            int lot = -1;
            int[] x = new int[1];
            x[0] = 0;
            for(; ; )
            {
                if (radar.run < 0)  b.pokaInterfejs();
                else radar.pokaRadar();                             // aktualizuje i wyswietla radar
                b.pokaListeLotow(radar,x[0]);                       // wypisuje liste lotow do podgledu/edycji
                b.insert(200,26,Convert.ToString(ms/10+" s"));      // wypisuje czas trwania programu
                b.insert(212, 51, Convert.ToString("."));           // wypisuje nic na koncu okna
                
                if (Console.KeyAvailable)                           // pobiera wybrany przycisk
                {
                    char wybor = Console.ReadKey().KeyChar;
                    if(lot>=0)
                    {
                        switch(wybor)
                        {
                            case '0':b.czyscKonsole(); lot = -1; break;
                            case '1': if (radar.s[lot].traiektoria < 1) radar.s[lot].traiektoria++; else radar.s[lot].traiektoria--; break;
                            case '2': radar.s[lot].zmienTraiektorie(); break;
                        }
                    }                                          // zarzadzanie lotem
                    else if (wybor-49 >= 0 && wybor-49 < 10 && wybor-49 <= radar.s.Count)
                    {
                        lot = wybor - 49;
                    }   // wypisuje info o danym locie
                    else if (wybor == 'q')
                    {
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }   // zabija aplikacje
                    else if (wybor == 'e')
                    {

                    }   // info o wlascicielach
                    else if (wybor == 'w')
                    {
                        losowySamolot = new Trasa(1,2,0,0,200);
                    }   // pusc samolot
                    else if (wybor == 'r')
                    {
                        radar.run *= -1;
                    }   // wlaczy/wylacz radar
                }
                b.pokaLot(radar, lot);
                Thread.Sleep(100);
                ms++;
                if (losowySamolot.s != 0) b.pokaSamolot(ref losowySamolot); // ladujacy samolot
            }
        }
    }
} 