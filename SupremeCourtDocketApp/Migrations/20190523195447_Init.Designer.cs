﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SupremeCourtDocketApp.Models;

namespace SupremeCourtDocketApp.Migrations
{
    [DbContext(typeof(SupremeCourtDocketAppContext))]
    [Migration("20190523195447_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("SupremeCourtDocketApp.Models.DocketContacts", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressBlock");

                    b.Property<string>("AttorneyCity");

                    b.Property<string>("AttorneyCityStateZip");

                    b.Property<string>("AttorneyEmail");

                    b.Property<string>("AttorneyFullName");

                    b.Property<string>("AttorneyOffice");

                    b.Property<string>("AttorneyState");

                    b.Property<string>("AttorneyStreetAddress");

                    b.Property<string>("AttorneySurname");

                    b.Property<string>("AttorneyZip");

                    b.Property<bool>("IsCityStateZipValid");

                    b.Property<bool>("IsCounselOfRecord");

                    b.Property<string>("NameBlock");

                    b.Property<string>("PartyDescription");

                    b.Property<string>("PartyFooter");

                    b.Property<string>("PartyHeader");

                    b.Property<string>("PartyName");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("PhoneNumberTenDigit");

                    b.Property<int>("SupremeCourtDocketID");

                    b.HasKey("ID");

                    b.HasIndex("SupremeCourtDocketID");

                    b.ToTable("DocketContacts");
                });

            modelBuilder.Entity("SupremeCourtDocketApp.Models.DocketInfoLink", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Link");

                    b.Property<int?>("SupremeCourtDocketID");

                    b.Property<string>("Text");

                    b.HasKey("ID");

                    b.HasIndex("SupremeCourtDocketID");

                    b.ToTable("DocketInfoLink");
                });

            modelBuilder.Entity("SupremeCourtDocketApp.Models.DocketProceedings", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ProceedingDate");

                    b.Property<string>("ProceedingDescription");

                    b.Property<DateTime>("SecondaryDate");

                    b.Property<int>("SupremeCourtDocketID");

                    b.Property<int>("TypeOfProceeding");

                    b.HasKey("ID");

                    b.HasIndex("SupremeCourtDocketID");

                    b.ToTable("DocketProceedings");
                });

            modelBuilder.Entity("SupremeCourtDocketApp.Models.ProceedingLink", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DocketProceedingsID");

                    b.Property<string>("Link");

                    b.Property<string>("LinkDescription");

                    b.HasKey("ID");

                    b.HasIndex("DocketProceedingsID");

                    b.ToTable("ProceedingLink");
                });

            modelBuilder.Entity("SupremeCourtDocketApp.Models.SupremeCourtDocket", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Analyst");

                    b.Property<string>("CaseTitle");

                    b.Property<DateTime?>("DateDocketed");

                    b.Property<DateTime?>("DateOfDecision");

                    b.Property<DateTime?>("DateOfDiscretionaryCourtDecision");

                    b.Property<DateTime?>("DateOfRehearingDenied");

                    b.Property<DateTime>("DateRetrieved");

                    b.Property<int?>("DocketNoYear");

                    b.Property<string>("DocketNumber");

                    b.Property<int?>("DocketYear");

                    b.Property<string>("LowerCourt");

                    b.Property<string>("LowerCourtCaseNumbers");

                    b.Property<string>("WebAddress");

                    b.Property<string>("WebPage");

                    b.HasKey("ID");

                    b.ToTable("SupremeCourtDocket");
                });

            modelBuilder.Entity("SupremeCourtDocketApp.Models.DocketContacts", b =>
                {
                    b.HasOne("SupremeCourtDocketApp.Models.SupremeCourtDocket")
                        .WithMany("Contacts")
                        .HasForeignKey("SupremeCourtDocketID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SupremeCourtDocketApp.Models.DocketInfoLink", b =>
                {
                    b.HasOne("SupremeCourtDocketApp.Models.SupremeCourtDocket")
                        .WithMany("DocketInfoLinks")
                        .HasForeignKey("SupremeCourtDocketID");
                });

            modelBuilder.Entity("SupremeCourtDocketApp.Models.DocketProceedings", b =>
                {
                    b.HasOne("SupremeCourtDocketApp.Models.SupremeCourtDocket")
                        .WithMany("Proceedings")
                        .HasForeignKey("SupremeCourtDocketID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SupremeCourtDocketApp.Models.ProceedingLink", b =>
                {
                    b.HasOne("SupremeCourtDocketApp.Models.DocketProceedings")
                        .WithMany("ProceedingLinks")
                        .HasForeignKey("DocketProceedingsID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
