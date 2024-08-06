using CanIEatIt.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CanIEatIt.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CanIEatItContext(serviceProvider.GetRequiredService<DbContextOptions<CanIEatItContext>>()))
            {
                if (context.Mushroom.Any())
                {
                    return;
                }
                context.Mushroom.AddRange(
                    new Mushroom
                    {
                        Name = "Agaricus Arvensis (Horse Mushroom)",
                        Family = "Agaricaceae",
                        Location = "Europe and parts of North America and Asia",
                        CapDiameter = "8-20cm",
                        StemHeight = "8-10cm",
                        Edible = true,
                        EdibleDescription = "Undistinctive taste and tends to accumulate heavy metals so do not eat in excess.",
                        CapDescription = "White to cream, possibly fine scales, hemispherical to flat in shape depending on age. With white, firm flesh which turns yellowish when bruised.",
                        StemDescription = "White to cream, slightly club-shaped and smooth to finely scaly with a ring.",
                        GillDescription = "Free and crowded, pink to chocolate brown or black in colour.",
                        SporeDescription = "Dark purple-brown coloured print.",
                        MicroscopicDescription = "Ellipsoidal spores.",
                        Note = "Beware of toxic lookalike the Yellow Stainer. Strong odor of aniseed, unlike the Yellow Stainer."
                    },

                    new Mushroom
                    {
                        Name = "Amanita Muscaria (fly agaric)",
                        Family = "Amanitaceae",
                        Location = "Native throughout the temperate and boreal regions of the Northern Hemisphere. Invasive to the Southern Hemisphere.",
                        CapDiameter = "8-20cm",
                        StemHeight = "5-20cm",
                        Edible = false,
                        EdibleDescription = "Poisonous and psychoactive. Can be eaten after parboiling twice in water.",
                        CapDescription = "Bright red, often covered in white to yellow warts or spots, and ranging from globose to flat depending on age.",
                        StemDescription = "White with a bulb at the bottom.",
                        GillDescription = "Free, white gills.",
                        SporeDescription = "White spore print.",
                        MicroscopicDescription = "Oval spores that do not turn blue with the application of iodine.",
                        Note = "Mild earthy smell."
                    }

                    );
                context.SaveChanges();
            }
        }
    }
}
