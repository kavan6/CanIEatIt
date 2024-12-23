using CanIEatIt.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using NUglify.Helpers;

namespace CanIEatIt.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CanIEatItContext(serviceProvider.GetRequiredService<DbContextOptions<CanIEatItContext>>()))
            {
				if (!context.Users.Where(user => user.UserName == "Admin").Any())
				{
					context.Users.Add(
						new IdentityUser
						{
							UserName = "Admin",
							NormalizedUserName = "ADMIN"
						}
					);
					context.SaveChanges();
				}
				if (!context.Users.Where(user => user.UserName == "BasicUser").Any())
				{
					context.Users.Add(
						new IdentityUser
						{
							UserName = "BasicUser",
							NormalizedUserName = "BASICUSER"
						}
					);
					context.SaveChanges();
				}
				if (!context.Roles.Where(role=>role.Name == "Admin").Any())
                {
                    context.Roles.Add(
                        new IdentityRole
                        {
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        }
                    );
					context.SaveChanges();
				}
				if (!context.Roles.Where(role => role.Name == "BasicUser").Any())
				{
					context.Roles.Add(
						new IdentityRole
						{
							Name = "BasicUser",
							NormalizedName = "BASICUSER"
						}
					);
					context.SaveChanges();
				}

				context.UserRoles.ForEach(ur => context.UserRoles.Remove(ur));
                context.UserRoles.AddRange(
                    new IdentityUserRole<string>
                    {
                        UserId = context.Users.Where(u=>u.UserName=="Admin").First().Id,
                        RoleId = context.Roles.Where(r=>r.Name == "Admin").First().Id
					},
					new IdentityUserRole<string>
					{
						UserId = context.Users.Where(u => u.UserName == "BasicUser").First().Id,
						RoleId = context.Roles.Where(r => r.Name == "BasicUser").First().Id
					}
				);
                context.SaveChanges();
				if (context.Mushroom.Any())
                {
                    return;
                }
                context.Mushroom.AddRange(
                    new Mushroom
                    {
                        Name = "Agaricus Arvensis (Horse Mushroom)",
                        Family = "Agaricaceae",
                        Location = "Europe and parts of North America and Asia.",
                        CapDiameter = "8-20cm",
                        LowerDiameter = 8,
                        UpperDiameter = 20,
                        StemHeight = "8-10cm",
                        LowerHeight = 8,
                        UpperHeight = 10,
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
                        LowerDiameter = 8,
                        UpperDiameter = 20,
                        StemHeight = "5-20cm",
                        LowerHeight = 5,
                        UpperHeight = 20,
                        Edible = false,
                        EdibleDescription = "Poisonous and psychoactive. Can be eaten after parboiling twice in water.",
                        CapDescription = "Bright red, often covered in white to yellow warts or spots, and ranging from globose to flat depending on age.",
                        StemDescription = "White with a bulb at the bottom.",
                        GillDescription = "Free, white gills.",
                        SporeDescription = "White spore print.",
                        MicroscopicDescription = "Oval spores that do not turn blue with the application of iodine.",
                        Note = "Mild earthy smell."
                    },

                    new Mushroom
                    {
                        Name = "Agaricus Augustus (the prince)",
                        Family = "Agaricaceae",
                        Location = "Europe, Asia, Northern Africa, and many parts of North America.",
                        CapDiameter = "8-30cm",
                        LowerDiameter = 8,
                        UpperDiameter = 30,
                        StemHeight = "7-30cm",
                        LowerHeight = 7,
                        UpperHeight = 30,
                        Edible = true,
                        EdibleDescription = "Very tasty but can accumulate cadmium so pick away from built up areas.",
                        CapDescription = "Hemispherical to nearly flat, covered in brown coloured scales set over a white to yellow background, thick firm white flesh that can turn yellow when bruised.",
                        StemDescription = "Solid and cylindrical to club-shaped, whitish above the ring, below the ring covered in brownish scales.",
                        GillDescription = "Detached from the stem, crowded and pallid to pink then dark brown with age.",
                        SporeDescription = "Purple-brown.",
                        MicroscopicDescription = "Ellipsoidal in shape, with a smooth surface.",
                        Note = "Found in deciduous and coniferous woods, fruiting in late summer to autumn in Europe."
                    }

                    );
                context.SaveChanges();
            }
        }
    }
}
