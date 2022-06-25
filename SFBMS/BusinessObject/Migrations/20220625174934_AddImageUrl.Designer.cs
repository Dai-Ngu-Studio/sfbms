﻿// <auto-generated />
using System;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BusinessObject.Migrations
{
    [DbContext(typeof(SfbmsDbContext))]
    [Migration("20220625174934_AddImageUrl")]
    partial class AddImageUrl
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BusinessObject.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("money")
                        .HasColumnName("total_price");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(128)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("BusinessObject.BookingDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("BookingId")
                        .HasColumnType("int")
                        .HasColumnName("booking_id");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("end_time");

                    b.Property<int?>("FieldId")
                        .HasColumnType("int")
                        .HasColumnName("field_id");

                    b.Property<decimal>("Price")
                        .HasColumnType("money")
                        .HasColumnName("price");

                    b.Property<int>("SlotNumber")
                        .HasColumnType("int")
                        .HasColumnName("slot_number");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("start_time");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(128)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("FieldId");

                    b.HasIndex("UserId");

                    b.ToTable("BookingDetails");
                });

            modelBuilder.Entity("BusinessObject.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("BusinessObject.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(800)")
                        .HasColumnName("content");

                    b.Property<DateTime>("FeedbackTime")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("feedback_time");

                    b.Property<int?>("FieldId")
                        .HasColumnType("int")
                        .HasColumnName("field_id");

                    b.Property<int>("Rating")
                        .HasColumnType("int")
                        .HasColumnName("rating");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("title");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(128)")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("UserId");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("BusinessObject.Field", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("category_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(600)")
                        .HasColumnName("description");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image_url");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("name");

                    b.Property<int>("NumberOfSlots")
                        .HasColumnType("int")
                        .HasColumnName("number_of_slots");

                    b.Property<decimal>("Price")
                        .HasColumnType("money")
                        .HasColumnName("price");

                    b.Property<double>("TotalRating")
                        .HasColumnType("float")
                        .HasColumnName("total_rating");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Field");
                });

            modelBuilder.Entity("BusinessObject.Slot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("end_time");

                    b.Property<int?>("FieldId")
                        .HasColumnType("int")
                        .HasColumnName("field_id");

                    b.Property<int>("SlotNumber")
                        .HasColumnType("int")
                        .HasColumnName("slot_number");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("start_time");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.ToTable("Slots");
                });

            modelBuilder.Entity("BusinessObject.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(128)")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("email");

                    b.Property<int>("IsAdmin")
                        .HasColumnType("int")
                        .HasColumnName("is_admin");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BusinessObject.Booking", b =>
                {
                    b.HasOne("BusinessObject.User", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessObject.BookingDetail", b =>
                {
                    b.HasOne("BusinessObject.Booking", "Booking")
                        .WithMany("BookingDetails")
                        .HasForeignKey("BookingId");

                    b.HasOne("BusinessObject.Field", "Field")
                        .WithMany("BookingDetails")
                        .HasForeignKey("FieldId");

                    b.HasOne("BusinessObject.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Booking");

                    b.Navigation("Field");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessObject.Feedback", b =>
                {
                    b.HasOne("BusinessObject.Field", "Field")
                        .WithMany("Feedbacks")
                        .HasForeignKey("FieldId");

                    b.HasOne("BusinessObject.User", "User")
                        .WithMany("Feedbacks")
                        .HasForeignKey("UserId");

                    b.Navigation("Field");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessObject.Field", b =>
                {
                    b.HasOne("BusinessObject.Category", "Category")
                        .WithMany("Fields")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BusinessObject.Slot", b =>
                {
                    b.HasOne("BusinessObject.Field", "Field")
                        .WithMany("Slots")
                        .HasForeignKey("FieldId");

                    b.Navigation("Field");
                });

            modelBuilder.Entity("BusinessObject.Booking", b =>
                {
                    b.Navigation("BookingDetails");
                });

            modelBuilder.Entity("BusinessObject.Category", b =>
                {
                    b.Navigation("Fields");
                });

            modelBuilder.Entity("BusinessObject.Field", b =>
                {
                    b.Navigation("BookingDetails");

                    b.Navigation("Feedbacks");

                    b.Navigation("Slots");
                });

            modelBuilder.Entity("BusinessObject.User", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Feedbacks");
                });
#pragma warning restore 612, 618
        }
    }
}
