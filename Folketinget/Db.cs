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
        public virtual DbSet<Akt�r> Akt�r { get; set; }
        public virtual DbSet<Akt�rAkt�r> Akt�rAkt�r { get; set; }
        public virtual DbSet<Akt�rAkt�rRolle> Akt�rAkt�rRolle { get; set; }
        public virtual DbSet<Akt�rtype> Akt�rtype { get; set; }
        public virtual DbSet<Almdel> Almdel { get; set; }
        public virtual DbSet<Dagsordenspunkt> Dagsordenspunkt { get; set; }
        public virtual DbSet<DagsordenspunktDokument> DagsordenspunktDokument { get; set; }
        public virtual DbSet<DagsordenspunktSag> DagsordenspunktSag { get; set; }
        public virtual DbSet<Debat> Debat { get; set; }
        public virtual DbSet<Dokument> Dokument { get; set; }
        public virtual DbSet<DokumentAkt�r> DokumentAkt�r { get; set; }
        public virtual DbSet<DokumentAkt�rRolle> DokumentAkt�rRolle { get; set; }
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
        public virtual DbSet<M�de> M�de { get; set; }
        public virtual DbSet<M�deAkt�r> M�deAkt�r { get; set; }
        public virtual DbSet<M�destatus> M�destatus { get; set; }
        public virtual DbSet<M�detype> M�detype { get; set; }
        public virtual DbSet<Nyhed> Nyhed { get; set; }
        public virtual DbSet<Omtryk> Omtryk { get; set; }
        public virtual DbSet<Periode> Periode { get; set; }
        public virtual DbSet<Sag> Sag { get; set; }
        public virtual DbSet<SagAkt�r> SagAkt�r { get; set; }
        public virtual DbSet<SagAkt�rRolle> SagAkt�rRolle { get; set; }
        public virtual DbSet<SagDokument> SagDokument { get; set; }
        public virtual DbSet<SagDokumentRolle> SagDokumentRolle { get; set; }
        public virtual DbSet<Sagskategori> Sagskategori { get; set; }
        public virtual DbSet<Sagsstatus> Sagsstatus { get; set; }
        public virtual DbSet<Sagstrin> Sagstrin { get; set; }
        public virtual DbSet<SagstrinAkt�r> SagstrinAkt�r { get; set; }
        public virtual DbSet<SagstrinAkt�rRolle> SagstrinAkt�rRolle { get; set; }
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
            b.Entity<Afstemning>().HasRequired(x => x.M�de).WithMany(x => x.Afstemning).HasForeignKey(x => x.m�deid).WillCascadeOnDelete(false);
            b.Entity<Afstemning>().HasRequired(x => x.Afstemningstype).WithMany(x => x.Afstemning).HasForeignKey(x => x.typeid).WillCascadeOnDelete(false);
            b.Entity<Stemme>().HasRequired(x => x.Afstemning).WithMany(x => x.Stemme).HasForeignKey(x => x.afstemningid);
            b.Entity<Stemme>().HasRequired(x => x.Akt�r).WithMany(x => x.Stemme).HasForeignKey(x => x.akt�rid);
            b.Entity<Stemme>().HasRequired(x => x.Stemmetype).WithMany(x => x.Stemme).HasForeignKey(x => x.typeid);
            b.Entity<M�de>().HasRequired(x => x.Periode).WithMany(x => x.M�de).HasForeignKey(x => x.periodeid);
            b.Entity<Akt�r>().HasOptional(x => x.Periode).WithMany(x => x.Akt�r).HasForeignKey(x => x.periodeid);
            b.Entity<Akt�r>().HasRequired(x => x.Akt�rtype).WithMany(x => x.Akt�r).HasForeignKey(x => x.typeid);
            b.Entity<Akt�r>().HasMany(x => x.FraAkt�rAkt�r).WithRequired(x => x.FraAkt�r).HasForeignKey(x => x.fraakt�rid).WillCascadeOnDelete(false);
            b.Entity<Akt�r>().HasMany(x => x.TilAkt�rAkt�r).WithRequired(x => x.TilAkt�r).HasForeignKey(x => x.tilakt�rid).WillCascadeOnDelete(false);
            b.Entity<Akt�r>().HasMany(x => x.M�deAkt�r).WithRequired(x => x.Akt�r).HasForeignKey(x => x.akt�rid).WillCascadeOnDelete(false);
            b.Entity<Akt�r>().HasMany(x => x.DokumentAkt�r).WithRequired(x => x.Akt�r).HasForeignKey(x => x.akt�rid).WillCascadeOnDelete(false);
            b.Entity<Akt�r>().HasMany(x => x.SagAkt�r).WithRequired(x => x.Akt�r).HasForeignKey(x => x.akt�rid).WillCascadeOnDelete(false);
            b.Entity<Akt�r>().HasMany(x => x.SagstrinAkt�r).WithRequired(x => x.Akt�r).HasForeignKey(x => x.akt�rid).WillCascadeOnDelete(false);
            b.Entity<Akt�r>().HasMany(x => x.Tale).WithRequired(x => x.Akt�r).HasForeignKey(x => x.akt�rid).WillCascadeOnDelete(false);
            b.Entity<Akt�r>().HasMany(x => x.Tale).WithRequired(x => x.Akt�r).HasForeignKey(x => x.akt�rid).WillCascadeOnDelete(false);
            b.Entity<SagstrinSagstrin>().HasRequired(x => x.F�rsteSagstrin).WithMany(x => x.F�rsteSagstrinSagstrin).HasForeignKey(x => x.f�rstesagstrinid).WillCascadeOnDelete(false);
            b.Entity<SagstrinSagstrin>().HasRequired(x => x.AndetSagstrin).WithMany(x => x.AndetSagstrinSagstrin).HasForeignKey(x => x.andetsagstrinid).WillCascadeOnDelete(false);
            b.Entity<SagstrinTale>().HasRequired(x => x.Sagstrin).WithMany(x => x.SagstrinTale).HasForeignKey(x => x.sagstrinid).WillCascadeOnDelete(false);
            b.Entity<SagstrinTale>().HasRequired(x => x.Tale).WithMany(x => x.SagstrinTale).HasForeignKey(x => x.sagstrinid).WillCascadeOnDelete(false);
            b.Entity<Dagsordenspunkt>().HasMany(x => x.DagsordenspunktDokument).WithRequired(x => x.Dagsordenspunkt).HasForeignKey(x => x.dagsordenspunktid).WillCascadeOnDelete(false);
            b.Entity<Dagsordenspunkt>().HasMany(x => x.DagsordenspunktSag).WithRequired(x => x.Dagsordenspunkt).HasForeignKey(x => x.dagsordenspunktid).WillCascadeOnDelete(false);
            b.Entity<Dagsordenspunkt>().HasMany(x => x.DagsordenspunktSag).WithRequired(x => x.Dagsordenspunkt).HasForeignKey(x => x.dagsordenspunktid).WillCascadeOnDelete(false);
            b.Entity<Akt�rAkt�r>().HasRequired(x => x.Akt�rAkt�rRolle).WithMany(x => x.Akt�rAkt�r).HasForeignKey(x => x.rolleid);
            base.OnModelCreating(b);
        }
    }
}