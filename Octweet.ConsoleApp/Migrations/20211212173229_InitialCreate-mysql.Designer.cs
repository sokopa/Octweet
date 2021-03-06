// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Octweet.Data;

#nullable disable

namespace Octweet.ConsoleApp.Migrations
{
    [DbContext(typeof(OctweetDbContext))]
    [Migration("20211212173229_InitialCreate-mysql")]
    partial class InitialCreatemysql
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Octweet.Data.Abstractions.EntityAnnotation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Locale")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TweetMediaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("EntityAnnotations");
                });

            modelBuilder.Entity("Octweet.Data.Abstractions.QueryLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("LatestExecution")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LatestTweetId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Query")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("QueryLog");
                });

            modelBuilder.Entity("Octweet.Data.Abstractions.Tweet", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Tweets");
                });

            modelBuilder.Entity("Octweet.Data.Abstractions.TweetMedia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AnnotationId")
                        .HasColumnType("int");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("MediaKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("ProcessedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TweetId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TweetId");

                    b.ToTable("TweetsMedia");
                });

            modelBuilder.Entity("Octweet.Data.Abstractions.TweetMedia", b =>
                {
                    b.HasOne("Octweet.Data.Abstractions.Tweet", "Tweet")
                        .WithMany("Media")
                        .HasForeignKey("TweetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tweet");
                });

            modelBuilder.Entity("Octweet.Data.Abstractions.Tweet", b =>
                {
                    b.Navigation("Media");
                });
#pragma warning restore 612, 618
        }
    }
}
