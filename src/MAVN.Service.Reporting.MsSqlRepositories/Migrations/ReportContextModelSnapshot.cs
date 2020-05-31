﻿// <auto-generated />
using System;
using MAVN.Service.Reporting.MsSqlRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MAVN.Service.Reporting.MsSqlRepositories.Migrations
{
    [DbContext(typeof(ReportContext))]
    partial class ReportContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("report")
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MAVN.Service.Reporting.MsSqlRepositories.Entities.TransactionReportEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasColumnName("amount");

                    b.Property<Guid?>("CampaignId")
                        .HasColumnName("campaign_id");

                    b.Property<string>("CampaignName")
                        .HasColumnName("campaign_name");

                    b.Property<string>("Currency")
                        .HasColumnName("currency");

                    b.Property<string>("Info")
                        .HasColumnName("info");

                    b.Property<string>("LocationExternalId")
                        .HasColumnName("location_ext_id");

                    b.Property<string>("LocationInfo")
                        .HasColumnName("location_info");

                    b.Property<string>("LocationIntegrationCode")
                        .HasColumnName("location_integration_code");

                    b.Property<string>("PartnerId")
                        .HasColumnName("partner_id");

                    b.Property<string>("PartnerName")
                        .HasColumnName("partner_name");

                    b.Property<string>("ReceiverEmail")
                        .HasColumnName("receiver_email");

                    b.Property<string>("ReceiverName")
                        .HasColumnName("receiver_name");

                    b.Property<string>("SenderEmail")
                        .HasColumnName("sender_email");

                    b.Property<string>("SenderName")
                        .HasColumnName("sender_name");

                    b.Property<string>("Status")
                        .HasColumnName("status");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnName("timestamp");

                    b.Property<string>("TransactionCategory")
                        .HasColumnName("transaction_category");

                    b.Property<string>("TransactionType")
                        .HasColumnName("transaction_type");

                    b.Property<string>("Vertical")
                        .HasColumnName("vertical");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.HasIndex("PartnerId");

                    b.HasIndex("Timestamp");

                    b.ToTable("transactions_report_2");
                });
#pragma warning restore 612, 618
        }
    }
}
