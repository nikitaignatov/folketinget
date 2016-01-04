using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Services.Client;
using System.Linq;
using System.Threading;
using Folketinget.FolkService;
using Newtonsoft.Json;

namespace Folketinget
{
    class Program
    {
        public static int ThrottlingMs { get; } = 200;

        static void Main(string[] args)
        {
            QuickGen();
            ImportData();
        }

        private static void Load<T>(Container container, Func<Container, DataServiceQuery<T>> selectType) where T : class
        {
            using (var db = new Db())
            {
                db.Database.ExecuteSqlCommand("sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' ");
                var totalExisting = db.Set<T>().Count();
                var collection = new DataServiceCollection<T>(selectType(container).IncludeTotalCount().Skip(totalExisting));
                Save(collection, db);
                while (collection.Continuation != null)
                {
                    Thread.Sleep(ThrottlingMs);

                    Save(collection, db);
                    var response = container.Execute<T>(collection.Continuation);
                    collection = new DataServiceCollection<T>(response);
                }
                db.Database.ExecuteSqlCommand("sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL' ");
            }
        }

        private static void Save<T>(DataServiceCollection<T> dump, Db db) where T : class
        {
            foreach (var entity in dump.ToArray())
            {
                try
                {
                    AddToContext(db, entity);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(new { entity, exception }));
                    // whatever, just move on
                }
            }
        }

        private static Db AddToContext<T>(Db context, T entity) where T : class
        {
            context.Set<T>().AddOrUpdate(entity);
            context.SaveChanges();
            return context;
        }

        private static void ImportData()
        {
            Database.SetInitializer<Db>(new DropCreateDatabaseIfModelChanges<Db>());
            Uri uri = new Uri("http://oda.ft.dk/api/");
            var container = new Container(uri);
            container.SendingRequest2 += Container_SendingRequest2;

            Load(container, x => x.Afstemning);
            Load(container, x => x.Afstemningstype);
            Load(container, x => x.Aktstykke);
            Load(container, x => x.Aktør);
            Load(container, x => x.AktørAktør);
            Load(container, x => x.AktørAktørRolle);
            Load(container, x => x.Aktørtype);
            Load(container, x => x.Almdel);
            Load(container, x => x.Dagsordenspunkt);
            Load(container, x => x.DagsordenspunktDokument);
            Load(container, x => x.DagsordenspunktSag);
            Load(container, x => x.Debat);
            Load(container, x => x.Dokument);
            Load(container, x => x.DokumentAktør);
            Load(container, x => x.DokumentAktørRolle);
            Load(container, x => x.Dokumentation);
            Load(container, x => x.Dokumentkategori);
            Load(container, x => x.Dokumenttype);
            Load(container, x => x.Dokumentstatus);
            Load(container, x => x.Emneord);
            Load(container, x => x.EmneordDokument);
            Load(container, x => x.EmneordSag);
            Load(container, x => x.Emneordstype);
            Load(container, x => x.EUsag);
            Load(container, x => x.Forslag);
            Load(container, x => x.Fil);
            Load(container, x => x.KolloneBeskrivelse);
            Load(container, x => x.TabelBeskrivelse);
            Load(container, x => x.Møde);
            Load(container, x => x.MødeAktør);
            Load(container, x => x.Mødestatus);
            Load(container, x => x.Mødetype);
            Load(container, x => x.Nyhed);
            Load(container, x => x.Omtryk);
            Load(container, x => x.Periode);
            Load(container, x => x.Sag);
            Load(container, x => x.SagAktør);
            Load(container, x => x.SagAktørRolle);
            Load(container, x => x.SagDokument);
            Load(container, x => x.SagDokumentRolle);
            Load(container, x => x.Sagskategori);
            Load(container, x => x.Sagsstatus);
            Load(container, x => x.Sagstrin);
            Load(container, x => x.SagstrinAktør);
            Load(container, x => x.SagstrinAktørRolle);
            Load(container, x => x.SagstrinSagstrin);
            Load(container, x => x.SagstrinDokument);
            Load(container, x => x.Sagstrinsstatus);
            Load(container, x => x.SagstrinTale);
            Load(container, x => x.Sagstrinstype);
            Load(container, x => x.Sagstype);
            Load(container, x => x.Stemme);
            Load(container, x => x.Stemmetype);
            Load(container, x => x.Tale);
            Load(container, x => x.Video);
            Load(container, x => x.Slettet);
            Load(container, x => x.SlettetMap);
        }

        private static void Container_SendingRequest2(object sender, SendingRequest2EventArgs e)
        {
            Console.WriteLine("{0} {1}", e.RequestMessage.Method, e.RequestMessage.Url);
        }

        private static void QuickGen()
        {
            var folketinget = typeof(Container)
                .GetProperties()
                .Where(x => x.PropertyType.IsGenericType)
                .Where(x => x.PropertyType.GenericTypeArguments[0].Namespace == typeof(Container).Namespace);

            var ctx = string.Join(@"{ get; set; }
            public virtual DbSet<", folketinget.Select(x => x.Name + "> " + x.Name));
            var gen = string.Join(@");
            Load(container, x => x.", folketinget.Select(x => x.Name));
            Console.WriteLine(@"public virtual DbSet<{0} {{ get; set; }}", ctx);
            Console.WriteLine("Load(container, x => x.{0});", gen);
        }
    }
}
