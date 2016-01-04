using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Folketinget.FolkService;

namespace Folketinget
{
    public class Db : DbContext
    {
        public Db() : base("name=Db")
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }

        public virtual DbSet<Afstemning> Afstemning { get; set; }
        public virtual DbSet<Afstemningstype> Afstemningstype { get; set; }
        public virtual DbSet<Aktstykke> Aktstykke { get; set; }
        public virtual DbSet<Aktør> Aktør { get; set; }
        public virtual DbSet<AktørAktør> AktørAktør { get; set; }
        public virtual DbSet<AktørAktørRolle> AktørAktørRolle { get; set; }
        public virtual DbSet<Aktørtype> Aktørtype { get; set; }
        public virtual DbSet<Almdel> Almdel { get; set; }
        public virtual DbSet<Dagsordenspunkt> Dagsordenspunkt { get; set; }
        public virtual DbSet<DagsordenspunktDokument> DagsordenspunktDokument { get; set; }
        public virtual DbSet<DagsordenspunktSag> DagsordenspunktSag { get; set; }
        public virtual DbSet<Debat> Debat { get; set; }
        public virtual DbSet<Dokument> Dokument { get; set; }
        public virtual DbSet<DokumentAktør> DokumentAktør { get; set; }
        public virtual DbSet<DokumentAktørRolle> DokumentAktørRolle { get; set; }
        public virtual DbSet<Dokumentation> Dokumentation { get; set; }
        public virtual DbSet<Dokumentkategori> Dokumentkategori { get; set; }
        public virtual DbSet<Dokumenttype> Dokumenttype { get; set; }
        public virtual DbSet<Dokumentstatus> Dokumentstatus { get; set; }
        public virtual DbSet<Emneord> Emneord { get; set; }
        public virtual DbSet<EmneordDokument> EmneordDokument { get; set; }
        public virtual DbSet<EmneordSag> EmneordSag { get; set; }
        public virtual DbSet<Emneordstype> Emneordstype { get; set; }
        public virtual DbSet<EUsag> EUsag { get; set; }
        public virtual DbSet<Forslag> Forslag { get; set; }
        public virtual DbSet<Fil> Fil { get; set; }
        public virtual DbSet<KolloneBeskrivelse> KolloneBeskrivelse { get; set; }
        public virtual DbSet<TabelBeskrivelse> TabelBeskrivelse { get; set; }
        public virtual DbSet<Møde> Møde { get; set; }
        public virtual DbSet<MødeAktør> MødeAktør { get; set; }
        public virtual DbSet<Mødestatus> Mødestatus { get; set; }
        public virtual DbSet<Mødetype> Mødetype { get; set; }
        public virtual DbSet<Nyhed> Nyhed { get; set; }
        public virtual DbSet<Omtryk> Omtryk { get; set; }
        public virtual DbSet<Periode> Periode { get; set; }
        public virtual DbSet<Sag> Sag { get; set; }
        public virtual DbSet<SagAktør> SagAktør { get; set; }
        public virtual DbSet<SagAktørRolle> SagAktørRolle { get; set; }
        public virtual DbSet<SagDokument> SagDokument { get; set; }
        public virtual DbSet<SagDokumentRolle> SagDokumentRolle { get; set; }
        public virtual DbSet<Sagskategori> Sagskategori { get; set; }
        public virtual DbSet<Sagsstatus> Sagsstatus { get; set; }
        public virtual DbSet<Sagstrin> Sagstrin { get; set; }
        public virtual DbSet<SagstrinAktør> SagstrinAktør { get; set; }
        public virtual DbSet<SagstrinAktørRolle> SagstrinAktørRolle { get; set; }
        public virtual DbSet<SagstrinSagstrin> SagstrinSagstrin { get; set; }
        public virtual DbSet<SagstrinDokument> SagstrinDokument { get; set; }
        public virtual DbSet<Sagstrinsstatus> Sagstrinsstatus { get; set; }
        public virtual DbSet<SagstrinTale> SagstrinTale { get; set; }
        public virtual DbSet<Sagstrinstype> Sagstrinstype { get; set; }
        public virtual DbSet<Sagstype> Sagstype { get; set; }
        public virtual DbSet<Stemme> Stemme { get; set; }
        public virtual DbSet<Stemmetype> Stemmetype { get; set; }
        public virtual DbSet<Tale> Tale { get; set; }
        public virtual DbSet<Video> Video { get; set; }
        public virtual DbSet<Slettet> Slettet { get; set; }
        public virtual DbSet<SlettetMap> SlettetMap { get; set; }

        protected override void OnModelCreating(DbModelBuilder b)
        {
            b.Properties().Configure(x => x.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None));
            b.Conventions.Remove<PluralizingTableNameConvention>();

            b.Entity<Afstemning>().HasOptional(x => x.Sagstrin).WithMany(x => x.Afstemning).HasForeignKey(x => x.sagstrinid);
            b.Entity<Afstemning>().HasRequired(x => x.Møde).WithMany(x => x.Afstemning).HasForeignKey(x => x.mødeid).WillCascadeOnDelete(false);
            b.Entity<Afstemning>().HasRequired(x => x.Afstemningstype).WithMany(x => x.Afstemning).HasForeignKey(x => x.typeid).WillCascadeOnDelete(false);
            b.Entity<Stemme>().HasRequired(x => x.Afstemning).WithMany(x => x.Stemme).HasForeignKey(x => x.afstemningid);
            b.Entity<Stemme>().HasRequired(x => x.Aktør).WithMany(x => x.Stemme).HasForeignKey(x => x.aktørid);
            b.Entity<Stemme>().HasRequired(x => x.Stemmetype).WithMany(x => x.Stemme).HasForeignKey(x => x.typeid);
            b.Entity<Møde>().HasRequired(x => x.Periode).WithMany(x => x.Møde).HasForeignKey(x => x.periodeid);
            b.Entity<Aktør>().HasOptional(x => x.Periode).WithMany(x => x.Aktør).HasForeignKey(x => x.periodeid);
            b.Entity<Aktør>().HasRequired(x => x.Aktørtype).WithMany(x => x.Aktør).HasForeignKey(x => x.typeid);
            b.Entity<Aktør>().HasMany(x => x.FraAktørAktør).WithRequired(x => x.FraAktør).HasForeignKey(x => x.fraaktørid).WillCascadeOnDelete(false);
            b.Entity<Aktør>().HasMany(x => x.TilAktørAktør).WithRequired(x => x.TilAktør).HasForeignKey(x => x.tilaktørid).WillCascadeOnDelete(false);
            b.Entity<Aktør>().HasMany(x => x.MødeAktør).WithRequired(x => x.Aktør).HasForeignKey(x => x.aktørid).WillCascadeOnDelete(false);
            b.Entity<Aktør>().HasMany(x => x.DokumentAktør).WithRequired(x => x.Aktør).HasForeignKey(x => x.aktørid).WillCascadeOnDelete(false);
            b.Entity<Aktør>().HasMany(x => x.SagAktør).WithRequired(x => x.Aktør).HasForeignKey(x => x.aktørid).WillCascadeOnDelete(false);
            b.Entity<Aktør>().HasMany(x => x.SagstrinAktør).WithRequired(x => x.Aktør).HasForeignKey(x => x.aktørid).WillCascadeOnDelete(false);
            b.Entity<Aktør>().HasMany(x => x.Tale).WithRequired(x => x.Aktør).HasForeignKey(x => x.aktørid).WillCascadeOnDelete(false);
            b.Entity<Aktør>().HasMany(x => x.Tale).WithRequired(x => x.Aktør).HasForeignKey(x => x.aktørid).WillCascadeOnDelete(false);
            b.Entity<SagstrinSagstrin>().HasRequired(x => x.FørsteSagstrin).WithMany(x => x.FørsteSagstrinSagstrin).HasForeignKey(x => x.førstesagstrinid).WillCascadeOnDelete(false);
            b.Entity<SagstrinSagstrin>().HasRequired(x => x.AndetSagstrin).WithMany(x => x.AndetSagstrinSagstrin).HasForeignKey(x => x.andetsagstrinid).WillCascadeOnDelete(false);
            b.Entity<SagstrinTale>().HasRequired(x => x.Sagstrin).WithMany(x => x.SagstrinTale).HasForeignKey(x => x.sagstrinid).WillCascadeOnDelete(false);
            b.Entity<SagstrinTale>().HasRequired(x => x.Tale).WithMany(x => x.SagstrinTale).HasForeignKey(x => x.sagstrinid).WillCascadeOnDelete(false);
            b.Entity<Dagsordenspunkt>().HasMany(x => x.DagsordenspunktDokument).WithRequired(x => x.Dagsordenspunkt).HasForeignKey(x => x.dagsordenspunktid).WillCascadeOnDelete(false);
            b.Entity<Dagsordenspunkt>().HasMany(x => x.DagsordenspunktSag).WithRequired(x => x.Dagsordenspunkt).HasForeignKey(x => x.dagsordenspunktid).WillCascadeOnDelete(false);
            b.Entity<Dagsordenspunkt>().HasMany(x => x.DagsordenspunktSag).WithRequired(x => x.Dagsordenspunkt).HasForeignKey(x => x.dagsordenspunktid).WillCascadeOnDelete(false);
            b.Entity<AktørAktør>().HasRequired(x => x.AktørAktørRolle).WithMany(x => x.AktørAktør).HasForeignKey(x => x.rolleid);
            base.OnModelCreating(b);
        }
    }
}