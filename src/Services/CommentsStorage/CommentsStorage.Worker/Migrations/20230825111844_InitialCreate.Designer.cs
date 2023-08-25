﻿// <auto-generated />
using System;
using CommentsStorage.Worker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommentsStorage.Worker.Migrations
{
    [DbContext(typeof(CommentDbContext))]
    [Migration("20230825111844_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CommentsStorage.Worker.Models.Comment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AuthorChannelId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AuthorChannelUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AuthorDisplayName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AuthorProfileImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ChannelId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Etag")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("LikeCount")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTimeOffset>("PublishedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TextDisplay")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TextOriginal")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("TotalReplyCount")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("VideoId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ViewerRating")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
