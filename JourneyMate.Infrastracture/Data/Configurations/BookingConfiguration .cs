﻿using JourneyMate.Core.Models.Booking_Aggregation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Infrastracture.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {

            builder.OwnsOne(x => x.Item, item =>
            {
                item.Property(i => i.Price).HasColumnName("Price");
                item.Property(i => i.Quantity).HasColumnName("Quantity");

                item.OwnsOne(i => i.TravelItemBooked, travel =>
                {
                    travel.Property(t => t.TravelId).HasColumnName("TravelId");
                    travel.Property(t => t.Title).HasColumnName("TravelTitle");
                    travel.Property(t => t.TravelProfileUrl).HasColumnName("TravelProfileUrl");
                });
            });

            builder.Property(x => x.TotalCost)
              .HasColumnType("decimal(18,2)");

            builder.Property(x => x.PaymentIntentId)
             .HasMaxLength(100);

            builder.Property(o => o.Status)
               .HasConversion(
               Ostatus => Ostatus.ToString(),
               Ostatus => (BookingStatus)Enum.Parse(typeof(BookingStatus), Ostatus));
        }
    }
}
