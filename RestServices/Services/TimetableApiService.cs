using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Interfaces.Services;
using Core.TimetableApiModels;

namespace RestServices.Services
{
    public class TimetableApiService : BaseHttpService, ITimetableApiService
    {
        public TimetableApiService(HttpClient httpClient) : base(httpClient) { }
        
        public Task<List<Departments.Response>> DepartmentsAsync()
        {
            const string json = "[{\"id\":20,\"name\":\"Antrenörlük Eğitimi\"},{\"id\":54,\"name\":\"Bahçe ve Tarla Bitkileri\"},{\"id\":22,\"name\":\"Batı Dilleri (İngiliz Dili ve Edebiyatı)\"},{\"id\":19,\"name\":\"Beden Eğitimi ve Spor Öğretmenliği\"},{\"id\":9,\"name\":\"Bilgisayar Mühendisliği\"},{\"id\":58,\"name\":\"Bilgisayar Programcılığı (MYO)\"},{\"id\":55,\"name\":\"Bitki Koruma\"},{\"id\":18,\"name\":\"Biyoloji\"},{\"id\":43,\"name\":\"Büro Yönetimi ve Yönetici Asistanlığı\"},{\"id\":10,\"name\":\"Çevre Mühendisliği\"},{\"id\":44,\"name\":\"Çocuk Gelişimi\"},{\"id\":38,\"name\":\"Din Bilimleri\"},{\"id\":23,\"name\":\"Doğu Dilleri (Çin Dili ve Edebiyatı)\"},{\"id\":24,\"name\":\"Doğu Dilleri (Rus Dili ve Edebiyatı)\"},{\"id\":25,\"name\":\"Eğitim Bilimleri (Pedagojik Formasyon)\"},{\"id\":26,\"name\":\"Eğitim Bilimleri (Rehberlik ve Psikolojik Danışmanlık)\"},{\"id\":56,\"name\":\"Elektrik-Elektronik Mühendisliği\"},{\"id\":57,\"name\":\"Endüstri Mühendisliği\"},{\"id\":66,\"name\":\"FBE - Bahçe ve Tarla Bitkileri ABD\"},{\"id\":61,\"name\":\"FBE - Bilgisayar Mühendisliği ABD\"},{\"id\":67,\"name\":\"FBE - Bitki Koruma ABD\"},{\"id\":68,\"name\":\"FBE - Biyoloji ABD\"},{\"id\":69,\"name\":\"FBE - Biyoteknoloji ABD\"},{\"id\":70,\"name\":\"FBE - Çevre Mühendisliği ABD\"},{\"id\":71,\"name\":\"FBE - Gıda Mühendisliği ABD\"},{\"id\":72,\"name\":\"FBE - Kimya Mühendisliği ABD\"},{\"id\":65,\"name\":\"FBE - Matematik ABD\"},{\"id\":27,\"name\":\"Felsefe\"},{\"id\":16,\"name\":\"Finans ve Bankacılık\"},{\"id\":50,\"name\":\"Gastronomi ve Mutfak Sanatları\"},{\"id\":40,\"name\":\"Gazetecilik\"},{\"id\":11,\"name\":\"Gıda Mühendisliği\"},{\"id\":34,\"name\":\"Grafik\"},{\"id\":41,\"name\":\"Halkla İlişkiler ve Reklamcılık\"},{\"id\":14,\"name\":\"İktisat\"},{\"id\":45,\"name\":\"İnşaat\"},{\"id\":39,\"name\":\"İslam Bilimleri\"},{\"id\":15,\"name\":\"İşletme\"},{\"id\":17,\"name\":\"Kimya Mühendisliği\"},{\"id\":12,\"name\":\"Matematik\"},{\"id\":59,\"name\":\"Metal ve Kaynak Teknolojisi\"},{\"id\":60,\"name\":\"Mobilya ve Dekorasyon\"},{\"id\":46,\"name\":\"Muhasebe\"},{\"id\":28,\"name\":\"Mütercim-Tercümanlık (Kırgızca-İngilizce)\"},{\"id\":29,\"name\":\"Mütercim-Tercümanlık (Kırgızca-Türkçe)\"},{\"id\":30,\"name\":\"Mütercim-Tercümanlık (Türkçe-Rusça)\"},{\"id\":35,\"name\":\"Müzik\"},{\"id\":47,\"name\":\"Otomotiv\"},{\"id\":48,\"name\":\"Pazarlama\"},{\"id\":42,\"name\":\"Radyo-Televizyon ve Sinema\"},{\"id\":36,\"name\":\"Resim\"},{\"id\":37,\"name\":\"Sahne Sanatları\"},{\"id\":62,\"name\":\"SBE - Felsefe\"},{\"id\":64,\"name\":\"SBE - Mutercim Tercumanlik\"},{\"id\":51,\"name\":\"Seyahat İşletmeciliği ve Turizm Rehberliği\"},{\"id\":31,\"name\":\"Sosyoloji\"},{\"id\":32,\"name\":\"Tarih\"},{\"id\":52,\"name\":\"Turizm ve Otel İşletmeciliği\"},{\"id\":49,\"name\":\"Turizm ve Otel İşletmeciliği (MYO)\"},{\"id\":33,\"name\":\"Türkoloji\"},{\"id\":21,\"name\":\"Uluslararası İlişkiler\"},{\"id\":13,\"name\":\"Uygulamalı Matematik ve Enformatik\"},{\"id\":53,\"name\":\"Veteriner\"}]";
            return Task.FromResult(Deserialize<List<Departments.Response>>(json));
        }

        public Task<List<Faculties.Response>> FacultiesAsync()
        {
            const string json = "[{\"id\":10,\"name\":\"BEDEN EĞİTİMİ VE SPOR YÜKSEKOKULU\"},{\"id\":9,\"name\":\"EDEBİYAT FAKÜLTESİ\"},{\"id\":19,\"name\":\"FEN BİLİMLER ENSTİTÜSÜ\"},{\"id\":6,\"name\":\"FEN FAKÜLTESİ\"},{\"id\":11,\"name\":\"GÜZEL SANATLAR FAKÜLTESİ\"},{\"id\":7,\"name\":\"İKTİSADİ VE İDARİ BİLİMLER FAKÜLTESİ\"},{\"id\":12,\"name\":\"İLAHİYAT FAKÜLTESİ\"},{\"id\":8,\"name\":\"İLETİŞİM FAKÜLTESİ\"},{\"id\":13,\"name\":\"MESLEK YÜKSEKOKULU\"},{\"id\":5,\"name\":\"MÜHENDİSLİK FAKÜLTESİ\"},{\"id\":18,\"name\":\"SOSYAL BİLİMLER ENSTİTÜSÜ\"},{\"id\":14,\"name\":\"TURİZM VE OTELCİLİK YÜKSEK OKULU\"},{\"id\":15,\"name\":\"VETERİNER FAKÜLTESİ\"},{\"id\":17,\"name\":\"YABANCI DİLLER YÜKSEK OKULU\"},{\"id\":16,\"name\":\"ZİRAAT FAKÜLTESİ\"}]";
            return Task.FromResult(Deserialize<List<Faculties.Response>>(json));
        }

        public Task<Timetable.Response> TimetableAsync(string lessonCode)
        {
            const string json = "{\"faculty\":{\"id\":5,\"name\":\"MÜHENDİSLİK FAKÜLTESİ\"},\"department\":{\"id\":9,\"name\":\"Bilgisayar Mühendisliği\"},\"teacher\":\"MEHMET KENAN DÖNMEZ\",\"timetable\":[{\"time_day\":\"Pazartesi\",\"time_hour\":\"9:45-10:20\",\"classroom\":\"MFFB-522\"},{\"time_day\":\"Salı\",\"time_hour\":\"9:45-10:20\",\"classroom\":\"MFFB-522\"}],\"type\":\"Bölüm Zorunlu Dersi\"}";
            return Task.FromResult(Deserialize<Timetable.Response>(json));
        }
    }
}
