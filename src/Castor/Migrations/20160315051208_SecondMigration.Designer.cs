using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Castor.Models;

namespace Castor.Migrations
{
    [DbContext(typeof(BloggingContext))]
    [Migration("20160315051208_SecondMigration")]
    partial class SecondMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Castor.Models.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("BlogId");
                });

            modelBuilder.Entity("Castor.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("CategoryId");
                });

            modelBuilder.Entity("Castor.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlogId");

                    b.Property<int>("CategoryId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Email");

                    b.Property<string>("Title");

                    b.Property<int>("UserId");

                    b.HasKey("PostId");
                });

            modelBuilder.Entity("Castor.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<string>("DisplayName");

                    b.Property<string>("Password");

                    b.Property<string>("Username");

                    b.HasKey("UserId");
                });

            modelBuilder.Entity("Castor.Models.Post", b =>
                {
                    b.HasOne("Castor.Models.Blog")
                        .WithMany()
                        .HasForeignKey("BlogId");

                    b.HasOne("Castor.Models.Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("Castor.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
        }
    }
}
