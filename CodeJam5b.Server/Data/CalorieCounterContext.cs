using Microsoft.EntityFrameworkCore;
using CodeJam5b.Models;

namespace CodeJam5b.Server.Data
{
    public class CalorieCounterContext : DbContext
    {
        public CalorieCounterContext(DbContextOptions<CalorieCounterContext> options)
            : base(options)
        {
        }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<UserProgress> Progress { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Meal>().HasData(
                // Breakfast Items
                new Meal { MealId = "1", MealName = "Oatmeal with Berries", Calories = 280, Carbs = 48, Fat = 5, Protein = 10 },
                new Meal { MealId = "2", MealName = "Scrambled Eggs", Calories = 220, Carbs = 2, Fat = 16, Protein = 18 },
                new Meal { MealId = "3", MealName = "Greek Yogurt Parfait", Calories = 240, Carbs = 32, Fat = 6, Protein = 15 },
                new Meal { MealId = "4", MealName = "Avocado Toast", Calories = 310, Carbs = 35, Fat = 16, Protein = 10 },
                new Meal { MealId = "5", MealName = "Protein Pancakes", Calories = 380, Carbs = 42, Fat = 12, Protein = 24 },
                new Meal { MealId = "6", MealName = "Breakfast Burrito", Calories = 520, Carbs = 45, Fat = 26, Protein = 28 },
                new Meal { MealId = "7", MealName = "Smoothie Bowl", Calories = 320, Carbs = 56, Fat = 8, Protein = 12 },
                new Meal { MealId = "8", MealName = "French Toast", Calories = 420, Carbs = 58, Fat = 14, Protein = 14 },
                new Meal { MealId = "9", MealName = "Bagel with Cream Cheese", Calories = 360, Carbs = 52, Fat = 12, Protein = 12 },
                new Meal { MealId = "10", MealName = "Egg White Omelet", Calories = 180, Carbs = 4, Fat = 4, Protein = 32 },

                // Lunch Items
                new Meal { MealId = "11", MealName = "Chicken Caesar Salad", Calories = 450, Carbs = 18, Fat = 28, Protein = 36 },
                new Meal { MealId = "12", MealName = "Turkey Sandwich", Calories = 480, Carbs = 52, Fat = 16, Protein = 32 },
                new Meal { MealId = "13", MealName = "Grilled Chicken Wrap", Calories = 420, Carbs = 38, Fat = 18, Protein = 30 },
                new Meal { MealId = "14", MealName = "Tuna Salad", Calories = 320, Carbs = 12, Fat = 18, Protein = 28 },
                new Meal { MealId = "15", MealName = "Veggie Burger", Calories = 380, Carbs = 46, Fat = 14, Protein = 18 },
                new Meal { MealId = "16", MealName = "Chicken Quesadilla", Calories = 560, Carbs = 42, Fat = 28, Protein = 34 },
                new Meal { MealId = "17", MealName = "Sushi Roll (12 pieces)", Calories = 380, Carbs = 62, Fat = 8, Protein = 16 },
                new Meal { MealId = "18", MealName = "BLT Sandwich", Calories = 440, Carbs = 38, Fat = 24, Protein = 20 },
                new Meal { MealId = "19", MealName = "Chicken Noodle Soup", Calories = 240, Carbs = 28, Fat = 6, Protein = 18 },
                new Meal { MealId = "20", MealName = "Cobb Salad", Calories = 520, Carbs = 16, Fat = 36, Protein = 32 },

                // Dinner Items
                new Meal { MealId = "21", MealName = "Grilled Salmon", Calories = 450, Carbs = 5, Fat = 25, Protein = 50 },
                new Meal { MealId = "22", MealName = "Steak with Vegetables", Calories = 580, Carbs = 22, Fat = 32, Protein = 48 },
                new Meal { MealId = "23", MealName = "Chicken Stir Fry", Calories = 420, Carbs = 38, Fat = 16, Protein = 34 },
                new Meal { MealId = "24", MealName = "Spaghetti Bolognese", Calories = 620, Carbs = 78, Fat = 18, Protein = 32 },
                new Meal { MealId = "25", MealName = "Shrimp Tacos", Calories = 480, Carbs = 42, Fat = 20, Protein = 32 },
                new Meal { MealId = "26", MealName = "BBQ Ribs", Calories = 680, Carbs = 28, Fat = 42, Protein = 46 },
                new Meal { MealId = "27", MealName = "Grilled Chicken Breast", Calories = 320, Carbs = 2, Fat = 8, Protein = 58 },
                new Meal { MealId = "28", MealName = "Beef Tacos (3)", Calories = 540, Carbs = 48, Fat = 24, Protein = 36 },
                new Meal { MealId = "29", MealName = "Teriyaki Chicken", Calories = 460, Carbs = 52, Fat = 12, Protein = 36 },
                new Meal { MealId = "30", MealName = "Fish and Chips", Calories = 720, Carbs = 68, Fat = 36, Protein = 32 },

                // Vegetarian Options
                new Meal { MealId = "31", MealName = "Veggie Stir Fry", Calories = 300, Carbs = 50, Fat = 10, Protein = 15 },
                new Meal { MealId = "32", MealName = "Lentil Soup", Calories = 280, Carbs = 42, Fat = 6, Protein = 18 },
                new Meal { MealId = "33", MealName = "Caprese Salad", Calories = 320, Carbs = 12, Fat = 22, Protein = 16 },
                new Meal { MealId = "34", MealName = "Vegetable Curry", Calories = 380, Carbs = 52, Fat = 14, Protein = 12 },
                new Meal { MealId = "35", MealName = "Quinoa Bowl", Calories = 420, Carbs = 58, Fat = 14, Protein = 16 },
                new Meal { MealId = "36", MealName = "Falafel Wrap", Calories = 480, Carbs = 62, Fat = 18, Protein = 16 },
                new Meal { MealId = "37", MealName = "Margherita Pizza (2 slices)", Calories = 540, Carbs = 68, Fat = 20, Protein = 22 },
                new Meal { MealId = "38", MealName = "Black Bean Burger", Calories = 360, Carbs = 48, Fat = 12, Protein = 16 },
                new Meal { MealId = "39", MealName = "Spinach and Ricotta Ravioli", Calories = 480, Carbs = 62, Fat = 16, Protein = 20 },
                new Meal { MealId = "40", MealName = "Tofu Scramble", Calories = 260, Carbs = 18, Fat = 14, Protein = 20 },

                // Snacks
                new Meal { MealId = "41", MealName = "Apple with Peanut Butter", Calories = 220, Carbs = 28, Fat = 10, Protein = 6 },
                new Meal { MealId = "42", MealName = "Protein Bar", Calories = 240, Carbs = 28, Fat = 8, Protein = 20 },
                new Meal { MealId = "43", MealName = "Trail Mix", Calories = 180, Carbs = 16, Fat = 12, Protein = 6 },
                new Meal { MealId = "44", MealName = "Hummus with Carrots", Calories = 160, Carbs = 18, Fat = 8, Protein = 6 },
                new Meal { MealId = "45", MealName = "String Cheese", Calories = 80, Carbs = 1, Fat = 6, Protein = 7 },
                new Meal { MealId = "46", MealName = "Banana", Calories = 105, Carbs = 27, Fat = 0, Protein = 1 },
                new Meal { MealId = "47", MealName = "Greek Yogurt", Calories = 130, Carbs = 10, Fat = 4, Protein = 16 },
                new Meal { MealId = "48", MealName = "Almonds (1 oz)", Calories = 164, Carbs = 6, Fat = 14, Protein = 6 },
                new Meal { MealId = "49", MealName = "Rice Cakes with Almond Butter", Calories = 200, Carbs = 24, Fat = 10, Protein = 6 },
                new Meal { MealId = "50", MealName = "Hard Boiled Eggs (2)", Calories = 140, Carbs = 2, Fat = 10, Protein = 12 },

                // Fast Food
                new Meal { MealId = "51", MealName = "Big Mac", Calories = 563, Carbs = 46, Fat = 30, Protein = 26 },
                new Meal { MealId = "52", MealName = "Chicken Nuggets (10 pc)", Calories = 440, Carbs = 28, Fat = 26, Protein = 24 },
                new Meal { MealId = "53", MealName = "Medium Fries", Calories = 340, Carbs = 44, Fat = 16, Protein = 4 },
                new Meal { MealId = "54", MealName = "Cheeseburger", Calories = 300, Carbs = 32, Fat = 12, Protein = 15 },
                new Meal { MealId = "55", MealName = "Chicken Sandwich", Calories = 420, Carbs = 42, Fat = 16, Protein = 28 },
                new Meal { MealId = "56", MealName = "Pepperoni Pizza (2 slices)", Calories = 620, Carbs = 72, Fat = 26, Protein = 26 },
                new Meal { MealId = "57", MealName = "Chipotle Burrito Bowl", Calories = 580, Carbs = 62, Fat = 22, Protein = 36 },
                new Meal { MealId = "58", MealName = "Subway Footlong Turkey", Calories = 560, Carbs = 86, Fat = 10, Protein = 38 },
                new Meal { MealId = "59", MealName = "Taco Bell Crunchwrap", Calories = 530, Carbs = 71, Fat = 21, Protein = 16 },
                new Meal { MealId = "60", MealName = "KFC Fried Chicken (2 pc)", Calories = 520, Carbs = 18, Fat = 32, Protein = 38 },

                // Seafood
                new Meal { MealId = "61", MealName = "Grilled Tilapia", Calories = 280, Carbs = 2, Fat = 8, Protein = 48 },
                new Meal { MealId = "62", MealName = "Shrimp Scampi", Calories = 380, Carbs = 24, Fat = 20, Protein = 28 },
                new Meal { MealId = "63", MealName = "Tuna Steak", Calories = 320, Carbs = 0, Fat = 6, Protein = 62 },
                new Meal { MealId = "64", MealName = "Lobster Tail", Calories = 230, Carbs = 2, Fat = 4, Protein = 48 },
                new Meal { MealId = "65", MealName = "Crab Cakes", Calories = 380, Carbs = 22, Fat = 20, Protein = 26 },
                new Meal { MealId = "66", MealName = "Mahi Mahi", Calories = 290, Carbs = 1, Fat = 6, Protein = 54 },
                new Meal { MealId = "67", MealName = "Fish Tacos (2)", Calories = 420, Carbs = 38, Fat = 18, Protein = 28 },
                new Meal { MealId = "68", MealName = "Clam Chowder", Calories = 340, Carbs = 28, Fat = 18, Protein = 16 },
                new Meal { MealId = "69", MealName = "Cajun Shrimp Pasta", Calories = 620, Carbs = 68, Fat = 24, Protein = 34 },
                new Meal { MealId = "70", MealName = "Cod with Rice", Calories = 420, Carbs = 52, Fat = 8, Protein = 38 },

                // Asian Cuisine
                new Meal { MealId = "71", MealName = "Pad Thai", Calories = 520, Carbs = 72, Fat = 18, Protein = 22 },
                new Meal { MealId = "72", MealName = "General Tso's Chicken", Calories = 680, Carbs = 82, Fat = 28, Protein = 32 },
                new Meal { MealId = "73", MealName = "Beef and Broccoli", Calories = 480, Carbs = 32, Fat = 24, Protein = 36 },
                new Meal { MealId = "74", MealName = "Fried Rice", Calories = 460, Carbs = 68, Fat = 16, Protein = 12 },
                new Meal { MealId = "75", MealName = "Spring Rolls (4)", Calories = 320, Carbs = 42, Fat = 12, Protein = 10 },
                new Meal { MealId = "76", MealName = "Chicken Teriyaki Bowl", Calories = 540, Carbs = 78, Fat = 12, Protein = 32 },
                new Meal { MealId = "77", MealName = "Ramen Noodles", Calories = 580, Carbs = 72, Fat = 22, Protein = 24 },
                new Meal { MealId = "78", MealName = "Dim Sum Platter", Calories = 420, Carbs = 48, Fat = 18, Protein = 16 },
                new Meal { MealId = "79", MealName = "Sweet and Sour Pork", Calories = 560, Carbs = 68, Fat = 20, Protein = 28 },
                new Meal { MealId = "80", MealName = "Miso Soup with Tofu", Calories = 140, Carbs = 12, Fat = 6, Protein = 10 },

                // Mexican Cuisine
                new Meal { MealId = "81", MealName = "Chicken Enchiladas", Calories = 520, Carbs = 48, Fat = 22, Protein = 32 },
                new Meal { MealId = "82", MealName = "Beef Burrito", Calories = 680, Carbs = 72, Fat = 28, Protein = 38 },
                new Meal { MealId = "83", MealName = "Nachos Supreme", Calories = 720, Carbs = 62, Fat = 42, Protein = 24 },
                new Meal { MealId = "84", MealName = "Quesadilla", Calories = 480, Carbs = 38, Fat = 26, Protein = 24 },
                new Meal { MealId = "85", MealName = "Carnitas Tacos (3)", Calories = 540, Carbs = 42, Fat = 28, Protein = 32 },
                new Meal { MealId = "86", MealName = "Chile Relleno", Calories = 420, Carbs = 32, Fat = 24, Protein = 20 },
                new Meal { MealId = "87", MealName = "Tamales (2)", Calories = 380, Carbs = 48, Fat = 16, Protein = 12 },
                new Meal { MealId = "88", MealName = "Taco Salad", Calories = 560, Carbs = 42, Fat = 32, Protein = 28 },
                new Meal { MealId = "89", MealName = "Guacamole with Chips", Calories = 340, Carbs = 38, Fat = 20, Protein = 4 },
                new Meal { MealId = "90", MealName = "Fajitas (Chicken)", Calories = 480, Carbs = 38, Fat = 20, Protein = 36 },

                // Desserts
                new Meal { MealId = "91", MealName = "Chocolate Chip Cookie", Calories = 160, Carbs = 22, Fat = 8, Protein = 2 },
                new Meal { MealId = "92", MealName = "Vanilla Ice Cream (1 cup)", Calories = 280, Carbs = 36, Fat = 14, Protein = 4 },
                new Meal { MealId = "93", MealName = "Apple Pie Slice", Calories = 320, Carbs = 48, Fat = 14, Protein = 3 },
                new Meal { MealId = "94", MealName = "Brownie", Calories = 240, Carbs = 32, Fat = 12, Protein = 3 },
                new Meal { MealId = "95", MealName = "Cheesecake Slice", Calories = 420, Carbs = 38, Fat = 28, Protein = 8 },
                new Meal { MealId = "96", MealName = "Chocolate Cake", Calories = 380, Carbs = 52, Fat = 18, Protein = 5 },
                new Meal { MealId = "97", MealName = "Tiramisu", Calories = 340, Carbs = 42, Fat = 16, Protein = 6 },
                new Meal { MealId = "98", MealName = "Fruit Salad", Calories = 120, Carbs = 30, Fat = 0, Protein = 2 },
                new Meal { MealId = "99", MealName = "Frozen Yogurt", Calories = 180, Carbs = 32, Fat = 4, Protein = 6 },
                new Meal { MealId = "100", MealName = "Protein Cookie", Calories = 220, Carbs = 28, Fat = 8, Protein = 16 }
             );

            modelBuilder.Entity<UserProgress>().HasData(
                new UserProgress 
                { 
                    ProgressId = "1",
                    CurrentWeight = 185,
                    TargetWeight = 165,
                    TargetCals = 2100,
                    TargetCarbs = 240,
                    TargetFat = 70,
                    TargetProtein = 150
                }    
            );

        }
    
    }
    
}
