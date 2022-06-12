using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Donatech.Core.Model.DbModels
{
    public partial class DonatechDBContext : DbContext
    {
        public DonatechDBContext()
        {
        }

        public DonatechDBContext(DbContextOptions<DonatechDBContext> options)
            : base(options)
        {            
        }

        public virtual DbSet<Comuna> Comunas { get; set; } = null!;
        public virtual DbSet<Mensaje> Mensajes { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<Provincia> Provincias { get; set; } = null!;
        public virtual DbSet<Region> Regions { get; set; } = null!;
        public virtual DbSet<Rol> Rols { get; set; } = null!;
        public virtual DbSet<TipoProducto> TipoProductos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {                
                //optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Donatech;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Modern_Spanish_CI_AS");

            modelBuilder.Entity<Comuna>(entity =>
            {
                entity.ToTable("Comuna");

                entity.Property(e => e.Id).HasDefaultValueSql("('0')");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdProvinciaNavigation)
                    .WithMany(p => p.Comunas)
                    .HasForeignKey(d => d.IdProvincia)
                    .HasConstraintName("FK__Comuna__IdProvin__5070F446");
            });

            modelBuilder.Entity<Mensaje>(entity =>
            {
                entity.ToTable("Mensaje");

                entity.Property(e => e.FchEnvio).HasColumnType("datetime");

                entity.Property(e => e.Mensaje1)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("Mensaje");

                entity.HasOne(d => d.IdEmisorNavigation)
                    .WithMany(p => p.MensajeIdEmisorNavigations)
                    .HasForeignKey(d => d.IdEmisor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mensaje_Usuario");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.Mensajes)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mensaje_Producto");

                entity.HasOne(d => d.IdReceptorNavigation)
                    .WithMany(p => p.MensajeIdReceptorNavigations)
                    .HasForeignKey(d => d.IdReceptor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mensaje_Usuario1");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .IsClustered(false);

                entity.ToTable("Producto");

                entity.Property(e => e.Descripcion).HasMaxLength(300);

                entity.Property(e => e.Estado).HasMaxLength(20);

                entity.Property(e => e.FchFinalizacion).HasColumnType("datetime");

                entity.Property(e => e.FchPublicacion).HasColumnType("datetime");

                entity.Property(e => e.ImagenMimeType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdDemandanteNavigation)
                    .WithMany(p => p.ProductoIdDemandanteNavigations)
                    .HasForeignKey(d => d.IdDemandante)
                    .HasConstraintName("FK_Producto_Usuario1");

                entity.HasOne(d => d.IdOferenteNavigation)
                    .WithMany(p => p.ProductoIdOferenteNavigations)
                    .HasForeignKey(d => d.IdOferente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Producto_Usuario");

                entity.HasOne(d => d.IdTipoNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdTipo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Producto_TipoProducto");
            });

            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("('0')");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(23)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdRegionNavigation)
                    .WithMany(p => p.Provincia)
                    .HasForeignKey(d => d.IdRegion)
                    .HasConstraintName("FK__Provincia__IdReg__4BAC3F29");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("Region");

                entity.Property(e => e.Id).HasDefaultValueSql("('0')");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol);

                entity.ToTable("Rol");

                entity.Property(e => e.IdRol)
                    .ValueGeneratedNever()
                    .HasColumnName("idRol");

                entity.Property(e => e.NombreRol)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("nombreRol");
            });

            modelBuilder.Entity<TipoProducto>(entity =>
            {
                entity.ToTable("TipoProducto");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.HasIndex(e => e.Run, "IX_Usuario")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "IX_Usuario_1")
                    .IsUnique();

                entity.Property(e => e.Apellidos).HasMaxLength(80);

                entity.Property(e => e.Celular)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Nombre).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Run)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.IdComunaNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdComuna)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_Comuna");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_Rol");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
