using System;
using System.Collections.Generic;
using System.Linq;

namespace Banka2
{
    class Program
    {
        static void Main(string[] args)
        {

            bool islogin = false;
            musteri currentmusteri = null;
            hesap aktifhesap = null;
            int gunluklimit = 5000;
            List<musteri> musteriler = new List<musteri>();
            veriata();

            int secim = -1;
            while (secim != 0)
            {
                //Console.Clear();
                Console.WriteLine("1- yeni kullanıcı oluştur");
                Console.WriteLine("2- kullanıcı girişi yap");
                Console.WriteLine("3- hesap oluştur");
                Console.WriteLine("4- hesap seç");
                Console.WriteLine("5- hesaptan para çek");
                Console.WriteLine("6- hesaba para transferi yap");
                Console.WriteLine("7- hesabın döviz karşılığı");
                Console.WriteLine("8- hesabı kapat");
                Console.WriteLine("9- Oturumu kapat");
                Console.WriteLine("0- Uygulamayı Kapat");
                Console.Write("Seçiminiz:");
                secim = Convert.ToInt32(Console.ReadLine());


                switch (secim)
                {
                    case 1:
                        yenikullaniciolustur();
                        break;
                    case 2:
                        login();
                        break;
                    case 3:
                        hesapolustur();
                        break;
                    case 4:
                        hesapsec();
                        break;
                    case 5:
                        hesaptanparacek();
                        break;
                    case 6:
                        hesabatransfer();
                        break;
                    case 7:
                        dovizkarsiligigoster();
                        //doviz uygulaması sabah yapıldı
                        break;
                    case 8:
                        hesabikapat();
                        break;
                    case 9:
                        oturumukapat();
                        break;
                    case 0:
                        break;
                }

            }

            hesapolustur();
            currentmusteri.hesaplar[0].bakiye += 100;
            Console.Write("Güncel bakiye: " + currentmusteri.hesaplar[0].bakiye);


            //bir musterinin birden fazla hesabı olabilir. 
            //kişinin yeni hesap oluşturmasını sağlayın. 
            //hesapoluştur() - (hesapno, bakiye) -> m1 için bir hesap ekleyin.. 



            void yenikullaniciolustur()
            {
                Console.Write("TC:");
                int tc = Convert.ToInt32(Console.ReadLine());
                Console.Write("isim:");
                string isim = Console.ReadLine();
                Console.Write("bakiye:");
                int bakiye = Convert.ToInt32(Console.ReadLine());
                Console.Write("password:");
                string password = Console.ReadLine();
                var kullanici = new musteri(tc, isim, bakiye, password);
                musteriler.Add(kullanici);
                Console.WriteLine("başarıyla hesap oluşturdunuz!");
            }
            void hesapolustur()
            {
                if (islogin)
                {
                    Console.Write("hesapno girin:");
                    int yenihesapno = int.Parse(Console.ReadLine());

                    Console.Write("bakiye girin:");
                    int yenibakiye = int.Parse(Console.ReadLine());

                    currentmusteri.hesaplar.Add(new hesap(yenihesapno, yenibakiye));
                    Console.WriteLine("Hesap başarıyla oluşturuldu!");
                }
                else
                {
                    Console.WriteLine("Önce üye girişi yapmalısınız!");
                }
            }
            void hesapsec()
            {
                if (islogin)
                {
                    currentmusteri.hesaplistele();
                    Console.Write("hangi hesap:");
                    int sayi = int.Parse(Console.ReadLine());
                    aktifhesap = currentmusteri.hesaplar[sayi - 1];
                    Console.WriteLine($"{aktifhesap.hesapno} nolu hesap başarılya seçildi");
                }
                else
                {
                    Console.WriteLine("Önce üye girişi yapmalısınız!");
                }

            }
            void veriata()
            {
                musteri m1 = new musteri();
                m1.bakiye = 2000;
                m1.isim = "ayse";
                m1.tcno = 1;

                musteri m2 = new musteri(2, "fatma", 10000, "abc");
                m2.hesaplar.Add(new hesap(21, 1000));
                m2.hesaplar.Add(new hesap(34, 55000));

                musteriler.Add(m1);
                musteriler.Add(m2);
                musteriler.Add(new musteri(3, "ahmet", 20000, "123"));

            }
            void login()
            {
                int tcgelen = -1;
                while (!islogin && tcgelen != 0)
                {
                    Console.Write("tc girin(çıkış için 0):");
                    tcgelen = int.Parse(Console.ReadLine());
                    Console.Write("şifre girin:");
                    string sifregelen = Console.ReadLine();

                    //foreach (musteri m in musteriler)
                    //{
                    //    if (m.tcno == tcgelen && m.password == sifregelen)
                    //    {
                    //        islogin = true;
                    //        currentmusteri = m;
                    //        break;
                    //    }
                    //}
                    currentmusteri = musteriler.Where(musteri => musteri.tcno == tcgelen && musteri.password == sifregelen).FirstOrDefault();
                    if (currentmusteri != null)
                    {
                        islogin = true;
                        Console.WriteLine("başarıyla giriş yaptınız!");
                    }
                    else
                        islogin = false;

                }
            }
            void hesaptanparacek()
            {
                if (islogin && aktifhesap != null)
                {

                    Console.Write("Çekilecek miktarı girin:");
                    int cekilenmiktar = int.Parse(Console.ReadLine());
                    if (cekilenmiktar > aktifhesap.bakiye)
                    {
                        Console.WriteLine($"işleminiz yapılamıyor, hesabınızda {aktifhesap.bakiye} tl var, bu yetersiz.");
                    }
                    else
                    {
                        if (cekilenmiktar > gunluklimit)
                        {
                            Console.WriteLine($"işleminiz yapılamıyor, günlük kimitiniz  {gunluklimit} tl kaldı");
                        }
                        else
                        {
                            gunluklimit -= cekilenmiktar;
                            aktifhesap.bakiye -= cekilenmiktar;
                            Console.WriteLine($"hesabınızdan {cekilenmiktar} tl çekildi, {aktifhesap.bakiye} tl kaldı");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Önce kullanıcı girişi yapıp hesap seçmelisiniz!");
                }
            }
            void hesabatransfer()
            {
                if (islogin && aktifhesap != null)
                {
                    Console.Write("Para gönderilecek hesap no:");
                    int hesapno = int.Parse(Console.ReadLine());
                    Console.Write("Gönderilecek hpara miktarını giriniz:");
                    int paramiktar = int.Parse(Console.ReadLine());

                    bool gidecekhesapbulundu = false;
                    hesap gidecekhesap = null;

                    foreach (musteri m in musteriler)
                    {
                        foreach (hesap h in m.hesaplar)
                        {
                            if (h.hesapno == hesapno)
                            {
                                gidecekhesapbulundu = true;
                                gidecekhesap = h;

                                break;
                            }
                        }
                        if (gidecekhesapbulundu) break;
                    }

                    if (gidecekhesapbulundu && gidecekhesap != null)
                    {
                        if (aktifhesap.bakiye >= paramiktar)
                        {
                            aktifhesap.bakiye -= paramiktar;
                            gidecekhesap.bakiye += paramiktar;
                            Console.WriteLine("Para aktarımınız gerçekleşti... ");
                        }
                        else
                        {
                            Console.WriteLine("hesabınızda transfer edecek kadar para bulunmuyor!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Bu isimde bir gönderi hesabı bulunamadı!");
                    }

                }
                else
                {
                    Console.WriteLine("Önce kullanıcı girişi yapıp hesap seçmelisiniz!");
                }
            }
            void hesabikapat()
            {
                currentmusteri.hesaplistele();
                Console.Write("hesap seçin:");
                int secim = Convert.ToInt32(Console.ReadLine());

                if (currentmusteri.hesaplar[secim - 1].bakiye > 0)
                {
                    Console.WriteLine("hesabınızda para bulurken, silme işlemi yapılamaz, önce hesabı sıfırlamalısınız");
                }
                else
                {
                    currentmusteri.hesaplar.RemoveAt(secim - 1);
                }
            }
            void oturumukapat()
            {
                currentmusteri = null;
                aktifhesap = null;
                islogin = false;
                Console.Clear();
                Console.WriteLine("Çıkışınız yapıldı");
            }
            void dovizkarsiligigoster()
            {
                if (islogin && aktifhesap != null)
                {
                    Console.WriteLine($"hesabınızda {aktifhesap.bakiye} tl vardır, dolar karşılığı {aktifhesap.bakiye / 18.6} dir");
                }
                else
                {
                    Console.WriteLine("Önce kullanıcı girişi yapıp hesap seçmelisiniz!");
                }

                // musteri den tc ve sifre isteyin, bu kişi varsa login olsun, yoksa tekrar giriş istesin.. 
            }
        }
    }

    class musteri
    {
        public int tcno;
        public string isim;
        public float bakiye;
        public string password;
        public List<hesap> hesaplar = new List<hesap>();

        public musteri()
        {
            Console.WriteLine("yeni müşteri oluştu");
        }
        public musteri(int tcgelen,string isimgelen,float bakiyegelen, string gelenpassword) //overload
        {
            tcno = tcgelen;
            isim = isimgelen;
            bakiye = bakiyegelen;
            password = gelenpassword;
        }
        public void hesaplistele()
        {
            int i = 1;
            foreach (hesap h in hesaplar)
            {
                Console.WriteLine($"{i}- {h.hesapno} bakiyesi: {h.bakiye}");
                i++;
            }
        }
    }
    class hesap
    {
        public int hesapno;
        public float bakiye;
        public hesap(int gelenhesap, int gelenbakiye)
        {
            hesapno = gelenhesap;
            bakiye = gelenbakiye;  
        }
        
    }
}
